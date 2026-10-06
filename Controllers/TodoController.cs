
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class TodoController : ControllerBase
    {

        private readonly TodoService _todoService;
        private readonly ITransientService _transient1;
        private readonly ITransientService _transient2;
        private readonly IScopedService _scoped1;
        private readonly IScopedService _scoped2;
        private readonly ISingletonService _singleton1;
        private readonly ISingletonService _singleton2;

        public TodoController(TodoService todoService,
            ITransientService transient1,
            ITransientService transient2,
            IScopedService scoped1,
            IScopedService scoped2,
            ISingletonService singleton1,
            ISingletonService singleton2)
        {
            _todoService = todoService;
            _transient1 = transient1;
            _transient2 = transient2;
            _scoped1 = scoped1;
            _scoped2 = scoped2;
            _singleton1 = singleton1;
            _singleton2 = singleton2;
        }


        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] bool? tamamlandi, [FromQuery] string? ara)
        {
            var sonucListe = await _todoService.GetTodosAsync(tamamlandi, ara);

            return Ok(sonucListe);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            var sonuc = await _todoService.GetTodoAsync(id);

            if (sonuc == null)
            {
                return NotFound($"ID {id} bulunamadı");
            }
            return Ok(sonuc);
        }

        [HttpPost]

        public async Task<IActionResult> CreateTodo(Todo newTodo)
        {
            if (string.IsNullOrWhiteSpace(newTodo.Title))
            {
                return BadRequest("Görev Başlığı boş olamaz.");
            }

            try
            {
                var createdTodo = await _todoService.CreateTodoAsync(newTodo);
                return CreatedAtAction(nameof(GetTodo), new { id = createdTodo.Id }, createdTodo);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet("lifetimes")]
        public IActionResult GetLifetimes()
        {
            var sonuc = new
            {
                Transient = new
                {
                    BirinciCagri = _transient1.GetGuid(),
                    IkinciCagri = _transient2.GetGuid()
                },
                Scoped = new
                {
                    BirinciCagri = _scoped1.GetGuid(),
                    IkinciCagri = _scoped2.GetGuid()
                },
                Singleton = new
                {
                    BirinciCagri = _singleton1.GetGuid(),
                    IkinciCagri = _singleton2.GetGuid()
                }
            };

            return Ok(sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, Todo updatedTodo)
        {
            var sonuc = await _todoService.UpdateTodoAsync(id, updatedTodo);

            if (sonuc == null)
            {
                return NotFound($"ID{id} bulunamadı");

            }

            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {

            bool isDeleted = await _todoService.DeleteTodoAsync(id);

            if (!isDeleted)
            {
                return NotFound($"ID {id} bulunamadı");
            }


            return NoContent();
        }
        [HttpGet("test-error")]
        public async Task<IActionResult> TestError()
        {
            throw new Exception("planlı bir test patlamasıdır");

        }
    }
}
