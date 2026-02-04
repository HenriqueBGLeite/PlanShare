using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using PlanShare.App.Models;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.User.Register;

public class RegisterUserUseCase(IUserApi userApi, IUserStorage userStorage, ITokensStorage tokensStorage) : IRegisterUserUseCase
{
    public async Task Execute(UserRegisterAccount model)
    {
        var request = new RequestRegisterUserJson
        {
            Email = model.Email,
            Name = model.Name,
            Password = model.Password,
        };

        var response = await userApi.Register(request);

        var user = new Models.ValueObjects.User(response.Id, response.Name);
        var tokens = new Models.ValueObjects.Tokens(response.Tokens.AccessToken, response.Tokens.RefreshToken);

        userStorage.Save(user);
        await tokensStorage.Save(tokens);
    }
}
