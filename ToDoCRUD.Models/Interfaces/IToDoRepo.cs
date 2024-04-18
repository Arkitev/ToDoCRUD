using ToDoCRUD.Models.Entities;

namespace ToDoCRUD.Models.Interfaces
{
    public interface IToDoRepo
    {
        Task<ICollection<ToDoItem>> GetAllToDoItemsAsync();
        Task<ToDoItem> GetToDoItemAsync(Guid toDoItemId);
        Task<ToDoItem> AddToDoItemAsync(ToDoItem toDoItem);
        Task<ToDoItem> UpdateToDoItemAsync(ToDoItem toDoItem);
        Task<ToDoItem> DeleteToDoItemAsync(Guid toDoItemId);
        Task<ICollection<ToDoItem>> GetToDoItemsNewerThanDateTime(DateTime dateTime);
    }
}
