using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptography;
using CommonTestUtilities.Services.LoggedUser;
using PlanShare.Application.UseCases.User.ChangePassword;
using PlanShare.Communication.Requests;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;
using Shouldly;
using System.Threading.Tasks;

namespace UseCases.Tests.User.ChangePassword;

public class ChangePasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        (var user, var password) = UserBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();
        request.Password = password;

        var currentPasswordHash = user.Password;

        var useCase = CreateUseCase(user);

        // Action
        var act = async () => await useCase.Execute(request);

        // Assert
        await act.ShouldNotThrowAsync();
        user.Password.ShouldNotBe(currentPasswordHash);
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        // Arrange
        (var user, var password) = UserBuilder.Build();
        var request = new RequestChangePasswordJson
        {
            Password = password,
            NewPassword = string.Empty
        };

        var useCase = CreateUseCase(user);

        // Action
        var act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        // Assert
        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(erros =>
            {
                erros.Count().ShouldBe(1);
                erros.ShouldContain(ResourceMessagesException.PASSWORD_EMPTY);
            });
        });
    }

    [Fact]
    public async Task Error_CurrentPasswor_Different()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();

        var useCase = CreateUseCase(user);

        // Action
        var act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        // Assert
        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(erros =>
            {
                erros.Count().ShouldBe(1);
                erros.ShouldContain(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);
            });
        });
    }

    private ChangePasswordUseCase CreateUseCase(PlanShare.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var userUpdateOnlyRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, passwordEncripter, userUpdateOnlyRepository, unitOfWork);
    }
}
