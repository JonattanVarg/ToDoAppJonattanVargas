using API.Dtos;
using API.Dtos.Responses;

namespace API.Services.Interfaces
{
    public interface IToDoItemService
    {
        Task<GenericResponseDto<ToDoItemDto>> GetByIdAsync(int id, string userId);
        Task<GenericResponseDto<IEnumerable<ToDoItemDto>>> GetAllAsync(string userId);
        Task<GenericResponseDto<IEnumerable<ToDoItemDto>>> GetByCompletionStatusAsync(string userId, bool isCompleted);
        Task<GenericResponseDto<ToDoItemDto>> CreateAsync(CreateToDoItemDto createDto, string userId);
        Task<GenericResponseDto<ToDoItemDto>> UpdateAsync(int id, UpdateToDoItemDto updateDto, string userId);
        Task<GenericResponseDto<ToDoItemDto>> DeleteAsync(int id, string userId);
        Task<GenericResponseDto<ToDoItemMetricsDto>> GetMetricsAsync(string userId);
    }
}
