using CommonTestUtilities.Requests;
using PlanShare.Application.UseCases.User.Update;
using PlanShare.Exceptions;
using Shouldly;

namespace Validators.Tests.User.Update;

public class UpdateUserValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserBuilder.Build();

        // Action
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserBuilder.Build();
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
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserBuilder.Build();
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
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserBuilder.Build();
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
}
