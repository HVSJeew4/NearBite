namespace NearBite.Api.Domain;

public class Review
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsReported { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property back to the listing
    public Listing? Listing { get; set; }
}