using Microsoft.AspNetCore.SignalR;
using PlanShare.Communication.Enums;
using PlanShare.Communication.Responses;
using PlanShare.Exceptions;

namespace PlanShare.Api.Filters;

public class UserConnectionsExceptionHubFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch
        {
            return HubOperationResult<string>.Failure(ResourceMessagesException.UNKNOWN_ERROR, UserConnectionErrorCode.Unknown);
        }
    }
}
