using PlanShare.App.Data.Network.Api;
using PlanShare.Communication.Requests;

namespace PlanShare.App.UseCases.Login.DoLogin;

public class DoLoginUseCase(ILoginApi loginApi) : IDoLoginUseCase
{
    public async Task Execute(Models.Login login)
    {
        var request = new RequestLoginJson
        {
            Email = login.Email,
            Password = login.Password,
        };

        var result = await loginApi.Login(request);
    }
}
