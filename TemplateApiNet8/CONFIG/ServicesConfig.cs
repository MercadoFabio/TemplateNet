namespace $safeprojectname$.Config
{
    /// <summary>
    /// Clase de configuración de servicios.
    /// </summary>
    public static class ServicesConfig
    {
        /// <summary>
        /// Método para agregar la configuración de servicios.
        /// </summary>
        /// <param name="services">La colección de servicios.</param>
        /// <param name="configuration">La configuración.</param>
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            // Acá se agregan servicios de extensiones de paquetes nuget como por ejemplo MemoryCache, HttpContextAccesor, Cors, etc
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddSwagger();
            services.AddJwt();
            services.AddExternalServices();
            services.AddInternalServices();
            services.BindAppSettings(configuration);
            services.AddControllers();

            //Se agrega la config auto mapper.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Se agrega la config de  los contextos.
            services.AddDbContext<Context>(options =>
               options.UseInMemoryDatabase("InMemoryDb"));



            // TODO: Descomentar cuando se tenga la configuración de roles y permisos
            // Política de autorización por defecto
            /* services.AddAuthorizationBuilder()
                 .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build());*/

            //Se agrega la config de los servicios.
            services.AddConfiguration(configuration);
        }
        /// <summary>
        /// Método para agregar la configuración de JWT.
        /// </summary>
        /// <param name="services">La colección de servicios.</param>
        private static void AddJwt(this IServiceCollection services)
        {
            _ = services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppSettings.Jwt.Issuer,
                    ValidAudience = AppSettings.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.Jwt.SecretKey)),
                    ClockSkew = TimeSpan.Zero,
                };

                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        return context.Response.WriteAsync(JsonSerializer.Serialize(
                            OperationResponse<object>.CreateBuilder().WithCode(401)
                                .WithMessage(Utilities.MsjNotAuthenticate).Build()));
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(JsonSerializer.Serialize(OperationResponse<object>
                            .CreateBuilder().WithCode(401)
                            .WithMessage(Utilities.MsjUnauthorized).Build()));
                    },
                };
            });
        }

        /// <summary>
        /// Método para agregar los servicios externos.
        /// </summary>
        /// <param name="services">La colección de servicios.</param>
        private static void AddExternalServices(this IServiceCollection services)
        {
            // Acá se añade el ciclo de vida de los servicios externos
        }

        private static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            Sequence.Initialize(configuration);
        }

        private static void AddSwagger(this IServiceCollection services)
        {
            // Acá se añade la implementación del Swagger
            services.AddSwaggerGen(c =>
            {
                var projectName = Assembly.GetEntryAssembly()?.GetName().Name;
                c.SwaggerDoc("v1", new OpenApiInfo { Title = projectName, Version = "v1" });
                // Agrega la configuración de seguridad para el Bearer Token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ingrese el token de autenticación en el siguiente formato: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // Agrega la configuración de seguridad requerida para acceder a los endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }


        private static void AddInternalServices(this IServiceCollection services)
        {
            // Acá se añade el ciclo de vida de los servicios
            services.AddScoped<IFileService, FileService>();
        }


        private static void BindAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // Se bindean los datos del appsettings.json hacia la clase AppSettings
            var connection = new AppSettings.ConnectionString();
            var jwt = new AppSettings.Jwt();
            var sequences = new AppSettings.SequenceConfig();
            var amazon = new AppSettings.AmazonS3();
            var rutaS3 = new AppSettings.RutaS3();
            var smtp = new AppSettings.Smtp();

            configuration.Bind("ConnectionStrings", connection);
            configuration.Bind("JWT", jwt);
            configuration.Bind("Sequences", sequences);
            configuration.Bind("AmazonS3", amazon);
            configuration.Bind("RutasS3", rutaS3);
            configuration.Bind("Smtp", smtp);
        }
    }
}