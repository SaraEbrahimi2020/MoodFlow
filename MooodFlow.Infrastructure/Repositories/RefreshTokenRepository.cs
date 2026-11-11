using Moodflow.Domain.Entities;
using Moodflow.Domain.Repositories;
using MoodFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MooodFlow.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenAndUserIdAsync(string token, string userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token && x.UserId == userId, cancellationToken);
    }

    public async Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .Where(x => x.UserId == userId && !x.IsRevoked && !x.IsUsed)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}