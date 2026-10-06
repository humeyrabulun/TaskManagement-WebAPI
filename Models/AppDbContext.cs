using Microsoft.EntityFrameworkCore;


namespace TodoApi.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opitons): base(opitons) {
        }
        public DbSet<Todo> Todos { get; set; }
        

    }
}
