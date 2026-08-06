namespace NearBite.Api.Dtos;

public class MenuItemDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsVeg { get; set; }
    public string? PhotoUrl { get; set; }
}