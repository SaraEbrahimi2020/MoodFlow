namespace Moodflow.Domain.Entities;

public class MoodType
{
    public int Id { get; set; }
    /// <summary>
    /// حالت روحی مثل عالی ،مضطرب
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// گذاشتن ایموجی
    /// </summary>
    public string Icon { get; set; }=string.Empty;
}
