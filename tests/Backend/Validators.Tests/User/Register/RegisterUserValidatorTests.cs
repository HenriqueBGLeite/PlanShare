using CommonTestUtilities.Requests;
using PlanShare.Application.UseCases.User.Register;
using PlanShare.Exceptions;
using Shouldly;

namespace Validators.Tests.User.Register;

public class RegisterUserValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();

        // Action
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();
        request.Name = string.Empty;

        // Action
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        });
    }

    [Fact]
    public void Error_Email_Empty()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();
        request.Email = string.Empty;

        // Action
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
        });
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();
        request.Email = "test.com";

        // Action
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
        });
    }

    [Fact]
    public void Error_Password_Empty()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();
        request.Password = string.Empty;

        // Action
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_EMPTY));
        });
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int amountPasswordCharacters)
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build(amountPasswordCharacters);

        // Action
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
        });
    }
}
