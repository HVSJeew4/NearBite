namespace NearBite.Api.Dtos;

public class ListingFilterDto
{
    public string? Cuisine { get; set; }
    public int? MaxPrice { get; set; }
    public bool? IsVeg { get; set; }
    public string? Search { get; set; }
    public double? UserLat { get; set; }
    public double? UserLng { get; set; }
    public string? SortBy { get; set; }
}