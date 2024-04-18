using AutoMapper;
using ToDoCRUD.Models.Dtos;
using ToDoCRUD.Models.Entities;

namespace ToDoCRUD.Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoItem, ToDoItemDto>();
            CreateMap<ToDoItemDto, ToDoItem>();
        }
    }
}
