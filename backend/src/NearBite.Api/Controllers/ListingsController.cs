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
    public async Task<ActionResult<IEnumerable<ListingDto>>> GetAll([FromQuery] ListingFilterDto filter)
    {
        try
        {
            var listings = await _service.GetAllAsync(filter);
            return Ok(listings);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ListingDto>> GetById(int id)
    {
        var listing = await _service.GetByIdAsync(id);

        if (listing == null)
        {
            return NotFound();
        }

        return Ok(listing);
    }

    [HttpPost]
    public async Task<ActionResult<ListingDto>> Create(CreateListingDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateListingDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (updated == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    // -----------------------------
    // Nested menu-item routes
    // -----------------------------

    [HttpGet("{listingId:int}/menu-items")]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItemsForListing(int listingId)
    {
        var items = await _service.GetMenuItemsForListingAsync(listingId);

        if (items == null)
        {
            return NotFound();
        }

        return Ok(items);
    }

    [HttpPost("{listingId:int}/menu-items")]
    public async Task<ActionResult<MenuItemDto>> CreateMenuItem(int listingId, CreateMenuItemDto dto)
    {
        var created = await _service.CreateMenuItemAsync(listingId, dto);

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