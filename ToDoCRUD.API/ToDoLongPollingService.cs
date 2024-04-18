using ToDoCRUD.Models.Entities;
using ToDoCRUD.Models.Interfaces;

namespace ToDoCRUD.API
{
    public class ToDoLongPollingService : IToDoLongPollingService
    {
        private readonly IToDoRepo _toDoRepo;

        public ToDoLongPollingService(IToDoRepo toDoRepo)
        {
            _toDoRepo = toDoRepo;
        }

        public async Task<ICollection<ToDoItem>> ProcessToDoLongPollingRequestAsync(Guid lastItemId, int timeout)
        {
            var lastItem = await _toDoRepo.GetToDoItemAsync(lastItemId);
            var startTime = DateTime.UtcNow;
            var currentTime = startTime;

            while ((currentTime - startTime).TotalMilliseconds < timeout)
            {
                var toDoItemsNewerThanDate = await _toDoRepo.GetToDoItemsNewerThanDateTime(lastItem.UpdateDate);
                if (toDoItemsNewerThanDate.Any())
                {
                    return toDoItemsNewerThanDate;
                }

                await Task.Delay(250);
                currentTime = DateTime.UtcNow;
            }

            return new List<ToDoItem>();
        }
    }
}
