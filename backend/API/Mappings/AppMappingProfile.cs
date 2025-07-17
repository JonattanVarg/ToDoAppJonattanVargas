using API.Dtos;
using API.Models;
using AutoMapper;

namespace API.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<ToDoItem, ToDoItemDto>();
            CreateMap<CreateToDoItemDto, ToDoItem>();
            CreateMap<UpdateToDoItemDto, ToDoItem>();
        }
    }
}
