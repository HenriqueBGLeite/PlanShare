using Microsoft.AspNetCore.SignalR.Client;
using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;
using System.Globalization;

namespace PlanShare.App.Data.Network.Hubs;

public class UserConnectionByCodeClient(string urlBase, ITokensStorage tokensStorage) : IUserConnectionByCodeClient
{
    public HubConnection CreateClient()
    {
        return new HubConnectionBuilder()
            .WithUrl($"{urlBase}/connection", options =>
            {
                options.Headers.Add("Accept-Language", CultureInfo.CurrentCulture.Name);

                options.AccessTokenProvider = async () =>
                {
                    var tokens = await tokensStorage.Get();
                    return tokens.AccessToken;
                };
            })
            .Build();
    }
}
