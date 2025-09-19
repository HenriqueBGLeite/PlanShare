using PlanShare.App.Data.Network.Api;
using PlanShare.App.Models;
using PlanShare.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanShare.App.UseCases.User.Register;

public class RegisterUserUserCase : IRegisterUserUserCase
{
    private readonly IUserApi _userApi;

    public RegisterUserUserCase(IUserApi userApi)
    {
        _userApi = userApi;
    }

    public async Task Execute(UserRegisterAccount user)
    {
        var request = new RequestRegisterUserJson
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
        };

        var responser = await _userApi.Register(request);
    }
}
