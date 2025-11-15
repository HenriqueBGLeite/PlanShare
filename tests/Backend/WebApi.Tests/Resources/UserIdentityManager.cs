using PlanShare.Domain.Dtos;

namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly PlanShare.Domain.Entities.User _user;
    private readonly string _password;
    private readonly TokensDto _tokens;


    public UserIdentityManager(PlanShare.Domain.Entities.User user, string password, TokensDto tokens)
    {
        _user = user;
        _password = password;
        _tokens = tokens;
    }

    public string GetPassword() => _password;
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetAccessToken() => _tokens.Access;
}
