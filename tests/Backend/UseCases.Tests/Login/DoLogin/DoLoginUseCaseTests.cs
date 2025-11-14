using CommonTestUtilities.Authentication;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptography;
using PlanShare.Application.UseCases.Login.DoLogin;
using PlanShare.Communication.Requests;
using PlanShare.Domain.Extensions;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Login.DoLogin;

public class DoLoginUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        // Action
        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        // Assert
        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Id.ShouldBe(user.Id);
        result.Name.ShouldBe(user.Name);
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        // Arrange
        var request = RequestLoginBuilder.Build();

        var useCase = CreateUseCase();

        // Action
        var act = async () => await useCase.Execute(request);
        var exception = await act.ShouldThrowAsync<InvalidLoginException>();

        // Assert
        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.Unauthorized);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count().ShouldBe(1);
                errors.ShouldContain(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
            });
        });
    }

    [Fact]
    public async Task Error_Invalid_Password()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();
        var request = RequestLoginBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user);

        // Action
        var act = async () => await useCase.Execute(request);
        var exception = await act.ShouldThrowAsync<InvalidLoginException>();

        // Assert
        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.Unauthorized);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count().ShouldBe(1);
                errors.ShouldContain(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
            });
        });
    }

    private DoLoginUseCase CreateUseCase(PlanShare.Domain.Entities.User? user = null)
    {
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var tokenService = TokenServiceBuilder.Build();

        if (user is not null)
            userReadOnlyRepository.GetUserByEmail(user);

        return new DoLoginUseCase(userReadOnlyRepository.Build(), passwordEncripter, tokenService);
    }
}
