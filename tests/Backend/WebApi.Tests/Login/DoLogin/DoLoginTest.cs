using CommonTestUtilities.Requests;
using PlanShare.Communication.Requests;
using PlanShare.Domain.Extensions;
using PlanShare.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Login.DoLogin;

public class DoLoginTest : CustomClassFixture
{
    private const string BaseUrl = "login";
    private readonly UserIdentityManager _user;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.User;
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = new RequestLoginJson
        {
            Email = _user.GetEmail(),
            Password = _user.GetPassword()
        };

        // Action
        var response = await DoPost(BaseUrl, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();

        var document = await JsonDocument.ParseAsync(responseBody);

        // Assert
        document.RootElement.GetProperty("id").GetGuid().ShouldNotBe(Guid.Empty);
        document.RootElement.GetProperty("name").GetString().ShouldBe(_user.GetName());
        document.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        // Arrange
        var request = RequestLoginBuilder.Build();

        // Action
        var response = await DoPost(BaseUrl, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        using var responseBody = await response.Content.ReadAsStreamAsync();

        var document = await JsonDocument.ParseAsync(responseBody);

        // Assert
        var errors = document.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().NotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}
