using API.Dtos;
using API.Dtos.Responses;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoItemsController : ControllerBase
    {
        private readonly IToDoItemService _toDoItemService;
        private readonly ILogger<ToDoItemsController> _logger;

        public ToDoItemsController(IToDoItemService toDoItemService, ILogger<ToDoItemsController> logger)
        {
            _toDoItemService = toDoItemService;
            _logger = logger;
        }

        // GET: api/todoitems
        /// <summary>
        /// Obtiene todas los ToDoItems de un usuario
        /// </summary>
        /// <returns>Una lista de ToDoItems.</returns>
        /// <response code="200">Operación exitosa. Devuelve la lista de ToDoItems.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtiene todas las tareas - ToDoItems del usuario",
                          Description = "Recupera una lista de todas los ToDoItems disponibles.")]
        [SwaggerResponse(200, "Operación exitosa", typeof(GenericResponseDto<IEnumerable<ToDoItemDto>>))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetToDoItems()
        {
            try
            {
                _logger.LogInformation("Iniciando solicitud para recuperar todas los ToDoItems de un usuario");

                var userId = User.GetUserId();
                var response = await _toDoItemService.GetAllAsync(userId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado al obtener todas los ToDoItems");
                return StatusCode(500, new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde.",
                    Data = null
                }); ;
            }
        }

        // GET: api/todoitems/completed
        /// <summary>
        /// Obtiene todas los ToDoItem completados de un usuario
        /// </summary>
        /// <returns>Una lista de ToDoItem.</returns>
        /// <response code="200">Operación exitosa. Devuelve la lista de ToDoItem completados de un usuario </response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("completed")]
        [SwaggerOperation(Summary = "Obtiene todas las tares - ToDoItems completados del usuario",
                          Description = "Recupera una lista de todas los ToDoItems completados de un usuario.")]
        [SwaggerResponse(200, "Operación exitosa", typeof(GenericResponseDto<IEnumerable<ToDoItemDto>>))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetCompletedToDoItems()
        {
            try
            {
                _logger.LogInformation("Iniciando solicitud para recuperar todas los ToDoItems completados de un usuario");

                var userId = User.GetUserId();
                var response = await _toDoItemService.GetByCompletionStatusAsync(userId, true);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado al obtener todas los ToDoItems completados.");
                return StatusCode(500, new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde.",
                    Data = null
                }); ;
            }
        }

        // GET: api/todoitems/pending
        /// <summary>
        /// Obtiene todas los ToDoItem pendientes de un usuario
        /// </summary>
        /// <returns>Una lista de ToDoItem.</returns>
        /// <response code="200">Operación exitosa. Devuelve la lista de ToDoItem pendientes de un usuario </response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("pending")]
        [SwaggerOperation(Summary = "Obtiene todas las tares - ToDoItems pendientes por completar del usuario",
                          Description = "Recupera una lista de todas los ToDoItems pendientes por completar de un usuario.")]
        [SwaggerResponse(200, "Operación exitosa", typeof(GenericResponseDto<IEnumerable<ToDoItemDto>>))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetPendingToDoItems()
        {
            try
            {
                _logger.LogInformation("Iniciando solicitud para recuperar todas los ToDoItems pendientes de un usuario");

                var userId = User.GetUserId();
                var response = await _toDoItemService.GetByCompletionStatusAsync(userId, false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado al obtener todas los ToDoItems pendientes.");
                return StatusCode(500, new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde.",
                    Data = null
                }); ;
            }
        }

        // GET: api/todoitems/5
        /// <summary>
        /// Obtiene una ToDoItem por su ID, de un usuario
        /// </summary>
        /// <param name="id">El ID del ToDoItem.</param>
        /// <returns>Un ToDoItem si se encuentra; de lo contrario, un error 404.</returns>
        /// <response code="200">Operación exitosa. Devuelve el ToDoItem.</response>
        /// <response code="404">No se encontró el ToDoItem con el ID proporcionado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtiene una tarea - ToDoItem por su ID, de un usuario",
                          Description = "Recupera uno ToDoItem por su , de un usuario.")]
        [SwaggerResponse(200, "Operación exitosa", typeof(GenericResponseDto<ToDoItemDto>))]
        [SwaggerResponse(404, "No se encontró el ToDoItemDto con el ID proporcionado")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetToDoItem(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando solicitud para recuperar el ToDoItem con ID: {Id}", id);
                var userId = User.GetUserId();
                var response = await _toDoItemService.GetByIdAsync(id, userId);

                if (!response.IsSuccess && response.Data == null) return NotFound(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado al obtener ToDoItem");
                return StatusCode(500, new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde."
                });
            }
        }

        // POST: api/todoitems
        /// <summary>
        /// Crea un nuevo ToDoItem
        /// </summary>
        /// <param name="CreateToDoItemDto">El DTO que contiene los datos del ToDoItem.</param>
        /// <returns>El ToDoItem creado.</returns>
        /// <response code="201">Operación exitosa. Devuelve el ToDoItem creado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Crea una nueva tarea - ToDoItem",
                          Description = "Permite crear un nueva una tarea -  ToDoItem proporcionando los datos necesarios.")]
        [SwaggerResponse(201, "Operación exitosa", typeof(GenericResponseDto<ToDoItemDto>))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<ActionResult<ToDoItemDto>> PostToDoItem([FromBody] CreateToDoItemDto createDto)
        {
            var response = new GenericResponseDto<ToDoItemDto>();

            try
            {
                if (!ModelState.IsValid)
                {
                    response = new GenericResponseDto<ToDoItemDto>
                    {
                        IsSuccess = false,
                        Message = "Modelo de datos no válido.",
                        Data = null
                    };
                    return BadRequest(response);
                }

                _logger.LogInformation("Iniciando solicitud para agregar ToDoItem");
                var userId = User.GetUserId();
                response  = await _toDoItemService.CreateAsync(createDto, userId);

                return CreatedAtAction(nameof(GetToDoItem), new { id = response.Data!.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al crear un nuevo ToDoItem");
                response = new GenericResponseDto<ToDoItemDto>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde."
                };
                return StatusCode(500, response);
            }
        }

        // PUT: api/todoitems/5
        /// <summary>
        /// Actualiza un ToDoItem existente.
        /// </summary>
        /// <param name="id">El ID del ToDoItem a actualizar.</param>
        /// <param name="jobOfferUpdateDto">El DTO que contiene los nuevos datos del ToDoItem.</param>
        /// <returns>Un objeto  que contiene el ToDoItem actualizada en caso de éxito.</returns>
        /// <response code="200">Operación exitosa. Devuelve el ToDoItem actualizado.</response>
        /// <response code="400">Solicitud inválida. El ID no coincide con el del modelo de datos.</response>
        /// <response code="404">No se encontró el ToDoItem con el ID especificado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Actualiza una tarea - ToDoItem existente",
                          Description = "Permite actualizar un ToDoItem existente utilizando su ID y los nuevos datos.")]
        [SwaggerResponse(200, "Operación exitosa. Devuelve el ToDoItem actualizado.", typeof(GenericResponseDto<ToDoItemDto>))]
        [SwaggerResponse(404, "No se encontró el ToDoItem con el ID especificado.")]
        [SwaggerResponse(500, "Error interno del servidor.")]
        public async Task<IActionResult> PutToDoItem(int id, [FromBody] UpdateToDoItemDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new GenericResponseDto<UpdateToDoItemDto>
                    {
                        IsSuccess = false,
                        Message = "Modelo de datos no válido.",
                        Data = null
                    });
                }

                _logger.LogInformation("Iniciando solicitud para actualizar el ToDoItem con ID: {Id}", id);
                var userId = User.GetUserId();
                var response = await _toDoItemService.UpdateAsync(id, updateDto, userId);

                if (!response.IsSuccess && response.Data == null) return NotFound(response);

                _logger.LogInformation("ToDoItem con ID: {Id} actualizado exitosamente.", id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al actualizar el ToDoItem con ID: {Id}", id);
                var response = new GenericResponseDto<ToDoItemDto>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde."
                };
                return StatusCode(500, response);
            }
        }

        // DELETE: api/todoitems/5
        /// <summary>
        /// Elimina un ToDoItem existente de un usario
        /// </summary>
        /// <param name="id">El ID del ToDoItem</param>
        /// <returns>Un objeto que contiene el ID del ToDoItem eliminado</returns>
        /// <response code="200">Operación exitosa..</response>
        /// <response code="404">No se encontró el ToDoItem con el ID especificado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Elimina una tarea - ToDoItem existente de un usuario",
                          Description = "Permite eliminar un ToDoItem utilizando su ID.")]
        [SwaggerResponse(200, "Operación exitosa.", typeof(GenericResponseDto<ToDoItemDto>))]
        [SwaggerResponse(404, "No se encontró el ToDoItem con el ID especificado.")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando solicitud para eliminar el ToDoItem con ID: {Id}", id);

                var userId = User.GetUserId();
                var response = await _toDoItemService.DeleteAsync(id, userId);

                if (!response.IsSuccess && response.Data == null) return NotFound(response);

                _logger.LogInformation("ToDoItem con ID: {Id} eliminada exitosamente.", id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al eliminar el ToDoItem con ID: {Id}", id);
                var errorResponse = new GenericResponseDto<ToDoItemDto>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde."
                };
                return StatusCode(500, errorResponse);
            }
        }

        // GET: api/todoitems/metrics
        /// <summary>
        /// Obtiene las metricas de un usuario
        /// </summary>
        /// <returns>Metricas del usuario.</returns>
        /// <response code="200">Operación exitosa. Devuelve las metricas del usuario.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("metrics")]
        [SwaggerOperation(Summary = "Obtiene todas las metricas del usuario",
                          Description = "Recupera una lista de todas las metricas del usuario.")]
        [SwaggerResponse(200, "Operación exitosa. Devuelve las metricas del usuario.", typeof(GenericResponseDto<IEnumerable<ToDoItemMetricsDto>>))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<ActionResult<ToDoItemMetricsDto>> GetMetrics()
        {
            try
            {
                _logger.LogInformation("Iniciando solicitud para recuperar todas las metricas de un usuario");

                var userId = User.GetUserId();
                var response = await _toDoItemService.GetMetricsAsync(userId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado al obtener todas las metricas");
                return StatusCode(500, new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde.",
                    Data = null
                }); ;
            }
        }

    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }
    }
}
