using System.Net;
using ContosoPizza.Entities;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzasController : ControllerBase
{
    private readonly IPizzaService _service;

    public PizzasController(IPizzaService service)
    {
        _service = service;
    }

    // GET all action
    [HttpGet]
    public IEnumerable<Pizza> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Pizza> GetById(int id)
    {
        var pizza = _service.GetById(id);
        return pizza is null ? NotFound() : pizza;
    }

    [HttpPost]
    public IActionResult Create(Pizza newPizza)
    {
        var pizza = _service.Create(newPizza);
        return CreatedAtAction(nameof(GetById), new { id = pizza!.Id }, pizza);
    }

    [HttpPost("{id}/image")]
    public async Task<IActionResult> UploadImage(int id, IFormFile file)
    {
        var response = await _service.UploadImageAsync(id, file);
        return response.HttpStatusCode == HttpStatusCode.OK ? Ok() : BadRequest();
    }

    [HttpGet("{id}/image")]
    public async Task<IActionResult> GetImage(int id)
    {
        var response = await _service.GetImageAsync(id);
        if (response is null)
            return NotFound();
        
        return File(response.ResponseStream, response.Headers.ContentType);
    }

    [HttpDelete("{id}/image")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var response = await _service.DeleteImageAsync(id);
        return response.HttpStatusCode switch
        {
            HttpStatusCode.NoContent => Ok(),
            HttpStatusCode.NotFound => NotFound(),
            _ => BadRequest()
        };
    }

    [HttpPut("{id}/addtopping")]
    public IActionResult AddTopping(int id, int toppingId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if (pizzaToUpdate is null)
            return NotFound();

        _service.AddTopping(id, toppingId);
        return NoContent();
    }

    [HttpPut("{id}/updatesauce")]
    public IActionResult UpdateSauce(int id, int sauceId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if (pizzaToUpdate is null)
            return NotFound();

        _service.UpdateSauce(id, sauceId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = _service.GetById(id);

        if (pizza is null)
            return NotFound();

        _service.DeleteById(id);
        return Ok();
    }
}