using System.Text.Json;
using ToDoCRUD.Models.Dtos;

namespace ToDoCRUD.Saver
{
    public class ToDoItemsSaver
    {
        private readonly string _apiEndpoint;
        private readonly string _outputFile;

        public ToDoItemsSaver(string apiEndpoint, string outputFile)
        {
            _apiEndpoint = apiEndpoint;
            _outputFile = outputFile;
        }

        public async Task FetchAndSaveToFileShortPollingAsync()
        {
            try
            {
                using (var toDoApiClient = new HttpClient())
                {
                    toDoApiClient.BaseAddress = new Uri(_apiEndpoint);

                    var response = await toDoApiClient.GetAsync("ToDo");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();

                        // Serialize and save to-do-items to local file
                        var jsonSerializerOptions = new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };
                        var toDoItems = JsonSerializer.Deserialize<List<ToDoItemDto>>(responseBody, jsonSerializerOptions);
                        var toDoItemsJson = JsonSerializer.Serialize(toDoItems, jsonSerializerOptions);
                        await File.WriteAllTextAsync($"{_outputFile}", toDoItemsJson);

                        Console.WriteLine($"Items saved to {_outputFile} successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get to-do-items. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }

        public async Task<Guid?> FetchAndSaveToFileLongPollingAsync(Guid? lastToDoItemId, int timeoutMiliseconds)
        {
            try
            {
                using (var toDoApiClient = new HttpClient())
                {
                    toDoApiClient.BaseAddress = new Uri(_apiEndpoint);

                    var response = await toDoApiClient.GetAsync($"ToDo?lastToDoItemId={lastToDoItemId}&timeout={timeoutMiliseconds}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();

                        // Serialize and save to-do-items to local file
                        var jsonSerializerOptions = new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };
                        var toDoItems = JsonSerializer.Deserialize<List<ToDoItemDto>>(responseBody, jsonSerializerOptions);
                        var orderedToDoItems = toDoItems.OrderByDescending(tdi => tdi.UpdateDate).ToList();
                        var lastToDoItem = toDoItems.FirstOrDefault();

                        if (toDoItems.Any())
                        {
                            var toDoItemsJson = JsonSerializer.Serialize(toDoItems, jsonSerializerOptions);
                            await File.WriteAllTextAsync($"{_outputFile}", toDoItemsJson);

                            Console.WriteLine($"Items saved to {_outputFile} successfully.\n");

                            return lastToDoItem.Id;
                        } 

                        Console.WriteLine($"No new items. Connection timed out.\n");

                        return null;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get to-do-items. Status code: {response.StatusCode}");

                        return lastToDoItemId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
