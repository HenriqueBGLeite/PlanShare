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

namespace WebApi.Tests.User.ChangePassword;

public class ChangeUserPasswordSuccessTest : CustomClassFixture
{
    private const string BaseUrl = "/users/change-password";
    private readonly UserIdentityManager _user;

    public ChangeUserPasswordSuccessTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.User;
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestChangePasswordBuilder.Build();
        request.Password = _user.GetPassword();

        // Action
        var response = await DoPut(BaseUrl, request, _user.GetAccessToken());

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
