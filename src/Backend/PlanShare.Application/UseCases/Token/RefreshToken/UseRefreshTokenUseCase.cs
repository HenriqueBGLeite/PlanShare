using Microsoft.Extensions.Options;
using PlanShare.Application.Services.Authentication;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.RefreshToken;
using PlanShare.Domain.Security.Tokens;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.Token.RefreshToken;

public class UseRefreshTokenUseCase(ITokenService tokenService, 
    IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
    IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
    IAccessTokenValidator accessTokenValidator,
    IUnitOfWork unitOfWork,
    IOptions<TokenSettings> tokenSettings) : IUseRefreshTokenUseCase
{
    public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
    {
        var refreshToken = await refreshTokenReadOnlyRepository.Get(request.RefreshToken);
        if (refreshToken is null)
            throw new RefreshTokenNotFoundException();

        var accessTokenId = accessTokenValidator.GetAccessTokenIdentifier(request.AccessToken);
        if (refreshToken.AccessTokenId != accessTokenId)
            throw new RefreshTokenNotFoundException();

        var expireAt = refreshToken.CreatedAt.AddDays(tokenSettings.Value.RefreshTokenValidityDays);
        if (DateTime.UtcNow > expireAt)
            throw new RefreshTokenExpiredException();
            
        var tokens = tokenService.GenerateTokens(refreshToken.User);

        await refreshTokenWriteOnlyRepository.Add(new Domain.Entities.RefreshToken
        {
            UserId = refreshToken.UserId,
            Token = tokens.Refresh,
            AccessTokenId = tokens.AccessTokenId
        });

        await unitOfWork.Commit();

        return new ResponseTokensJson
        {
            RefreshToken = tokens.Refresh,
            AccessToken = tokens.Access
        };
    }
}
