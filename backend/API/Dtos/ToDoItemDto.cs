using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ToDoItemDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Titulo no puede exceder 100 caracteres")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descripcion no puede exceder 500 caracteres")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        [Display(Name = "User ID")]
        public string? UserId { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public string Status => IsCompleted ? "Completado" : "Pendiente";
    }
}
