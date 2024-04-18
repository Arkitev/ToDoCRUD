using Microsoft.EntityFrameworkCore;
using ToDoCRUD.Models.Entities;

namespace ToDoCRUD.API.Data
{
    public class ToDoAppContext : DbContext
    {
        public ToDoAppContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
