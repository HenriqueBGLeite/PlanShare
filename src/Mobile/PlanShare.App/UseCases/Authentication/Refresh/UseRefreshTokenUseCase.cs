using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using PlanShare.App.Models.ValueObjects;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.Authentication.Refresh;

public class UseRefreshTokenUseCase(IAuthenticationApi authenticationApi,
    ITokensStorage tokensStorage,
    IUserStorage userStorage) : IUseRefreshTokenUseCase
{
    public async Task<Result<Tokens>> Execute()
    {
        var tokens = await tokensStorage.Get();

        var request = new RequestNewTokenJson
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };

        var response = await authenticationApi.Refresh(request);
        if (response.IsSuccessful)
        {
            tokens = new Tokens(response.Content.AccessToken, response.Content.RefreshToken);

            await tokensStorage.Save(tokens);

            return Result<Tokens>.Success(tokens);
        }

        userStorage.Clear();
        tokensStorage.Clear();

        return Result<Tokens>.Failure([]);
    }
}
