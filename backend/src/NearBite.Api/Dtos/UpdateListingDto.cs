using System.ComponentModel.DataAnnotations;

namespace NearBite.Api.Dtos;

public class UpdateListingDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Cuisine { get; set; } = string.Empty;

    [Range(1, 4)]
    public int PriceRange { get; set; }

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    public bool IsVeg { get; set; }

    [Required]
    [RegularExpression("^(Open|Closed|Busy)$",
        ErrorMessage = "LiveStatus must be Open, Closed, or Busy.")]
    public string LiveStatus { get; set; } = "Closed";
}