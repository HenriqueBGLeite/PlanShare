using PlanShare.Domain.Dtos;
using PlanShare.Domain.Entities;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Security.Tokens;

namespace PlanShare.Application.Services.Authentication;
public class TokenService(IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator) : ITokenService
{
    public TokensDto GenerateTokens(User user)
    {
        (var accessToken, var accessTokenIdentifier) = accessTokenGenerator.Generate(user);
        var refreshToken = refreshTokenGenerator.Generate();

        return new TokensDto
        {
            Access = accessToken,
            Refresh = refreshToken,
            AccessTokenId = accessTokenIdentifier
        };
    }
}
