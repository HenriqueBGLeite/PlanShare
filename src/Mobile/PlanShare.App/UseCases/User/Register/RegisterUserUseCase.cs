using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using PlanShare.App.Extensions;
using PlanShare.App.Models;
using PlanShare.App.Models.ValueObjects;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.User.Register;

public class RegisterUserUseCase(IUserApi userApi, IUserStorage userStorage, ITokensStorage tokensStorage) : IRegisterUserUseCase
{
    public async Task<Result> Execute(UserRegisterAccount model)
    {
        var request = new RequestRegisterUserJson
        {
            Email = model.Email,
            Name = model.Name,
            Password = model.Password,
        };

        var response = await userApi.Register(request);

        if (response.IsSuccessful)
        {
            var user = new Models.ValueObjects.User(response.Content.Id, response.Content.Name);
            var tokens = new Models.ValueObjects.Tokens(response.Content.Tokens.AccessToken, response.Content.Tokens.RefreshToken);

            userStorage.Save(user);
            await tokensStorage.Save(tokens);

            return Result.Success();
        }
        
        var errorResponse = await response.Error.GetResponseError();

        return Result.Failure(errorResponse.Errors);
    }
}
