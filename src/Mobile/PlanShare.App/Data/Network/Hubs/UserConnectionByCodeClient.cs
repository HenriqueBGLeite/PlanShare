using Microsoft.AspNetCore.SignalR.Client;
using PlanShare.App.Data.Network.Api;
using PlanShare.App.Data.Storage.SecureStorage.Tokens;

namespace PlanShare.App.Data.Network.Hubs;

public class UserConnectionByCodeClient(string urlBase, ITokensStorage tokensStorage) : IUserConnectionByCodeClient
{
    public HubConnection CreateClient()
    {
        return new HubConnectionBuilder()
            .WithUrl($"{urlBase}/connection", options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    var tokens = await tokensStorage.Get();
                    return tokens.AccessToken;
                };
            })
            .Build();
    }
}
