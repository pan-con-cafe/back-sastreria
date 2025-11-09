//using sastreria_domain.Auth.repositories;
//using sastreria_domain.Auth.services;
using sastreria_domain.repositories;
using sastreria_domain.services;
//using sastreria_data.Auth.repositories;
//using sastreria_data.Auth.services;

// using sastreria_data.Auth.repositories;
// using sastreria_data.Auth.services;
using sastreria_data.repositories;
using sastreria_data.services;
using Microsoft.EntityFrameworkCore;
//using WebSastreria.Middlewares;

using Serilog;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebSastreria.utils;
using Microsoft.OpenApi.Models;
using System.Reflection;
using sastreria_data.database.tables;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// string connStr = builder.Configuration
//     .GetValue<string>("ConnectionStrings:IcLocalDb");

string connStr = builder.Configuration
    .GetValue<string>("ConnectionStrings:DefaultConnection");

builder.Services.AddDbContext<sastreria_data.database.tables._dbContext>(
    // Connect to SqlServer
    (config) => config.UseSqlServer(connStr, b => b.MigrationsAssembly("WebSastreria"))
    // connect to sqlite database
    // (config) => config.UseSqlite(
    //     builder.Configuration.GetConnectionString("IcLocalDb"),
    //     b => b.MigrationsAssembly("WebSastreria")
    // )
);


// CONFIGURACION DEL CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("origins", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod(); // GET, POST, PUT, DELETE, PATCH
        builder.AllowAnyHeader();
    });
});

// CONFIGURACIÓN DE SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese 'Bearer' [espacio] seguido del token JWT."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddScoped<AuthService>();

// LOGS
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File($"Logs/logs.txt")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


// Inyeccion de dependencias
// services
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddHttpClient<IReniecService, ReniecService>();


// repositories
builder.Services.AddScoped<IModeloRepository, ModeloRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
builder.Services.AddScoped<IDatoSastreriaRepository, DatoSastreriaRepository>();
builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();
builder.Services.AddScoped<ISastreRepository,SastreRepository>();
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<ICitaImagenRepository, CitaImagenRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();






builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
//}

app.UseHttpsRedirection();
app.UseCors("origins");



app.UseAuthentication(); // ¡Importante! Debe ir antes de UseAuthorization

app.UseAuthorization();

app.UseStaticFiles(); // Asegúrate de que esta línea esté ANTES de app.UseRouting(), si llegas a usarlo


app.MapControllers();

app.Run();






