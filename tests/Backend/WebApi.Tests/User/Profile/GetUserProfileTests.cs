using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.Profile;

public class GetUserProfileTests : CustomClassFixture
{
    private const string BaseUrl = "/users";
    private readonly UserIdentityManager _user;

    public GetUserProfileTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.User;
    }

    [Fact]
    public async Task Success()
    {
        // Action
        var response = await DoGet(BaseUrl, token: _user.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();

        var document = await JsonDocument.ParseAsync(responseBody);

        // Assert
        document.RootElement.GetProperty("name").GetString().ShouldBe(_user.GetName());
        document.RootElement.GetProperty("email").GetString().ShouldBe(_user.GetEmail());
    }
}
