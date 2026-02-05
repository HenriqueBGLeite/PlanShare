using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.Authentication.Refresh;
using PlanShare.Communication.Responses;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PlanShare.App.Data.Network;

public partial class PlanShareHandler(ITokensStorage tokensStorage, 
    IUseRefreshTokenUseCase useRefreshTokenUseCase,
    INavigationService navigationService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ChangeRequestCulture(request);

        var tokens = await tokensStorage.Get();

        if (string.IsNullOrWhiteSpace(tokens.AccessToken) == false)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await response.Content.LoadIntoBufferAsync(cancellationToken);

            var error = await response.Content.ReadFromJsonAsync<ResponseErrorJson>(cancellationToken);
            if (error!.TokenIsExpired)
            {
                var result = await useRefreshTokenUseCase.Execute();
                if (result.IsSuccess)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.Response!.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
                else
                    await navigationService.GoToOnBoardingPage();
            }
        }

        return response;
    }

    private static void ChangeRequestCulture(HttpRequestMessage request)
    {
        var culture = CultureInfo.CurrentCulture.Name;

        request.Headers.AcceptLanguage.Clear();
        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}
