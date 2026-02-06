using PlanShare.Domain.Dtos;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.Connection;

namespace PlanShare.Application.UseCases.User.Connection.ApproveCode;

public class ApproveCodeUserConnectionUseCase(IUserConnectionWriteOnlyRepository repository, IUnitOfWork unitOfWork) : IApproveCodeUserConnectionUseCase
{
    public async Task Execute(UserConnectionsDto userConnections)
    {
        var connection = new Domain.Entities.UserConnection
        {
            UserId = userConnections.UserId,
            ConnectedUserId = userConnections.ConnectingUserId.Value
        };

        await repository.Add(connection);

        await unitOfWork.Commit();
    }
}
