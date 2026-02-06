using PlanShare.Domain.Dtos;
using PlanShare.Domain.Services.LoggedUser;

namespace PlanShare.Application.UseCases.User.Connection.JoinWithCode;

public class JoinWithCodeUseCase(ILoggedUser loggedUser) : IJoinWithCodeUseCase
{
    public async Task<ConnectingUserDto> Execute(Guid generatedById)
    {
        var userLogged = await loggedUser.Get();

        return new ConnectingUserDto(userLogged.Id, userLogged.Name, string.Empty);
    }
}
