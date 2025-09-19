using PlanShare.App.Models;

namespace PlanShare.App.UseCases.User.Register;

public interface IRegisterUserUserCase
{
    Task Execute(UserRegisterAccount model);
}
