using PlanShare.Application.Services.Authentication;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.RefreshToken;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Security.Cryptography;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase(IUserReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository) : IDoLoginUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await repository.GetUserByEmail(request.Email);

        if (user is null)
            throw new InvalidLoginException();

        var passwordMatch = passwordEncripter.IsValid(request.Password, user.Password);

        if (passwordMatch == false)
            throw new InvalidLoginException();

        var tokens = tokenService.GenerateTokens(user);

        await refreshTokenRepository.Add(new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = tokens.Refresh,
            AccessTokenId = tokens.AccessTokenId
        });

        await unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = tokens.Access,
                RefreshToken = tokens.Refresh
            }
        };
    }
}