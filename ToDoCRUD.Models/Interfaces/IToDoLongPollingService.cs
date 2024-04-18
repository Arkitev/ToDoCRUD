using ToDoCRUD.Models.Entities;

namespace ToDoCRUD.Models.Interfaces
{
    public interface IToDoLongPollingService
    {
        Task<ICollection<ToDoItem>> ProcessToDoLongPollingRequestAsync(Guid lastToDoItemId, int timeout);
    }
}
