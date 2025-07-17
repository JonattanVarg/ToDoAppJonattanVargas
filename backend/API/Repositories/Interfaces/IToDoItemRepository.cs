using API.Dtos;
using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IToDoItemRepository
    {
        Task<ToDoItem> GetByIdAsync(int id, string userId);
        Task<IEnumerable<ToDoItem>> GetAllAsync(string userId);
        Task<IEnumerable<ToDoItem>> GetByCompletionStatusAsync(string userId, bool isCompleted);
        Task<ToDoItem> AddAsync(ToDoItem item);
        Task UpdateAsync(ToDoItem item);
        Task DeleteAsync(int id, string userId);
        Task<ToDoItemMetricsDto> GetMetricsAsync(string userId);
    }
}
