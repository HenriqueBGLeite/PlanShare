using PlanShare.App.Data.Network.Api;
using PlanShare.App.Models;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.User.Register;

public class RegisterUserUseCase(IUserApi userApi) : IRegisterUserUseCase
{
    public async Task Execute(UserRegisterAccount user)
    {
        var request = new RequestRegisterUserJson
        {
            Email = user.Email,
            Name = user.Name,
            Password = user.Password,
        };

        var response = await userApi.Register(request);
    }
}
