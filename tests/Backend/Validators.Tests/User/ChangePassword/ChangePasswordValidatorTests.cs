using CommonTestUtilities.Requests;
using PlanShare.Application.UseCases.User.ChangePassword;
using PlanShare.Exceptions;
using Shouldly;

namespace Validators.Tests.User.ChangePassword;

public class ChangePasswordValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordBuilder.Build();

        // Action
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_NewPassword_Empty()
    {
        // Arrange
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordBuilder.Build();
        request.NewPassword = string.Empty;

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
    public void Error_NewPassword_Invalid(int passwordLength)
    {
        // Arrange
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordBuilder.Build(passwordLength);

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
