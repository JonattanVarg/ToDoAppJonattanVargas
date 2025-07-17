namespace API.Services
{
    #region Namespaces
    using API.Dtos;
    using API.Dtos.Responses;
    using API.Models;
    using API.Repositories.Interfaces;
    using API.Services.Interfaces;
    using AutoMapper;
    #endregion

    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _repository;
        private readonly ILogger<ToDoItemService> _logger;
        private readonly IMapper _mapper;

        public ToDoItemService(IToDoItemRepository repository, ILogger<ToDoItemService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera un ToDoItem por su ID desde el repositorio.
        /// </summary>
        /// <returns>Un objeto <see cref="GenericResponseDto{ToDoItemDto}"/> que representa el ToDoItem, o un error 404 si no se encuentra.</returns>
        public async Task<GenericResponseDto<ToDoItemDto>> GetByIdAsync(int id, string userId)
        {
            try
            {
                _logger.LogInformation("Recuperando el ToDoItem con ID: {Id} y {userId} desde el repositorio.", id, userId);
                var item = await _repository.GetByIdAsync(id, userId);

                if (item == null)
                {
                    _logger.LogWarning("No se encontró el ToDoItem con ID: {Id} y {userId} desde el repositorio.", id, userId);
                    return new GenericResponseDto<ToDoItemDto>
                    {
                        IsSuccess = false,
                        Message = "No se encontró el ToDoItem.",
                        Data = null
                    };
                }

                var toDoItemDto = _mapper.Map<ToDoItemDto>(item);
                return new GenericResponseDto<ToDoItemDto>
                {
                    IsSuccess = true,
                    Message = "ToDoItem recuperado exitosamente.",
                    Data = toDoItemDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al recuperar ToDoItem  con ID: {Id} y {userId}.", id, userId);
                return new GenericResponseDto<ToDoItemDto>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde."
                };
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas los ToDoItem de un usuario con el userId
        /// </summary>
        /// <returns>Una lista de todos los ToDoItem de un usuario</returns>
        public async Task<GenericResponseDto<IEnumerable<ToDoItemDto>>> GetAllAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Recuperando todos ToDoItemDto desde el repositorio.");

                var items = await _repository.GetAllAsync(userId);

                if (items == null || !items.Any())
                {
                    _logger.LogWarning("No se encontraron ToDoItemDto  disponibles.");
                    return new GenericResponseDto<IEnumerable<ToDoItemDto>>
                    {
                        IsSuccess = true,
                        Message = "No hay ToDoItemDto disponibles en este momento.",
                        Data = Enumerable.Empty<ToDoItemDto>()
                    };
                }

                _logger.LogInformation("ToDoItemDto recuperados exitosamente.");
                var toDoItems = _mapper.Map<IEnumerable<ToDoItemDto>>(items);

                return new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = true,
                    Message = "ToDoItemDto recuperados exitosamente.",
                    Data = toDoItems
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al recuperar todas los ToDoItemDto.");
                return new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno al recuperar los ToDoItemDto.",
                    Data = null
                };
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas los ToDoItem de un usuario con el userId y su estado de completado o no completado
        /// </summary>
        /// <param name="userId">El id del usuario</param>
        /// <param name="isCompleted">true o false dependiendo del estado que se busque </param>
        /// <returns>Una lista de todos los ToDoItem de un usuario dependiendo del estado completado o no completado</returns>
        public async Task<GenericResponseDto<IEnumerable<ToDoItemDto>>> GetByCompletionStatusAsync(string userId, bool isCompleted)
        {
            try
            {
                _logger.LogInformation("Recuperando todos ToDoItemDto desde el repositorio.");

                var items = await _repository.GetByCompletionStatusAsync(userId, isCompleted);

                if (items == null || !items.Any())
                {
                    _logger.LogWarning("No se encontraron ToDoItemDto  disponibles.");
                    return new GenericResponseDto<IEnumerable<ToDoItemDto>>
                    {
                        IsSuccess = true,
                        Message = "No hay ToDoItemDto disponibles en este momento.",
                        Data = Enumerable.Empty<ToDoItemDto>()
                    };
                }

                _logger.LogInformation("ToDoItemDto recuperados exitosamente.");
                var toDoItems = _mapper.Map<IEnumerable<ToDoItemDto>>(items);

                return new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = true,
                    Message = "ToDoItemDto recuperados exitosamente.",
                    Data = toDoItems
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al recuperar todas los ToDoItemDto.");
                return new GenericResponseDto<IEnumerable<ToDoItemDto>>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno al recuperar los ToDoItemDto.",
                    Data = null
                };
                throw;
            }
        }

        /// <summary>
        /// Agrega un nuevo ToDoItem al repositorio.
        /// </summary>
        /// <param name="CreateToDoItemDto">El objeto <see cref="CreateToDoItemDto"/> que contiene los datos del ToDoItemDto a agregar.</param>
        /// <returns>Un objeto <see cref="GenericResponseDto{CreateToDoItemDto}"/> que contiene el resultado de la operación.</returns>
        public async Task<GenericResponseDto<ToDoItemDto>> CreateAsync(CreateToDoItemDto createDto, string userId)
        {
            var response = new GenericResponseDto<ToDoItemDto>();

            try
            {
                var item = _mapper.Map<ToDoItem>(createDto);
                item.UserId = userId;
                _logger.LogInformation("Agregando un nuevo ToDoItemDto");

                var createdItem = await _repository.AddAsync(item);

                response.IsSuccess = true;
                response.Message = "ToDoItemDto creado exitosamente.";
                response.Data = _mapper.Map<ToDoItemDto>(createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al agregar un nuevo ToDoItemDto.");
                response.IsSuccess = false;
                response.Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde.";
                response.Data = null;
                throw;
            }

            return response;
        }

        /// <summary>
        /// Actualiza un ToDoItem en el repositorio.
        /// </summary>
        /// <param name="UpdateToDoItemDto">El objeto <see cref="UpdateToDoItemDto"/> que contiene los datos del ToDoItemDto a actualizar.</param>
        /// <returns>Un objeto <see cref="GenericResponseDto{UpdateToDoItemDto}"/> que contiene el resultado de la operación.</returns>
        public async Task<GenericResponseDto<ToDoItemDto>> UpdateAsync(int id, UpdateToDoItemDto updateDto, string userId)
        {
            var response = new GenericResponseDto<ToDoItemDto>();

            try
            {
                // Verificar si ToDoItemDto especificado existe
                var existingItem = await _repository.GetByIdAsync(id, userId);

                if (existingItem == null)
                {
                    _logger.LogWarning("No se encontró ToDoItemDto");

                    response.IsSuccess = false;
                    response.Message = "No se encontró ToDoItemDto";
                    response.Data = null;

                    return response;
                }

                existingItem = _mapper.Map(updateDto, existingItem);
                await _repository.UpdateAsync(existingItem);
                _logger.LogInformation("Actualizando ToDoItemDto");

                response.IsSuccess = true;
                response.Message = "ToDoItemDto actualizado exitosamente.";
                response.Data = _mapper.Map<ToDoItemDto>(existingItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al actualizar la oferta de trabajo con ID: {Id}.", id);
                response.IsSuccess = false;
                response.Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde.";
                response.Data = null;
                throw;
            }

            return response;
        }

        /// <summary>
        /// Elimina un ToDoItemDto existente del repositorio.
        /// </summary>
        /// <exception cref="Exception">Lanzada si ocurre un error durante la operación.</exception>
        public async Task<GenericResponseDto<ToDoItemDto>> DeleteAsync(int id, string userId)
        {
            try
            {
                // Verificar si ToDoItemDto especificado existe
                var existingItem = await _repository.GetByIdAsync(id, userId);

                if (existingItem == null)
                {
                    _logger.LogWarning("No se encontró ToDoItemDto");
                    return new GenericResponseDto<ToDoItemDto>
                    {
                        IsSuccess = false,
                        Message = "No se encontró ToDoItemDto.",
                        Data = null
                    };
                }

                _logger.LogInformation("Eliminando ToDoItemDto del repositorio.");
                await _repository.DeleteAsync(id, userId);

                return new GenericResponseDto<ToDoItemDto>
                {
                    IsSuccess = true,
                    Message = "ToDoItemDto eliminado exitosamente.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new GenericResponseDto<ToDoItemDto>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno al eliminar ToDoItemDto.",
                    Data = null
                };
                throw;
            }
        }

        /// <summary>
        /// Obtiene las metricas 
        /// </summary>
        public async Task<GenericResponseDto<ToDoItemMetricsDto>> GetMetricsAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Recuperando metricas desde el repositorio.");
                var metrics = await _repository.GetMetricsAsync(userId);

                if (metrics == null)
                {
                    _logger.LogWarning("No se encontró metricas desde el repositorio.");
                    return new GenericResponseDto<ToDoItemMetricsDto>
                    {
                        IsSuccess = false,
                        Message = "No se encontró metricas.",
                        Data = null
                    };
                }

                var toDoItemMetricsDto = _mapper.Map<ToDoItemMetricsDto>(metrics);
                return new GenericResponseDto<ToDoItemMetricsDto>
                {
                    IsSuccess = true,
                    Message = "Metricas recuperado exitosamente.",
                    Data = toDoItemMetricsDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al recuperar las metricas");
                return new GenericResponseDto<ToDoItemMetricsDto>
                {
                    IsSuccess = false,
                    Message = "Ocurrió un error interno en el servidor. Por favor, inténtelo de nuevo más tarde."
                };
                throw;
            }
        }
    }
}
