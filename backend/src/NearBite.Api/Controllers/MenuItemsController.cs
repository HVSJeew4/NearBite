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
    public ActionResult<MenuItemDto> GetById(int id)
    {
        var item = _service.GetMenuItemById(id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, UpdateMenuItemDto dto)
    {
        var updated = _service.UpdateMenuItem(id, dto);

        if (updated == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _service.DeleteMenuItem(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}