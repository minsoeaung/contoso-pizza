using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ContosoContext _context;

    public TodoController(ContosoContext context)
    {
        _context = context;
    }

    // If say nothing like [FromQuery],
    //      default to query params.
    // Upon inserting IEnumerable<int> todoItem to argument list, 
    //      "Request with GET/HEAD method cannot have body." error occurred.

    [HttpGet("query-params")]
    public int GetQueryTest(bool isIt, string? name)
    {
        return 1;
    }

    // Cannot have more than one parameter for post request.
    [HttpPost("post-json")]
    [Consumes("application/json")]
    public IActionResult PostJson(IEnumerable<TodoItemDto> todoItem)
    {
        return Ok(new { Todo = todoItem });
    }

    [HttpPost("post-file")]
    public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
    {
        var size = files.Sum(f => f.Length);

        foreach (var formFile in files)
        {
            if (formFile.Length <= 0) continue;
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }
        }

        // Process uploaded files
        // Don't rely on or trust the FileName property without validation.

        return Ok(new { count = files.Count, size });
    }

    [HttpPost("post-form")]
    public IActionResult PostForm([FromForm] IEnumerable<int> numbers, [FromForm] IEnumerable<string> names)
    {
        return Ok(new { Numbers = numbers, Names = names });
    }

    // GET: api/Todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems([FromQuery] string someQ = "nice query")
    {
        return await _context.TodoItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    // GET: api/Todo/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }

    // PUT: api/Todo/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDto todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Todo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoItemDto>> PostTodoItem(TodoItemDto todoDto)
    {
        var todoItem = new TodoItem
        {
            IsComplete = todoDto.IsComplete,
            Name = todoDto.Name
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }

    // DELETE: api/Todo/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    private static TodoItemDto ItemToDTO(TodoItem todoItem)
    {
        return new TodoItemDto
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
    }
}