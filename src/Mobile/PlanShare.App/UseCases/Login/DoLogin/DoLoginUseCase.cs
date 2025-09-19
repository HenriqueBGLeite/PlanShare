using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly ILoginApi _loginApi;
    private readonly IUserStorage _userStorage;
    private readonly ITokensStorage _tokenStorage;

    public DoLoginUseCase(ILoginApi loginApi, 
        IUserStorage userStorage, 
        ITokensStorage tokenStorage)
    {
        _loginApi = loginApi;
        _userStorage = userStorage;
        _tokenStorage = tokenStorage;
    }

    public async Task Execute(Models.Login model)
    {
        var request = new RequestLoginJson
        {
            Email = model.Email,
            Password = model.Password,
        };

        var response = await _loginApi.Login(request);

        var user = new Models.ValueObjects.User(response.Id, response.Name);
        var tokens = new Models.ValueObjects.Tokens(response.Tokens.AccessToken, response.Tokens.RefreshToken);

        _userStorage.Save(user);
        await _tokenStorage.Save(tokens);
    }
}
