using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoCRUD.Models.Dtos;
using ToDoCRUD.Models.Entities;
using ToDoCRUD.Models.Interfaces;

namespace ToDoCRUD.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IToDoRepo _toDoRepo;
        private readonly IToDoLongPollingService _longPollingService;

        public ToDoController(IMapper mapper, IToDoRepo toDoRepo, IToDoLongPollingService longPollingService)
        {
            _mapper = mapper;
            _toDoRepo = toDoRepo;
            _longPollingService = longPollingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllToDoItemsAsync(Guid? lastToDoItemId, int? timeout)
        {
            try
            {
                var allToDoItems = await _toDoRepo.GetAllToDoItemsAsync();
                var allToDoItemsDto = _mapper.Map<ICollection<ToDoItemDto>>(allToDoItems);

                if (lastToDoItemId != null && timeout != null)
                {
                    var allNewToDoItems = await _longPollingService.ProcessToDoLongPollingRequestAsync(lastToDoItemId.Value, timeout.Value);
                    var allNewToDoItemsDto = _mapper.Map<ICollection<ToDoItemDto>>(allNewToDoItems);
                    // Return empty array
                    if (!allNewToDoItemsDto.Any())
                    {                      
                        return Ok(allNewToDoItemsDto);
                    }

                    allToDoItemsDto = allToDoItemsDto.Concat(allNewToDoItemsDto).OrderByDescending(atdi => atdi.UpdateDate).ToList();
                }

                return Ok(allToDoItemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{toDoItemId}")]
        public async Task<IActionResult> GetToDoItemAsync(Guid toDoItemId)
        {
            try
            {
                var toDoItem = await _toDoRepo.GetToDoItemAsync(toDoItemId);
                if (toDoItem == null)
                {
                    return NotFound();
                }

                return Ok(toDoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToDoItemAsync([FromBody] ToDoItemDto toDoItemDto)
        {
            try
            {
                var toDoItem = _mapper.Map<ToDoItem>(toDoItemDto);
                var addedToDoItem = await _toDoRepo.AddToDoItemAsync(toDoItem);

                return Ok(addedToDoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{toDoItemId}")]
        public async Task<IActionResult> UpdateToDoItemAsync(Guid toDoItemId, [FromBody] ToDoItemDto toDoItemDto)
        {
            try
            {
                var toDoItemToUpdate = await _toDoRepo.GetToDoItemAsync(toDoItemId);
                if (toDoItemToUpdate == null)
                {
                    return NotFound();
                }

                var toDoItem = _mapper.Map<ToDoItem>(toDoItemDto);
                var updatedToDoItem = await _toDoRepo.UpdateToDoItemAsync(toDoItem);

                return Ok(updatedToDoItem);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{toDoItemId}")]
        public async Task<IActionResult> DeleteToDoItemAsync(Guid toDoItemId)
        {
            try
            {
                var toDoItemToDelete = await _toDoRepo.GetToDoItemAsync(toDoItemId);
                if (toDoItemToDelete == null)
                {
                    return NotFound();
                }

                var deletedToDoItem = await _toDoRepo.DeleteToDoItemAsync(toDoItemId);

                return Ok(deletedToDoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
