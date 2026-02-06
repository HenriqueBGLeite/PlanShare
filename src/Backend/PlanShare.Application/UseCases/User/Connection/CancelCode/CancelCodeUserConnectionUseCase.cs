using PlanShare.Communication.Enums;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Dtos;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;

namespace PlanShare.Application.UseCases.User.Connection.CancelCode;

public class CancelCodeUserConnectionUseCase(ILoggedUser loggedUser) : ICancelCodeUserConnectionUseCase
{
    public async Task<HubOperationResult<string>> Execute(UserConnectionsDto userConnections)
    {
        var userLogged = await loggedUser.Get();
        if (userLogged.Id != userConnections.UserId)
            return HubOperationResult<string>.Failure(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE, UserConnectionErrorCode.NotAuthorized);

        return HubOperationResult<string>.Success(string.Empty);
    }
}
