using System.ComponentModel.DataAnnotations;

namespace NearBite.Api.Dtos;

public class UpdateMenuItemDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    public bool IsVeg { get; set; }

    [StringLength(500)]
    [Url]
    public string? PhotoUrl { get; set; }
}