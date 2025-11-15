using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Tests.User.Register;

public class RegisterUserTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public RegisterUserTests(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestRegisterUserBuilder.Build();

        // Action
        var response = await _httpClient.PostAsJsonAsync("/users", request);

        using var responseBody = await response.Content.ReadAsStreamAsync();

        var document = await JsonDocument.ParseAsync(responseBody);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        document.RootElement.GetProperty("id").GetGuid().ShouldNotBe(Guid.Empty);
        document.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        document.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty();
    }
}
