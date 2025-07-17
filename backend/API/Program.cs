using API.Configurations.InitialUsers;
using API.Configurations.Swagger;
using API.Data;
using API.Data.Logging;
using API.Mappings;
using API.Models;
using API.Models.Logging;
using API.Repositories.Interfaces;
using API.Repositories;
using API.Services.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<InitialUsersConfig>(builder.Configuration.GetSection("InitialUsers"));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<InitialUsersConfig>>().Value);

// Uso de Sqlite para facilitar evaluación de la prueba por su inicio rápido y sencillo
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<LogsDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("LogsConnection")));

// ***  Configurar logging
var connectionString = builder.Configuration.GetConnectionString("LogsConnection")!;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Sink(new CustomSQLiteSink(connectionString))
    .CreateLogger();

builder.Host.UseSerilog();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// *** Servicio de Identificación - ASP.NET CORE IDENTITY
builder.Services.AddIdentity<AppUser, IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configuraciones para el servicio de Identifiaciòn y autenticación con ASP.NET CORE IDENTITY
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configuración de la contraseña
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); 
    options.Lockout.MaxFailedAccessAttempts = 4; 
    options.Lockout.AllowedForNewUsers = true; 

    options.User.RequireUniqueEmail = true;
});


// *** Servicio de Autenticación

var JWTSettings = builder.Configuration.GetSection("JWTSetting");

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => {
    opt.SaveToken = true;
    opt.RequireHttpsMetadata = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = JWTSettings["ValidAudience"],
        ValidIssuer = JWTSettings["ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSettings.GetSection("securityKey").Value!))
    };
});

// *** Agrego Repositorios y Servicios
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddControllers();

//builder.Services.AddOpenApi();

// *** Servicios Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {

    // Configuro la información básica del documento Swagger
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Configuro cómo Swagger UI debe presentar el esquema de autenticación a la API y permite a los usuarios ingresar el token JWT en la interfaz de Swagger.
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization Example : 'Bearer eyeyeyeyeyye'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    // Documento qué esquemas de seguridad son necesarios para acceder a diferentes recursos de la API, proporcionando contexto en la documentación sobre los requisitos de seguridad.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
        new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id="Bearer"
            },
            Scheme="Bearer",
            Name="Bearer",
            In=ParameterLocation.Header,
        },
        new List<string>()
        }
    });

    c.EnableAnnotations();
    c.SchemaFilter<LoginDtoSwaggerSchemaFilter>();
    c.SchemaFilter<RegisterDtoSwaggerSchemaFilter>();
});

// ---------------------------------------------------------------------------------------------------------------------------------

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(options => {
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var initialUsersConfig = scope.ServiceProvider.GetRequiredService<InitialUsersConfig>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await AppDbInitializer.Initialize(userManager, roleManager, initialUsersConfig, logger);
}

app.Run();
