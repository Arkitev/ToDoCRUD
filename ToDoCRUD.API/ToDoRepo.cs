using Microsoft.EntityFrameworkCore;
using ToDoCRUD.API.Data;
using ToDoCRUD.Models.Entities;
using ToDoCRUD.Models.Interfaces;

namespace ToDoCRUD.API
{
    public class ToDoRepo : IToDoRepo
    {
        private readonly ToDoAppContext _toDoAppContext;

        public ToDoRepo(ToDoAppContext toDoAppContext)
        {
            _toDoAppContext = toDoAppContext;
        }

        public async Task<ICollection<ToDoItem>> GetAllToDoItemsAsync()
        {
            // Mocked data for test
            /*            var toDoItemsList = new List<ToDoItem>();
                        for (var i = 0; i < 10; i++)
                        {
                            toDoItemsList.Add(new ToDoItem()s
                            {
                                Id = Guid.NewGuid(),
                                Name = $"name{i}",
                                Description = $"description{i}",
                            });
                        }*/

            var allToDoItems = await _toDoAppContext.ToDoItems
                .OrderByDescending(tdi => tdi.UpdateDate)
                .ToListAsync();

            return allToDoItems;
        }

        public async Task<ToDoItem> GetToDoItemAsync(Guid toDoItemId)
        {
            var toDoItem = await _toDoAppContext.ToDoItems
                .AsNoTracking()
                .Where(tdi => tdi.Id == toDoItemId)
                .SingleOrDefaultAsync();

            return toDoItem;
        }

        public async Task<ToDoItem> AddToDoItemAsync(ToDoItem toDoItem)
        {
            toDoItem.Id = Guid.NewGuid();
            toDoItem.UpdateDate = DateTime.UtcNow;
            _toDoAppContext.Add(toDoItem);
            await _toDoAppContext.SaveChangesAsync();

            return toDoItem;
        }

        public async Task<ToDoItem> UpdateToDoItemAsync(ToDoItem toDoItem)
        {
            toDoItem.UpdateDate = DateTime.UtcNow;
            _toDoAppContext.Entry(toDoItem).State = EntityState.Modified;
            await _toDoAppContext.SaveChangesAsync();

            return toDoItem;
        }

        public async Task<ToDoItem> DeleteToDoItemAsync(Guid toDoItemId)
        {
            var toDoItem = await _toDoAppContext.ToDoItems.FindAsync(toDoItemId);
            _toDoAppContext.ToDoItems.Remove(toDoItem);
            await _toDoAppContext.SaveChangesAsync();

            return toDoItem;
        }

        public async Task<ICollection<ToDoItem>> GetToDoItemsNewerThanDateTime(DateTime dateTime)
        {
            var toDoItemsNewerThanDateTime = await _toDoAppContext.ToDoItems
                .Where(tdi => tdi.UpdateDate > dateTime)
                .OrderByDescending(tdi => tdi.UpdateDate)
                .ToListAsync();

            return toDoItemsNewerThanDateTime;
        }
    }
}
