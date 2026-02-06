namespace PlanShare.Domain.Repositories.Connection;

public interface IUserConnectionWriteOnlyRepository
{
    Task Add(Entities.UserConnection userConnection);
}
