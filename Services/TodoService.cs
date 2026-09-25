using TodoApi.Models;
using Microsoft.Extensions.Options;


namespace TodoApi.Services
{
    public class TodoService
    {
        private static List<Todo> _todos = new List<Todo>();
        private readonly AppSettings _appSettings;

        public TodoService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }
       
        public List<Todo>GetTodos( bool? tamamlandi, string? ara)
        {
            var sorgu = _todos.AsQueryable();

            if (tamamlandi.HasValue)
            {
                sorgu = sorgu.Where(t => t.IsCompleted == tamamlandi.Value);

            }

            if (!string.IsNullOrWhiteSpace(ara))
            {
                sorgu = sorgu.Where(t => t.Title.Contains(ara));
            }

            return sorgu.ToList();
        }

        public Todo GetTodo(int id)
        {

            var todo = _todos.FirstOrDefault(t => t.Id == id);
            return (todo);
        }

     
        public Todo CreateTodo(Todo newTodo)
        {
            Console.WriteLine($"ŞU ANKİ LİMİT: {_appSettings.MaksimumTodoSayisi}");
            Console.WriteLine($"ŞU ANKİ GÖREV SAYISI: {_todos.Count}");

            if (_todos.Count >= _appSettings.MaksimumTodoSayisi)
            {
                throw new Exception($"{_appSettings.UygulamaAdi} kapasitesi doldu! En fazla {_appSettings.MaksimumTodoSayisi} görev eklenebilir.");
            }
            newTodo.Id = _todos.Any() ? _todos.Max(t => t.Id) + 1 : 1;
            _todos.Add(newTodo);
            return newTodo;

        }

   
        public Todo UpdateTodo(int id, Todo updatedTodo)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            return updatedTodo;

            if (todo == null) return null;

            todo.Title = updatedTodo.Title;
            todo.IsCompleted = updatedTodo.IsCompleted;

            return todo;
        }


        public bool DeleteTodo(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);

            if (todo==null) return false;

            _todos.Remove(todo);
            return true;

         
        }


    
    }
}
