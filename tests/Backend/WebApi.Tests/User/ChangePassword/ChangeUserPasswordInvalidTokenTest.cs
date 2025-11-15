using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.ChangePassword;

public class ChangeUserPasswordInvalidTokenTest : CustomClassFixture
{
    private const string BaseUrl = "/users/change-password";
    private readonly UserIdentityManager _user;

    public ChangeUserPasswordInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.User;
    }

    [Fact]
    public async Task Erro_Invalid_Token()
    {
        // Arrange
        var request = RequestChangePasswordBuilder.Build();
        request.Password = _user.GetPassword();

        // Action
        var response = await DoPut(BaseUrl, request, token: "invalidToken");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Erro_Empty_Token()
    {
        // Arrange
        var request = RequestChangePasswordBuilder.Build();
        request.Password = _user.GetPassword();

        // Action
        var response = await DoPut(BaseUrl, request, token: string.Empty);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
