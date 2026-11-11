using Moodflow.Domain.Entities;


namespace Moodflow.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByTokenAndUserIdAsync(string token, string userId, CancellationToken cancellationToken);
    Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
