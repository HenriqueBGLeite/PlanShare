using Mapster;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Repositories.Connection;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Services.LoggedUser;

namespace PlanShare.Application.UseCases.Dashboard;
public class GetDashboardUseCase(ILoggedUser loggedUser, IWorkItemReadOnlyRepository workItemRepository, IUserConnectionReadOnlyRepository personAssociationRepository) : IGetDashboardUseCase
{
    public async Task<ResponseDashboardJson> Execute()
    {
        var userLogged = await loggedUser.Get();

        var workItems = await workItemRepository.GetAll(userLogged);
        var connections = await personAssociationRepository.GetUserConnectionsForUser(userLogged);

        return new ResponseDashboardJson
        {
            WorkItems = workItems.Adapt<List<ResponseShortWorkItemJson>>(),
            Friends = connections.Adapt<List<ResponseAssigneeJson>>()
        };
    }
}
