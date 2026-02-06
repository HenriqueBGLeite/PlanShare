using Microsoft.AspNetCore.SignalR;
using PlanShare.Api.Hubs.Services;
using PlanShare.Communication.Enums;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Extensions;
using PlanShare.Exceptions;

namespace PlanShare.Api.Filters;

public class UserConnectionsExceptionHubFilter(CodeConnectionService codeConnectionService) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch
        {
            var connectionId = invocationContext.Hub.Context.ConnectionId;

            var code = codeConnectionService.GetCodeByConnectionId(connectionId);
            if (code.NotEmpty())
            {
                var connection = codeConnectionService.RemoveConnection(code);
                if (connection is not null && connection.ConnectingUserConnectionId.NotEmpty())
                {
                    await invocationContext.Hub.Clients.Client(connection.ConnectingUserConnectionId).SendAsync("ConnectionErrorOccurred");
                }
            }

            return HubOperationResult<string>.Failure(ResourceMessagesException.UNKNOWN_ERROR, UserConnectionErrorCode.Unknown);
        }
    }
}
