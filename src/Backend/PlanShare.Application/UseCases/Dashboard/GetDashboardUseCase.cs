using Mapster;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Repositories.Connection;
using PlanShare.Domain.Services.LoggedUser;

namespace PlanShare.Application.UseCases.Dashboard;
public class GetDashboardUseCase(ILoggedUser loggedUser, IUserConnectionReadOnlyRepository userConnectionRepository) : IGetDashboardUseCase
{
    public async Task<ResponseDashboardJson> Execute()
    {
        var userLogged = await loggedUser.Get();

        var connections = await userConnectionRepository.GetUserConnectionsForUser(userLogged);

        return new ResponseDashboardJson
        {
            UserName = userLogged.Name,
            ConnectedUsers = connections.Adapt<List<ResponseAssigneeJson>>()
        };
    }
}
