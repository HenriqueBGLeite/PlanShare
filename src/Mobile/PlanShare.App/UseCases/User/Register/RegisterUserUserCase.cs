using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using PlanShare.App.Extensions;
using PlanShare.App.Models;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.User.Register;

public class RegisterUserUserCase : IRegisterUserUserCase
{
    private readonly IUserApi _userApi;
    private readonly IUserStorage _userStorage;
    private readonly ITokensStorage _tokenStorage;

    public RegisterUserUserCase(IUserApi userApi, 
        IUserStorage userStorage, 
        ITokensStorage tokenStorage)
    {
        _userApi = userApi;
        _userStorage = userStorage;
        _tokenStorage = tokenStorage;
    }

    public async Task Execute(UserRegisterAccount model)
    {
        var request = new RequestRegisterUserJson
        {
            Name = model.Name,
            Email = model.Email,
            Password = model.Password,
        };

        var response = await _userApi.Register(request);

        if (response.IsSuccessful)
        {
            var user = new Models.ValueObjects.User(response.Content.Id, response.Content.Name);
            var tokens = new Models.ValueObjects.Tokens(response.Content.Tokens.AccessToken, response.Content.Tokens.RefreshToken);

            _userStorage.Save(user);
            await _tokenStorage.Save(tokens);
        }
        else
        {
            var errorResponse = await response.Error.GetResponseError();
        }
    }
}
