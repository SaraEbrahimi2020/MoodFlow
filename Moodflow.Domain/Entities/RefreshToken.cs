namespace Moodflow.Domain.Entities;

public class RefreshToken
{
    public long Id { get; set; }
    public string UserId { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // متد بیزینسی برای چک کردن اعتبار توکن
    public bool IsActive => !IsRevoked && !IsUsed && ExpiresAt > DateTime.UtcNow;

    // متد برای باطل کردن توکن
    public void Revoke()
    {
        IsRevoked = true;
    }

    // متد برای علامت‌گذاری استفاده شدن
    public void MarkAsUsed()
    {
        IsUsed = true;
    }
}