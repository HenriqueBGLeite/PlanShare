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

public class UserConnectionsHub(UserConnectionsService codeConnectionService, 
    IGenerateCodeUserConnectionUseCase generateCodeUserConnectionUseCase,
    IJoinWithCodeUseCase joinWithCodeUseCase,
    IApproveCodeUserConnectionUseCase approveCodeUserConnectionUseCase,
    ICancelCodeUserConnectionUseCase cancelCodeUserConnectionUseCase) : Hub
{
    public async Task<HubOperationResult<string>> GenerateCode()
    {
        var (code, generator) = await generateCodeUserConnectionUseCase.Execute();

        codeConnectionService.Start(code, generator, Context.ConnectionId);

        return HubOperationResult<string>.Success(code);
    }

    public async Task<HubOperationResult<string>> JoinWithCode(string code)
    {
        var userConnections = codeConnectionService.GetConnectionByCode(code);
        if (userConnections is null)
            return HubOperationResult<string>.Failure(ResourceMessagesException.PROVIDED_CODE_DOES_NOT_EXIST, UserConnectionErrorCode.InvalidCode);

        if (userConnections.Joiner is not null)
            return HubOperationResult<string>.Failure(ResourceMessagesException.CODE_ALREADY_LINKED_ANOTHER_CONNECTION, UserConnectionErrorCode.InvalidCode);

        var result = await joinWithCodeUseCase.Execute(userConnections.Generator);
        if (result.IsSuccess.IsFalse())
            return HubOperationResult<string>.Failure(result.ErrorMessage, result.ErrorCode!.Value);

        userConnections.Joiner = result.Response;
        userConnections.JoinerConnectionId = Context.ConnectionId;

        await Clients.Client(userConnections.GeneratorConnectionId).SendAsync("OnUserJoined", new ResponseConnectionUserJson 
        { 
            Name = result.Response!.Name, 
            ProfilePhotoUrl = result.Response.ProfilePhotoUrl 
        });

        return HubOperationResult<string>.Success(userConnections.Generator.Name);
    }

    public async Task<HubOperationResult<string>> Cancel(string code)
    {
        var userConnections = codeConnectionService.RemoveConnectionByCode(code);
        if (userConnections is null)
            return HubOperationResult<string>.Failure(ResourceMessagesException.PROVIDED_CODE_DOES_NOT_EXIST, UserConnectionErrorCode.InvalidCode);

        var result = await cancelCodeUserConnectionUseCase.Execute(userConnections);
        if (result.IsSuccess.IsFalse())
            return HubOperationResult<string>.Failure(result.ErrorMessage, result.ErrorCode!.Value);

        if (userConnections.JoinerConnectionId.NotEmpty())
            await Clients.Client(userConnections.JoinerConnectionId!).SendAsync("OnCancelled");

        codeConnectionService.RemoveCodeByConnectionId(Context.ConnectionId);

        return HubOperationResult<string>.Success(code);
    }

    public async Task<HubOperationResult<string>> ConfirmCodeJoin(string code)
    {
        var userConnections = codeConnectionService.RemoveConnectionByCode(code);
        if (userConnections is null)
            return HubOperationResult<string>.Failure(ResourceMessagesException.PROVIDED_CODE_DOES_NOT_EXIST, UserConnectionErrorCode.InvalidCode);

        var result = await approveCodeUserConnectionUseCase.Execute(userConnections);
        if (result.IsSuccess.IsFalse())
            return HubOperationResult<string>.Failure(result.ErrorMessage, result.ErrorCode!.Value);

        await Clients.Client(userConnections.JoinerConnectionId!).SendAsync("OnConnectionConfirmed");

        codeConnectionService.RemoveCodeByConnectionId(Context.ConnectionId);

        return HubOperationResult<string>.Success(code);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var code = codeConnectionService.RemoveCodeByConnectionId(Context.ConnectionId);
        if (code.NotEmpty())
        {
            var connection = codeConnectionService.RemoveConnectionByCode(code);
            if (connection is not null && connection.JoinerConnectionId.NotEmpty())
                Clients.Client(connection.JoinerConnectionId).SendAsync("OnUserDisconnected");
        }

        return base.OnDisconnectedAsync(exception);
    }
}
