using Moq;
using PlanShare.App.Models;
using PlanShare.App.Models.ValueObjects;
using PlanShare.App.UseCases.User.Register;

namespace CommonTestUtilities.UseCases.User.Register;

public class RegisterUserUserCaseBuilder
{
    public static IRegisterUserUserCase Build(Result result)
    {
        var mock = new Mock<IRegisterUserUserCase>();

        mock.Setup(useCase => useCase.Execute(It.IsAny<UserRegisterAccount>()))
            .ReturnsAsync(result);

        return mock.Object;
    }
}
