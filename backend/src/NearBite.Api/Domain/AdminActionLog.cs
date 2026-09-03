namespace NearBite.Api.Domain;

public class AdminActionLog
{
    public int Id { get; set; }
    public int AdminId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;
    public int TargetId { get; set; }
    public string? Reason { get; set; }
    public DateTime Timestamp { get; set; }
}