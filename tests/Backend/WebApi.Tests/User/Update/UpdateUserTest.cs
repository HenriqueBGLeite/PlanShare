using CommonTestUtilities.Requests;
using PlanShare.Domain.Extensions;
using PlanShare.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.Update;

public class UpdateUserTest : CustomClassFixture
{
    private const string BaseUrl = "/users";
    private readonly UserIdentityManager _user;

    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.User;
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestUpdateUserBuilder.Build();

        // Action
        var response = await DoPut(BaseUrl, request, _user.GetAccessToken());

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string culture)
    {
        // Arrange
        var request = RequestUpdateUserBuilder.Build();
        request.Name = string.Empty;

        // Action
        var response = await DoPut(BaseUrl, request, _user.GetAccessToken(), culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var responseBody = await response.Content.ReadAsStreamAsync();

        var document = await JsonDocument.ParseAsync(responseBody);

        // Assert
        var errors = document.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().NotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}
