
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TodoController : ControllerBase
    {
        private static List<Todo> _todos = new List<Todo>();

        [HttpGet]
        public IActionResult GetTodos([FromQuery]bool? tamamlandi, [FromQuery] string? ara)
        {
            var sorgu = _todos.AsQueryable();

            if(tamamlandi.HasValue)
            {
                sorgu = sorgu.Where(t => t.IsCompleted == tamamlandi.Value);

            }

            if (!string.IsNullOrWhiteSpace(ara))
            {
                sorgu=sorgu.Where(t=>t.Title.Contains(ara));
            }

            return Ok(sorgu.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetTodo(int id) { 

            var todo= _todos.FirstOrDefault(t=>t.Id==id);
            if (todo == null) return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public IActionResult CreateTodo(Todo newTodo)
        {
            if (string.IsNullOrWhiteSpace(newTodo.Title))
            {
                return BadRequest("Görev Başlığı boş olamaz.")
            }

            newTodo.Id = _todos.Any() ? _todos.Max(t => t.Id) + 1 : 1;
            _todos.Add(newTodo);

            return CreatedAtAction(nameof(GetTodo), new { id = newTodo.Id }, newTodo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, Todo updatedTodo)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo==null) return NotFound($"ID {id} bulunamadı");

            todo.Title = updatedTodo.Title;
            todo.IsCompleted= updatedTodo.IsCompleted;


            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return NotFound($"ID {id} bulunamadı");

            _todos.Remove(todo);
            return NoContent();
        }
    }

}
