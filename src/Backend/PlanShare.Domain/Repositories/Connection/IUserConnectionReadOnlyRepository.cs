namespace PlanShare.Domain.Repositories.Connection;
public interface IUserConnectionReadOnlyRepository
{
    Task<List<Entities.User>> GetUserConnectionsForUser(Entities.User user);
}
