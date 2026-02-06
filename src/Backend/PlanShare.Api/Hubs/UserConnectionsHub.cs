using Microsoft.AspNetCore.SignalR;
using PlanShare.Application.UseCases.User.Connection.GenerateCode;

namespace PlanShare.Api.Hubs;

public class UserConnectionsHub(IGenerateCodeUserConnectionUseCase generateCodeUserConnectionUseCase) : Hub
{
    public async Task<string> GenerateCode()
    {
        var codeUserConnectionDto = await generateCodeUserConnectionUseCase.Execute();

        return codeUserConnectionDto.Code;
    }
}
