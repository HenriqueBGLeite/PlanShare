using CommonTestUtilities.Authentication;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptography;
using PlanShare.Application.UseCases.User.Register;
using PlanShare.Domain.Extensions;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.Register;

public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestRegisterUserBuilder.Build();
        var useCase = CreateUseCase();

        // Action
        var result = await useCase.Execute(request);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Id.ShouldNotBe(Guid.Empty);
        result.Tokens.ShouldNotBeNull();
        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_EmailAlreadyRegistered()
    {
        // Arrange
        var request = RequestRegisterUserBuilder.Build();
        var useCase = CreateUseCase(request.Email);

        // Action
        var act = async () => await useCase.Execute(request);
        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        // Assert
        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
            });
        });
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        // Arrange
        var request = RequestRegisterUserBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        // Action
        var act = async () => await useCase.Execute(request);
        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        // Assert
        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(ResourceMessagesException.NAME_EMPTY);
            });
        });
    }

    private RegisterUserUseCase CreateUseCase(string? emailAlreadyExists = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripter = new PasswordEncripterBuilder().Build();
        var tokenService = TokenServiceBuilder.Build();

        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();

        if (emailAlreadyExists.NotEmpty())
            userReadOnlyRepository.ExistActiveUserWithEmail(emailAlreadyExists);

        return new RegisterUserUseCase(unitOfWork, userWriteOnlyRepository, userReadOnlyRepository.Build(), passwordEncripter, tokenService);
    }
}
