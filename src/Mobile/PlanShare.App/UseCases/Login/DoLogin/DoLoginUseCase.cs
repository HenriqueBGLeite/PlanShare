using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using PlanShare.App.Extensions;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.Login.DoLogin;

public class DoLoginUseCase(ILoginApi loginApi, IUserStorage userStorage, ITokensStorage tokensStorage) : IDoLoginUseCase
{
    public async Task Execute(Models.Login model)
    {
        var request = new RequestLoginJson
        {
            Email = model.Email,
            Password = model.Password,
        };

        var response = await loginApi.Login(request);

        if (response.IsSuccessful)
        {
            var user = new Models.ValueObjects.User(response.Content.Id, response.Content.Name);
            var tokens = new Models.ValueObjects.Tokens(response.Content.Tokens.AccessToken, response.Content.Tokens.RefreshToken);

            userStorage.Save(user);
            await tokensStorage.Save(tokens);
        }
        else
        {
            var errorResponse = await response.Error.GetResponseError();
        }
    }
}
