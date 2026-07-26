using Microsoft.EntityFrameworkCore;
using Moodflow.Domain.Entities;
using MoodFlow.Domain.Repositories;
using MoodFlow.Infrastructure.Persistence;

namespace MoodFlow.Infrastructure.Repositories;

public class MoodLogRepository : IMoodLogRepository
{
    private readonly AppDbContext _context;

    public MoodLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MoodLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.MoodLogs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MoodLog>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MoodLogs.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(MoodLog moodLog, CancellationToken cancellationToken = default)
    {
        await _context.MoodLogs.AddAsync(moodLog, cancellationToken);
    }

    public Task UpdateAsync(MoodLog moodLog, CancellationToken cancellationToken)
    {
        _context.MoodLogs.Update(moodLog);
        return Task.CompletedTask; // چون متد دیتابیسیِ async برای خودِ update وجود ندارد، این خط تپش قلب متد را به صورت async شبیه‌سازی می‌کند
    }
    public Task DeleteAsync(MoodLog moodLog, CancellationToken cancellationToken)
    {
        _context.MoodLogs.Remove(moodLog);
        return Task.CompletedTask;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
    public async Task<(List<MoodLog> Items, int TotalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    DateTime? fromDate,
    DateTime? toDate,
    CancellationToken cancellationToken)
    {
        // ۱. ساخت کوئری پایه (هنوز روی دیتابیس اجرا نشده)
        var query = _context.MoodLogs.AsNoTracking().AsQueryable();

        // ۲. اعمال فیلترهای تاریخ در صورت وجود
        if (fromDate.HasValue)
        {
            query = query.Where(x => x.Date >= fromDate.Value.Date);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.Date <= toDate.Value.Date);
        }

        // ۳. محاسبه تعداد کل رکوردهای واجد شرایط
        var totalCount = await query.CountAsync(cancellationToken);

        // ۴. اعمال مرتب‌سازی (جدیدترین‌ها اول) و صفحه‌بندی با Skip و Take
        var items = await query
            .OrderByDescending(x => x.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}