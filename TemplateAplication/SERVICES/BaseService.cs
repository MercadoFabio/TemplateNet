namespace $safeprojectname$.Services
{
    public abstract class BaseService
    {
        protected readonly Context _context;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IMemoryCache _cache;
        protected static ConcurrentDictionary<string, bool> _cacheKeys = new();


        /// <summary>
        /// Constructor de la clase BaseService.
        /// </summary>
        protected BaseService() { }

        /// <summary>
        /// Constructor de la clase BaseService para cache.
        /// </summary>
        protected BaseService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public BaseService(Context contex, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _context = contex;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        /// <summary>
        /// Obtiene una lista de objetos DTO de la caché utilizando la clave especificada o los recupera de la base de datos y los almacena en caché si no están presentes.
        /// </summary>
        /// <typeparam name="TEntity">El tipo de entidad de base de datos.</typeparam>
        /// <typeparam name="TDto">El tipo de objeto DTO.</typeparam>
        /// <param name="query">La consulta IQueryable que representa la selección de datos.</param>
        /// <param name="cacheKey">La clave utilizada para identificar los datos en caché.</param>
        /// <returns>Una lista de objetos DTO obtenidos de la caché o de la base de datos.</returns>
        protected async Task<OperationResponse<List<TDto>>> FindByConditionAsyncLongCache<TEntity, TDto>(
            IQueryable<TEntity> query, string cacheKey)
            where TEntity : class
        {
            if (GetCache(cacheKey) is List<TDto> cachedData)
            {
                return Ok(cachedData);
            }
            else
            {
                var entityList = await query.AsNoTracking().ToListAsync();

                var entityDtoList = _mapper.Map<List<TDto>>(entityList);

                SetLongCache(cacheKey, entityDtoList);

                return Ok(entityDtoList);
            }
        }

        /// <summary>
        /// Crea una respuesta de operación con un código de estado 400 (BadRequest).
        /// </summary>
        /// <typeparam name="T">Tipo de datos del objeto.</typeparam>
        /// <param name="message">Mensaje de error a incluir en la respuesta.</param>
        /// <param name="data">Datos de la operación.</param>
        /// <returns>Una respuesta de operación con un código de estado 400 y el mensaje proporcionado.</returns>
        protected static OperationResponse<T> BadRequest<T>(string message, T? data = default)
            => OperationResponse<T>.CustomErrorResponse(400, message, data);


        /// <summary>
        /// Crea una respuesta de operación con un código de estado 200 (OK).
        /// </summary>
        /// <typeparam name="T">Tipo de datos del objeto.</typeparam>
        /// <param name="data">Datos a incluir en la respuesta.</param>
        /// <param name="total">La cantidad de filas afectadas.</param>
        /// <returns>Una respuesta de operación con un código de estado 200, los datos proporcionados y el mensaje proporcionado.</returns>
        protected static OperationResponse<T> Ok<T>(T data, int total = default)
            => OperationResponse<T>.SuccessResponse(data, total);


        /// <summary>
        /// Crea una respuesta de operación con un código de estado 404 (NotFound).
        /// </summary>
        /// <typeparam name="T">Tipo de datos del objeto.</typeparam>
        /// <returns>Una respuesta de operación con un código de estado 404.</returns>
        protected static OperationResponse<T> NotFound<T>()
            => OperationResponse<T>.NotFoundResponse();


        /// <summary>
        /// Crea una respuesta de operación con un código de estado 500 (InternalServerError).
        /// </summary>
        /// <typeparam name="T">Tipo de datos del objeto.</typeparam>
        /// <param name="exception">Información de la excepción a incluir en la respuesta.</param>
        /// <returns>Una respuesta de operación con un código de estado 500, el mensaje proporcionado y la información de la excepción proporcionada.</returns>
        protected static OperationResponse<T> ServerErrorFile<T>(string exception)
            => OperationResponse<T>.ErrorFileResponse(exception);

        /// <summary>
        /// Obtiene un objeto de la caché utilizando la clave especificada.
        /// </summary>
        /// <param name="key">Clave de la caché.</param>
        /// <returns>El objeto almacenado en la caché.</returns>
        protected object GetCache(string key)
        {
            var data = _cache.Get(key);
            return data;
        }


        /// <summary>
        /// Almacena un objeto en la caché utilizando la clave y los datos especificados.
        /// </summary>
        /// <typeparam name="T">Tipo de datos del objeto.</typeparam>
        /// <param name="key">Clave de la caché.</param>
        /// <param name="data">Datos a almacenar en la caché.</param>
        protected void SetCache<T>(string key, T data) => _cache?.Set(key, data, DateTime.Now.AddMinutes(2));

        /// <summary>
        /// Almacena un objeto en la caché utilizando la clave y los datos especificados durante un largo período de tiempo.
        /// </summary>
        /// <typeparam name="T">Tipo de datos del objeto.</typeparam>
        /// <param name="key">Clave de la caché.</param>
        /// <param name="data">Datos a almacenar en la caché.</param>
        protected void SetLongCache<T>(string key, T data)
        {
            _cache?.Set(key, data, DateTime.Now.AddDays(4));
            _cacheKeys.TryAdd(key, true);
        }

        /// <summary>
        /// Obtiene todas las claves que se encuentran actualmente en la caché.
        /// </summary>
        /// <returns>Una colección de todas las claves en la caché.</returns>
        protected static IEnumerable<string> GetAllKeysCache() => _cacheKeys.Keys;

        /// <summary>
        /// Elimina todas las entradas de la caché utilizando las claves almacenadas.
        /// </summary>
        /// <returns>Verdadero si todas las entradas fueron eliminadas con éxito y la caché está vacía, falso en caso contrario.</returns>
        protected bool RemoveAllKeysCache()
        {
            foreach (var key in _cacheKeys)
            {
                RemoveKeyCache(key.Key);
            }

            _cacheKeys.Clear();
            return _cacheKeys.IsEmpty;
        }

        /// <summary>
        /// Elimina un objeto de la caché utilizando la clave especificada.
        /// </summary>
        /// <param name="key">Clave de la caché.</param>
        /// <returns>Retorna un valor booleano segun el resultado.</returns>
        protected bool RemoveKeyCache(string key)
        {
            var data = GetCache(key);
            if (data == null)
            {
                return false;
            }

            _cache?.Remove(key);
            _cacheKeys.TryRemove(key, out _);
            return true;
        }


        /// <summary>
        /// Obtiene los datos paginados de forma asíncrona para una entidad específica.
        /// </summary>
        /// <typeparam name="TEntity">El tipo de entidad de base de datos.</typeparam>
        /// <typeparam name="TDto">El tipo de objeto DTO.</typeparam>
        /// <param name="page">El número de página.</param>
        /// <param name="pageSize">El tamaño de la página.</param>
        /// <param name="query">La consulta IQueryable que representa la selección de datos.</param>
        /// <returns>Una respuesta de operación que contiene los datos paginados y el total de registros.</returns>
        protected async Task<OperationResponse<List<TDto>>> GetPagedDataAsync<TEntity, TDto>(int page, int pageSize,
            IQueryable<TEntity> query)
        {
            var total = await query.CountAsync();

            if (total == 0)
            {
                return NotFound<List<TDto>>();
            }

            var entitiesFilter = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var dtos = _mapper.Map<List<TDto>>(entitiesFilter);

            return Ok(dtos, total);
        }
        /// <summary>
        /// Obtiene los datos del usuario logueado.
        /// </summary>
        /// <returns>Devuelve el dni del usuario logueado.</returns>
        private string GetUserCache()
        {
            //TODO: Sacar el dato que necesitamos de la token modificar User por la propiedad de la token que vamos a usar.
            var user = _httpContextAccessor?.HttpContext?.User;
            return user?.Claims.FirstOrDefault(c => c.Type == "User")?.Value;
        }
        /// <summary>
        /// Inserta de forma asíncrona un objeto en la base de datos.
        /// </summary>
        /// <typeparam name="TEntity">El tipo de entidad de base de datos.</typeparam>
        /// <typeparam name="TDto">El tipo de objeto DTO.</typeparam>
        /// <param name="dto">El objeto DTO a insertar.</param>
        /// <param name="context">El contexto de la base de datos.</param>
        /// <param name="userIng">El  usuario que realizo el cambio.</param>
        /// <returns>Una respuesta de operación que contiene el objeto DTO insertado.</returns>
        protected async Task<OperationResponse<TDto>> InsertAsync<TEntity, TDto>(TDto dto, DbContext context)
            where TEntity : class
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            var entity = _mapper.Map<TEntity>(dto);
            if (entity is IEntityAuditable auditableEntity)
            {
                //TODO: Descomentar si vamos a trabajar con sequencias.
                //auditableEntity.Id = Sequence.GetSequenceForType<TEntity>();
                auditableEntity.UserIng = GetUserCache();
                auditableEntity.FecIng = DateTime.Now;
            }

            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(_mapper.Map<TDto>(entity));
        }

        /// <summary>
        /// Actualiza una entidad en la base de datos de forma asíncrona.
        /// </summary>
        /// <typeparam name="TEntity">El tipo de entidad de base de datos.</typeparam>
        /// <typeparam name="TDto">El tipo de objeto DTO.</typeparam>
        /// <param name="entity">La entidad que contiene los datos actualizados.</param>
        /// <param name="context">El contexto de la base de datos.</param>
        /// <param name="userMod">El usuario que realizao la modificación.</param>
        /// <returns>Una respuesta de operación que indica el resultado de la actualización.</returns>
        protected async Task<OperationResponse<TDto>> UpdateAsync<TEntity, TDto>(TEntity entity, DbContext context)
            where TEntity : class
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            if (entity is IEntityAuditable auditableEntity)
            {
                auditableEntity.UserMod = GetUserCache();
                auditableEntity.FecMod = DateTime.Now;
            }

            context.Set<TEntity>().Update(entity);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(_mapper.Map<TDto>(entity));
        }

        /// <summary>
        /// Realiza una baja logica a una entidad en la base de datos de forma asíncrona.
        /// </summary>
        /// <typeparam name="TEntity">El tipo de entidad de base de datos.</typeparam>
        /// <param name="entity">La entidad que va ser dada de baja.</param>
        /// <param name="context">El contexto de la base de datos.</param>
        /// <param name="motivoBaja">Motivo porque esta dando la baja de la entidad.</param>
        /// <param name="userBaja">Usuario que realizoi la baja.</param>
        /// <returns>Una respuesta de operación que indica el resultado de la actualización.</returns>
        protected async Task<OperationResponse<bool>> DeleteAsync<TEntity>(TEntity entity, DbContext context)
            //string? motivoBaja)
            where TEntity : class
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            context.Set<TEntity>().Update(entity);
            if (entity is IEntityAuditable auditableEntity)
            {
                auditableEntity.UserBaja = GetUserCache();
                auditableEntity.FecBaja = DateTime.Now;
                // TODO: Descomentar si se va a trabajar con motivos de baja.
                //auditableEntity.MotivoBaja = string.IsNullOrEmpty(motivoBaja) ? motivoBaja : "Baja por el usuario";
            }

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(true);
        }
    }
}