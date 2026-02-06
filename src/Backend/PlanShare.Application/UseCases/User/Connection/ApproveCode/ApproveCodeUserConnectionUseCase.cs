using PlanShare.Communication.Enums;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Dtos;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.Connection;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;

namespace PlanShare.Application.UseCases.User.Connection.ApproveCode;

public class ApproveCodeUserConnectionUseCase(ILoggedUser loggedUser, 
    IUserConnectionWriteOnlyRepository repository,
    IUserReadOnlyRepository userRepository,
    IUserConnectionReadOnlyRepository userConnectionRepository,
    IUnitOfWork unitOfWork) : IApproveCodeUserConnectionUseCase
{
    public async Task<HubOperationResult<string>> Execute(ConnectionByCodeDto connectionByCode)
    {
        var codeOwner = await loggedUser.Get();
        if (codeOwner.Id != connectionByCode.Generator.Id)
            return HubOperationResult<string>.Failure(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE, UserConnectionErrorCode.NotAuthorized);

        var joinerUser = connectionByCode.Joiner is not null ? await userRepository.GetById(connectionByCode.Joiner.Id) : null;
        if (joinerUser is null)
            return HubOperationResult<string>.Failure(ResourceMessagesException.NO_USER_CONNECTED_WITH_CODE, UserConnectionErrorCode.UserNotFound);

        if (joinerUser.Id == codeOwner.Id)
            return HubOperationResult<string>.Failure(ResourceMessagesException.SAME_USER_CANNOT_CONNECT_THEMSELVE, UserConnectionErrorCode.ConnectionToSelf);

        var existingConnection = await userConnectionRepository.AreUsersConnected(joinerUser, codeOwner);
        if (existingConnection)
        {
            var message = string.Format(ResourceMessagesException.YOU_ARE_ALREADY_CONNECTED_WITH, joinerUser.Name);
            return HubOperationResult<string>.Failure(message, UserConnectionErrorCode.ConnectionAlreadyExists);
        }

        var connection = new Domain.Entities.UserConnection
        {
            UserId = connectionByCode.Generator.Id,
            ConnectedUserId = connectionByCode.Joiner!.Id
        };

        await repository.Add(connection);

        await unitOfWork.Commit();

        return HubOperationResult<string>.Success(string.Empty);
    }
}
