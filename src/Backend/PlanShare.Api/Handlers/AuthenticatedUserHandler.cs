using Microsoft.AspNetCore.Authorization;
using PlanShare.Api.Handlers.Requirements;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories.RefreshToken;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Security.Tokens;

namespace PlanShare.Api.Handlers;

public class AuthenticatedUserHandler(IAccessTokenValidator accessTokenValidator, 
    IUserReadOnlyRepository userRepository,
    IRefreshTokenReadOnlyRepository refreshTokenRepository) : AuthorizationHandler<AuthenticatedUserRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthenticatedUserRequirement requirement)
    {
        try
        {
            var token = TokenOnConnection(context);
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Fail();
                return;
            }

            accessTokenValidator.Validate(token);

            var userIdentifier = accessTokenValidator.GetUserIdentifier(token);

            var user = await userRepository.GetById(userIdentifier);
            if (user is null)
            {
                context.Fail();
                return;
            }

            var accessTokenId = accessTokenValidator.GetAccessTokenIdentifier(token);
            var existRefreshTokenAssociated = await refreshTokenRepository.HasRefreshTokenAssociated(user, accessTokenId);
            if (existRefreshTokenAssociated.IsFalse())
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
        catch
        {
            context.Fail();
        }
    }

    private static string TokenOnConnection(AuthorizationHandlerContext context)
    {
        var defaultHttpContext = context.Resource as DefaultHttpContext;

        var authentication = defaultHttpContext?.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication))
            return string.Empty;

        return authentication["Bearer ".Length..].Trim();
    }
}
