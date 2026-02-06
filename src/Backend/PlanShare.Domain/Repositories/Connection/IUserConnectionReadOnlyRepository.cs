namespace PlanShare.Domain.Repositories.Connection;
public interface IUserConnectionReadOnlyRepository
{
    Task<List<Entities.User>> GetUserConnectionsForUser(Entities.User user);
    Task<bool> AreUsersConnected(Entities.User user1, Entities.User user2);
}
