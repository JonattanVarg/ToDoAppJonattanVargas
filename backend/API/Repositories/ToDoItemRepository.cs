namespace API.Repositories
{
    #region Namespaces
    using API.Data;
    using API.Dtos;
    using API.Models;
    using API.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;
    #endregion

    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ToDoItemRepository> _logger;

        public ToDoItemRepository(AppDbContext context, ILogger<ToDoItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene un ToDoItem por su ID y por el userId.
        /// </summary>
        /// <returns>El ToDoItem si se encuentra, de lo contrario, null.</returns>
        public async Task<ToDoItem> GetByIdAsync(int id, string userId)
        {
            try
            {
                _logger.LogInformation("Recuperando el ToDoItem con ID: {Id} y userId: {userId} desde la base de datos.", id, userId);

                var toDoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (toDoItem == null) _logger.LogWarning("No se encontró ToDoItem con ID: {Id}  y userId: {userId}.", id, userId);

                return toDoItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al recuperar ToDoItem con ID: {Id}  y userId: {userId}.", id, userId);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas los ToDoItem de un usuario con el userId
        /// </summary>
        /// <returns>Una lista de todos los ToDoItem de un usuario</returns>
        public async Task<IEnumerable<ToDoItem>> GetAllAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Recuperando todas los ToDoItem  de un usuario desde la base de datos.");
                return await _context.ToDoItems
               .Where(t => t.UserId == userId)
               .OrderByDescending(t => t.CreatedDate)
               .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al recuperar todas los ToDoItem de un usuario.");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas los ToDoItem de un usuario con el userId y su estado de completado o no completado
        /// </summary>
        /// <param name="userId">El id del usuario</param>
        /// <param name="isCompleted">true o false dependiendo del estado que se busque </param>
        /// <returns>Una lista de todos los ToDoItem de un usuario dependiendo del estado completado o no completado</returns>
        public async Task<IEnumerable<ToDoItem>> GetByCompletionStatusAsync(string userId, bool isCompleted)
        {
            try
            {
                _logger.LogInformation("Recuperando todas los ToDoItem  de un usuario dependiendo del estado desde la base de datos");
                return await _context.ToDoItems
                .Where(t => t.UserId == userId && t.IsCompleted == isCompleted)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al recuperar todas los ToDoItem de un usuario dependiendo del estado.");
                throw;
            }
        }

        /// <summary>
        /// Agrega un ToDoItem a la base de datos
        /// </summary>
        /// <param name="ToDoItem">El objeto <see cref="ToDoItem"/> que se va a agregar.</param>
        public async Task<ToDoItem> AddAsync(ToDoItem item)
        {
            try
            {
                _logger.LogInformation("Agregando un ToDoItem.");
                _context.ToDoItems.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("ToDoItem agregado exitosamente con ID: {Id}.", item.Id);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al agregar un ToDoItem.");
                throw;
            }
        }

        /// <summary>
        /// Actualiza un ToDoItem a la base de datos
        /// </summary>
        /// <param name="ToDoItem">El objeto <see cref="ToDoItem"/> que se va a actualizar.</param>
        public async Task UpdateAsync(ToDoItem item)
        {
            try
            {
                _logger.LogInformation("Actualizando un ToDoItem.");
                _context.ToDoItems.Update(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("ToDoItem actualizado exitosamente con ID: {Id}.", item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al actualizar un ToDoItem.");
                throw;
            }
        }

        /// <summary>
        /// Elimina un ToDoItem de la base de datos
        /// </summary>
        /// <param name="ToDoItem">El objeto <see cref="ToDoItem"/> que se va a eliminar.</param>
        public async Task DeleteAsync(int id, string userId)
        {
            try
            {
                _logger.LogInformation("Buscando el ToDoItem con ID: {Id} y userId: {userId} para eliminarlo.", id, userId);
                var item = await GetByIdAsync(id, userId);
                if (item != null)
                {
                    _context.ToDoItems.Remove(item);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("ToDoItem con ID: {Id} y userId: {userId} eliminado exitosamente.", id, userId);
                }
                else
                {
                    _logger.LogWarning("No se encontró ToDoItem con ID: {Id} y userId: {userId} para eliminar.", id, userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al eliminar ToDoItem con ID: {Id} y userId: {userId}.", id, userId);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las metricas 
        /// </summary>
        public async Task<ToDoItemMetricsDto> GetMetricsAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Recuperando metricas de un usuario desde la base de datos.");
                var items = await _context.ToDoItems
               .Where(t => t.UserId == userId)
               .ToListAsync();

                return new ToDoItemMetricsDto
                {
                    TotalTasks = items.Count,
                    CompletedTasks = items.Count(t => t.IsCompleted),
                    PendingTasks = items.Count(t => !t.IsCompleted)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al recuperar todas las metricas de un usuario.");
                throw;
            }
        }
    }
}
