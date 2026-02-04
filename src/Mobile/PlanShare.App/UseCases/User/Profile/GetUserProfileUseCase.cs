using PlanShare.App.Data.Network.Api;
using PlanShare.App.Extensions;
using PlanShare.App.Models.ValueObjects;

namespace PlanShare.App.UseCases.User.Profile;

public class GetUserProfileUseCase(IUserApi userApi) : IGetUserProfileUseCase
{
    public async Task<Result<Models.User>> Execute()
    {
        var response = await userApi.GetProfile();

        if (response.IsSuccessful)
        {
            var model = new Models.User
            {
                Name = response.Content.Name,
                Email = response.Content.Email,
            };

            return Result<Models.User>.Success(model);
        }

        var errorResponse = await response.Error.GetResponseError();

        return Result<Models.User>.Failure(errorResponse.Errors);
    }
}
