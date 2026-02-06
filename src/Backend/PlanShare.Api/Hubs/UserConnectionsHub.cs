using Microsoft.AspNetCore.SignalR;
using PlanShare.Api.Hubs.Services;
using PlanShare.Application.UseCases.User.Connection.ApproveCode;
using PlanShare.Application.UseCases.User.Connection.CancelCode;
using PlanShare.Application.UseCases.User.Connection.GenerateCode;
using PlanShare.Application.UseCases.User.Connection.JoinWithCode;
using PlanShare.Communication.Enums;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Extensions;
using PlanShare.Exceptions;

namespace PlanShare.Api.Hubs;

public class UserConnectionsHub(CodeConnectionService codeConnectionService, 
    IGenerateCodeUserConnectionUseCase generateCodeUserConnectionUseCase,
    IJoinWithCodeUseCase joinWithCodeUseCase,
    IApproveCodeUserConnectionUseCase approveCodeUserConnectionUseCase,
    ICancelCodeUserConnectionUseCase cancelCodeUserConnectionUseCase) : Hub
{
    public async Task<HubOperationResult<string>> GenerateCode()
    {
        throw new Exception("Teste Exception");

        var codeUserConnectionDto = await generateCodeUserConnectionUseCase.Execute();

        codeConnectionService.Start(codeUserConnectionDto, Context.ConnectionId);

        return HubOperationResult<string>.Success(codeUserConnectionDto.Code);
    }

    public async Task<HubOperationResult<string>> JoinWithCode(string code)
    {
        var userConnections = codeConnectionService.GetConnectionByCode(code);
        if (userConnections is null)
            return HubOperationResult<string>.Failure(ResourceMessagesException.PROVIDED_CODE_DOES_NOT_EXIST, UserConnectionErrorCode.InvalidCode);

        var result = await joinWithCodeUseCase.Execute(userConnections.UserId);
        if (result.IsSuccess.IsFalse())
            return HubOperationResult<string>.Failure(result.ErrorMessage, result.ErrorCode!.Value);

        userConnections.ConnectingUserId = result.Response!.Connector.Id;
        userConnections.ConnectingUserConnectionId = Context.ConnectionId;

        await Clients.Client(userConnections.UserConnectionId).SendAsync("OnUserJoined", new ResponseConnectionUserJson 
        { 
            Name = result.Response.Connector.Name, 
            ProfilePhotoUrl = result.Response.Connector.ProfilePhotoUrl 
        });

        return HubOperationResult<string>.Success(result.Response.Generator.Name);
    }

    public async Task<HubOperationResult<string>> Cancel(string code)
    {
        var userConnections = codeConnectionService.RemoveConnection(code);
        if (userConnections is null)
            return HubOperationResult<string>.Failure(ResourceMessagesException.PROVIDED_CODE_DOES_NOT_EXIST, UserConnectionErrorCode.InvalidCode);

        var result = await cancelCodeUserConnectionUseCase.Execute(userConnections);
        if (result.IsSuccess.IsFalse())
            return HubOperationResult<string>.Failure(result.ErrorMessage, result.ErrorCode!.Value);

        if (userConnections.ConnectingUserId.HasValue)
            await Clients.Client(userConnections.ConnectingUserConnectionId!).SendAsync("OnCancelled");

        return HubOperationResult<string>.Success(code);
    }

    public async Task<HubOperationResult<string>> ConfirmCodeJoin(string code)
    {
        var userConnections = codeConnectionService.RemoveConnection(code);
        if (userConnections is null)
            return HubOperationResult<string>.Failure(ResourceMessagesException.PROVIDED_CODE_DOES_NOT_EXIST, UserConnectionErrorCode.InvalidCode);

        var result = await approveCodeUserConnectionUseCase.Execute(userConnections);
        if (result.IsSuccess.IsFalse())
            return HubOperationResult<string>.Failure(result.ErrorMessage, result.ErrorCode!.Value);

        await Clients.Client(userConnections.ConnectingUserConnectionId!).SendAsync("OnConnectionConfirmed");

        return HubOperationResult<string>.Success(code);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var code = codeConnectionService.GetCodeByConnectionId(Context.ConnectionId);
        if (code.NotEmpty())
        {
            var connection = codeConnectionService.RemoveConnection(code);
            if (connection is not null && connection.ConnectingUserConnectionId.NotEmpty())
            {
                Clients.Client(connection.ConnectingUserConnectionId).SendAsync("OnUserDisconnected");
            }
        }

        return base.OnDisconnectedAsync(exception);
    }
}
