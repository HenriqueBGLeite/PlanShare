using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using PlanShare.Application.UseCases.User.Update;
using PlanShare.Domain.Extensions;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.Update;

public class UpdateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserBuilder.Build();

        var useCase = CreateUseCase(user);

        // Action
        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        // Assert
        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Error_EmailAlreadyRegistered()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

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
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

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

    private UpdateUserUseCase CreateUseCase(PlanShare.Domain.Entities.User user, string? emailAlreadyExists = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var userUpdateOnlyRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();

        if (emailAlreadyExists.NotEmpty())
            userReadOnlyRepository.ExistActiveUserWithEmail(emailAlreadyExists);

        return new UpdateUserUseCase(loggedUser, userUpdateOnlyRepository, userReadOnlyRepository.Build(), unitOfWork);
    }
}
