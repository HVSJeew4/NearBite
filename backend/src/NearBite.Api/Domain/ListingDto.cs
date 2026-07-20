namespace NearBite.Api.Dtos;

public class ListingDto
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
}