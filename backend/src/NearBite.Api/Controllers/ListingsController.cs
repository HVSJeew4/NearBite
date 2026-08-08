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

    [HttpPost]
    public ActionResult<ListingDto> Create(CreateListingDto dto)
    {
        var created = _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, UpdateListingDto dto)
    {
        var updated = _service.Update(id, dto);

        if (updated == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _service.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{listingId:int}/menu-items")]
    public ActionResult<IEnumerable<MenuItemDto>> GetMenuItemsForListing(int listingId)
    {
        var items = _service.GetMenuItemsForListing(listingId);

        if (items == null)
        {
            return NotFound();
        }

        return Ok(items);
    }

    [HttpPost("{listingId:int}/menu-items")]
    public ActionResult<MenuItemDto> CreateMenuItem(int listingId, CreateMenuItemDto dto)
    {
        var created = _service.CreateMenuItem(listingId, dto);

        if (created == null)
        {
            return NotFound();
        }

        return CreatedAtAction(
            nameof(MenuItemsController.GetById),
            "MenuItems",
            new { id = created.Id },
            created);
    }
}