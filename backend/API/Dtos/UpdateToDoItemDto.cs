using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UpdateToDoItemDto
    {
        /// <summary>
        /// El titulo de la tarea.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Titulo no puede exceder 100 caracteres")]
        [SwaggerSchema(Description = "El título de la tarea.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// La descripción de la tarea
        /// </summary>
        [StringLength(500)]
        [SwaggerSchema(Description = "La descripción de la tarea")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// true o false dependiendo si la tarea ha sido completada o no
        /// </summary>
        [SwaggerSchema(Description = "true o false dependiendo si la tarea ha sido completada o no")] 
        public bool IsCompleted { get; set; }
    }
}
