using Microsoft.EntityFrameworkCore;
using PlanShare.Domain.Entities;
using PlanShare.Domain.Repositories.RefreshToken;

namespace PlanShare.Infrastructure.DataAccess.Repositories;

internal class RefreshTokenRepository(PlanShareDbContext context) : IRefreshTokenReadOnlyRepository, IRefreshTokenWriteOnlyRepository
{
    public async Task Add(RefreshToken refreshToken)
    {
        await context.RefreshTokens
            .Where(token => token.UserId == refreshToken.UserId)
            .ExecuteDeleteAsync();

        await context.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> Get(string token) => await context
        .RefreshTokens
        .AsNoTracking()
        .Include(refreshToken => refreshToken.User)
        .FirstOrDefaultAsync(refreshToken => refreshToken.Token.Equals(token));

    public async Task<bool> HasRefreshTokenAssociated(User user, Guid accessTokenId) => await context.RefreshTokens
        .AnyAsync(refreshToken => refreshToken.UserId == user.Id && refreshToken.AccessTokenId == accessTokenId);
}
