using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using PlanShare.App.Extensions;
using PlanShare.App.Models.ValueObjects;
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

    public async Task<Result> Execute(Models.Login model)
    {
        var request = new RequestLoginJson
        {
            Email = model.Email,
            Password = model.Password,
        };

        var response = await _loginApi.Login(request);

        if (response.IsSuccessful)
        {
            var user = new Models.ValueObjects.User(response.Content.Id, response.Content.Name);
            var tokens = new Tokens(response.Content.Tokens.AccessToken, response.Content.Tokens.RefreshToken);

            _userStorage.Save(user);
            await _tokenStorage.Save(tokens);

            return Result.Success();
        }

        var errorResponse = await response.Error.GetResponseError();

        return Result.Failure(errorResponse.Errors);
    }
}
