using Microsoft.AspNetCore.SignalR;
using PlanShare.Api.Hubs.Services;
using PlanShare.Application.UseCases.User.Connection.GenerateCode;
using PlanShare.Application.UseCases.User.Connection.JoinWithCode;
using PlanShare.Communication.Responses;

namespace PlanShare.Api.Hubs;

public class UserConnectionsHub(CodeConnectionService codeConnectionService, 
    IGenerateCodeUserConnectionUseCase generateCodeUserConnectionUseCase,
    IJoinWithCodeUseCase joinWithCodeUseCase) : Hub
{
    public async Task<string> GenerateCode()
    {
        var codeUserConnectionDto = await generateCodeUserConnectionUseCase.Execute();

        codeConnectionService.Start(codeUserConnectionDto, Context.ConnectionId);

        return codeUserConnectionDto.Code;
    }

    public async Task JoinWithCode(string code)
    {
        var userConnections = codeConnectionService.GetConnectionByCode(code);

        var response = await joinWithCodeUseCase.Execute(userConnections.UserId);

        userConnections.ConnectingUserId = response.Id;
        userConnections.ConnectingUserConnectionId = Context.ConnectionId;

        await Clients.Client(userConnections.UserConnectionId).SendAsync("OnUserJoined", new ResponseConnectionUserJson 
        { 
            Name = response.Name, 
            ProfilePhotoUrl = response.ProfilePhotoUrl 
        });
    }

    public async Task Cancel(string code)
    {
        var connection = codeConnectionService.RemoveConnection(code);
        if (connection is not null && connection.ConnectingUserId.HasValue)
            await Clients.Client(connection.ConnectingUserConnectionId!).SendAsync("OnCancelled");
    }
}
