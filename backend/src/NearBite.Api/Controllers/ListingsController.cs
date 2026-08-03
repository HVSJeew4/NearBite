using Microsoft.AspNetCore.Mvc;
using NearBite.Api.Dtos;
using NearBite.Api.Services;

namespace NearBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IListingService _service;

    public ListingsController(IListingService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ListingDto>> GetAll()
    {
        var listings = _service.GetAll();
        return Ok(listings);
    }

    [HttpGet("{id:int}")]
    public ActionResult<ListingDto> GetById(int id)
    {
        var listing = _service.GetById(id);

        if (listing == null)
        {
            return NotFound();
        }

        return Ok(listing);
    }
}