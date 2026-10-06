using TodoApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;



namespace TodoApi.Services
{
    public class TodoService
    {
        private readonly AppDbContext _context;

        private readonly AppSettings _appSettings;

        private readonly ILogger<TodoService> _logger;

        public TodoService(AppDbContext context,IOptions<AppSettings> options,ILogger<TodoService> logger)
        {
            _appSettings = options.Value;
            _logger = logger;
            _context = context;
        }

        public async Task<List<Todo>> GetTodosAsync(bool? tamamlandi, string? ara)
        {
            var sorgu = _context.Todos.AsQueryable();

            if (tamamlandi.HasValue)
            {
                sorgu = sorgu.Where(t => t.IsCompleted == tamamlandi.Value);

            }

            if (!string.IsNullOrWhiteSpace(ara))
            {
                sorgu = sorgu.Where(t => t.Title.Contains(ara));
            }

            return await sorgu.ToListAsync();
        }

        public async Task<Todo> GetTodoAsync(int id)
        {

            return await _context.Todos.FindAsync(id);
        }

     
        public async Task<Todo> CreateTodoAsync(Todo newTodo)
        {

            

            _logger.LogInformation("Yeni bir Todo ekleme isteği geldi. Başlık: {Title}", newTodo.Title);
            if (await _context.Todos.CountAsync() >= _appSettings.MaksimumTodoSayisi)
            {
                _logger.LogWarning("DİKKAT: Todo kapasitesi ({Kapasite}) doldu. İstek reddedildi!", _appSettings.MaksimumTodoSayisi);
                throw new Exception($"{_appSettings.UygulamaAdi} kapasitesi doldu!");
            }
            await _context.Todos.AddAsync(newTodo);
            await _context.SaveChangesAsync(); 
            return newTodo;

        }

   
        public async Task<Todo> UpdateTodoAsync(int id, Todo updatedTodo)
        {
            

            var todo = await _context.Todos.FindAsync(id);

            if (todo == null) return null;

            todo.Title = updatedTodo.Title;
            todo.IsCompleted = updatedTodo.IsCompleted;

            await _context.SaveChangesAsync();

            return todo;
        }


        public async Task<bool> DeleteTodoAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null) return false;

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return true;

         
        }


    
    }
}
