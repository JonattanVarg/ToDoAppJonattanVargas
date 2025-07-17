using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ToDoItem
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Foreign key to user
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }
    }
}
