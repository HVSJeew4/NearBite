using Microsoft.AspNetCore.Mvc;
using NearBite.Api.Dtos;
using NearBite.Api.Services;

namespace NearBite.Api.Controllers;

[ApiController]
[Route("api/menu-items")]
public class MenuItemsController : ControllerBase
{
    private readonly IListingService _service;

    public MenuItemsController(IListingService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MenuItemDto>> GetById(int id)
    {
        var item = await _service.GetMenuItemByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateMenuItemDto dto)
    {
        var updated = await _service.UpdateMenuItemAsync(id, dto);

        if (updated == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteMenuItemAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}