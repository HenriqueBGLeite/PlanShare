using FluentValidation.Results;
using Mapster;
using PlanShare.Application.Services.Authentication;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.RefreshToken;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Security.Cryptography;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.User.Register;
public class RegisterUserUseCase(IUnitOfWork unitOfWork,
        IUserWriteOnlyRepository repository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncripter passwordEncripter,
        ITokenService tokenService,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository) : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = request.Adapt<Domain.Entities.User>();
        user.Password = passwordEncripter.Encrypt(request.Password);

        var tokens = tokenService.GenerateTokens(user);

        await repository.Add(user);

        await refreshTokenRepository.Add(new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = tokens.Refresh,
            AccessTokenId = tokens.AccessTokenId
        });

        await unitOfWork.Commit();

        return new()
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

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailExist = await userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}