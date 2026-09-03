using NearBite.Api.Domain.Enums;

namespace NearBite.Api.Domain;

public class Listing
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cuisine { get; set; } = string.Empty;
    public int PriceRange { get; set; }
    public string City { get; set; } = string.Empty;
    public string LiveStatus { get; set; } = "Closed";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsVeg { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // NEW in Sprint 5
    public SubmissionStatus SubmissionStatus { get; set; } = SubmissionStatus.Pending;
    public string? RejectionReason { get; set; }
    public int? OwnerId { get; set; }

    // Navigation properties
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}