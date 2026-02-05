using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Extensions;
using PlanShare.App.Models.ValueObjects;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.User.Update;

public class UpdateUserUseCase(IUserApi userApi, IUserStorage userStorage) : IUpdateUserUseCase
{
    public async Task<Result> Execute(Models.User model)
    {
        var request = new RequestUpdateUserJson
        {
            Email = model.Email,
            Name = model.Name,
        };

        var response = await userApi.UpdateProfile(request);

        if (response.IsSuccessful)
        {
            var user = userStorage.Get() with { Name = model.Name };

            userStorage.Save(user);

            return Result.Success();
        }

        var errorResponse = await response.Error.GetResponseError();

        return Result.Failure(errorResponse.Errors);
    }
}
