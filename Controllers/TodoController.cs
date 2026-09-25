
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
        public IActionResult GetTodos([FromQuery] bool? tamamlandi, [FromQuery] string? ara)
        {
            var sonucListe = _todoService.GetTodos(tamamlandi, ara);

            return Ok(sonucListe);
        }

        [HttpGet("{id}")]
        public IActionResult GetTodo(int id)
        {
            var sonuc = _todoService.GetTodo(id);

            if (sonuc == null)
            {
                return NotFound($"ID {id} bulunamadı");
            }
            return Ok(sonuc);
        }

        [HttpPost]

        public IActionResult CreateTodo(Todo newTodo)
        {
            if (string.IsNullOrWhiteSpace(newTodo.Title))
            {
                return BadRequest("Görev Başlığı boş olamaz.");
            }

            try
            {
                var createdTodo = _todoService.CreateTodo(newTodo);
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
        public IActionResult UpdateTodo(int id, Todo updatedTodo)
        {
            var sonuc = _todoService.UpdateTodo(id, updatedTodo);

            if (sonuc == null)
            {
                return NotFound($"ID{id} bulunamadı");

            }

            return NoContent();

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {

            bool isDeleted = _todoService.DeleteTodo(id);

            if (!isDeleted)
            {
                return NotFound($"ID {id} bulunamadı");
            }


            return NoContent();
        }

    }
}
