using Shouldly;
using System.Net;

namespace WebApi.Tests.User.Profile;

public class GetUserProfileInvalidTokenTest : CustomClassFixture
{
    private const string BaseUrl = "/users";

    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Erro_Invalid_Token()
    {
        // Action
        var response = await DoGet(BaseUrl, token: "invalidToken");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Erro_Empty_Token()
    {
        // Action
        var response = await DoGet(BaseUrl, token: string.Empty);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
