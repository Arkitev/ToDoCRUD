using ToDoCRUD.Saver;

public class Program
{
    private static async Task Main(string[] args)
    {
        var toDoItemsSaver = new ToDoItemsSaver("https://localhost:7224/", "ToDoItemsCurrentSnapshot.json");

        // Short polling
        /*var timerIntervalMiliseconds = 10000;
        var timer = new Timer(async (_) => await toDoItemsSaver.FetchAndSaveToFileShortPollingAsync(), null, 0, timerIntervalMiliseconds);

        Console.WriteLine("Press 'q' to quit the application.");
        while (Console.ReadKey().Key != ConsoleKey.Q)
        {
            // Keep running application until 'q' is pressed
        }*/

        // Long polling
        var serverTimeoutMiliseconds = 10000;
        Guid? initialToDoItemId = null;

        while (true)
        {
            try
            {
                Console.WriteLine("Start fetching...");
                var lastNewToDoItem = await toDoItemsSaver.FetchAndSaveToFileLongPollingAsync(initialToDoItemId, serverTimeoutMiliseconds);
                if (lastNewToDoItem != null)
                {
                    initialToDoItemId = lastNewToDoItem;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }

            await Task.Delay(5000);
        }
    }
}