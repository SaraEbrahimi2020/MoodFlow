using Moodflow.Domain.Entities;

namespace MoodFlow.Domain.Repositories;

public interface IMoodLogRepository
{
    Task<MoodLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MoodLog>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(MoodLog moodLog, CancellationToken cancellationToken = default);
    Task UpdateAsync(MoodLog moodLog, CancellationToken cancellationToken);
    Task DeleteAsync(MoodLog moodLog, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<(List<MoodLog> Items, int TotalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    DateTime? fromDate,
    DateTime? toDate,
    CancellationToken cancellationToken);
}