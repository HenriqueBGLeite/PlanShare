using PlanShare.Domain.Dtos;
using PlanShare.Domain.Services.LoggedUser;
using System.Security.Cryptography;

namespace PlanShare.Application.UseCases.User.Connection.GenerateCode;

public class GenerateCodeUserConnectionUseCase(ILoggedUser loggedUser) : IGenerateCodeUserConnectionUseCase
{
    public async Task<(string code, UserDto generator)> Execute()
    {
        var userLogged = await loggedUser.Get();

        var code = RandomNumberGenerator.GetInt32(fromInclusive: 1, toExclusive: 1_000_000).ToString("D6");

        return (code, new UserDto(userLogged.Id, userLogged.Name, string.Empty));
    }
}
