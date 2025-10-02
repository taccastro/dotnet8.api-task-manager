using GerenciadorTarefas.API.Middlewares;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Repositorios;
using GerenciadorTarefas.API.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Configuração do PostgreSQL
// ----------------------
builder.Services.AddDbContext<TarefaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BancoPostgreSQL")));

// ----------------------
// Configuração do Redis
// ----------------------
builder.Services.AddStackExchangeRedisCache(options =>
{
    var servidor = builder.Configuration["Redis:Servidor"];
    var porta = builder.Configuration["Redis:Porta"];
    options.Configuration = $"{servidor}:{porta}";
    options.InstanceName = "TarefasCache:";
});

// ----------------------
// Serviços internos
// ----------------------
builder.Services.AddScoped<ITarefaRepositorio, TarefaRepositorioPostgres>();
builder.Services.AddScoped<TarefaServico>();

// Configuração RabbitMQ
var rabbitMQHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
var rabbitMQPort = int.Parse(builder.Configuration["RabbitMQ:Porta"] ?? "5672");
var rabbitMQUsuario = builder.Configuration["RabbitMQ:Usuario"] ?? "guest";
var rabbitMQSenha = builder.Configuration["RabbitMQ:Senha"] ?? "guest";

builder.Services.AddSingleton<IRabbitMQPublisher>(provider => 
    new RabbitMQPublisher(rabbitMQHost, rabbitMQPort, rabbitMQUsuario, rabbitMQSenha));
//builder.Services.AddSingleton<AutenticacaoServico>(); PRA USAR REPO EM MEMO
builder.Services.AddScoped<AutenticacaoServico>();
builder.Services.AddSingleton<LogService>();



// ----------------------
// Controllers e Swagger
// ----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Gerenciador de Tarefas API",
        Version = "v1"
    });

    // Configuração para JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Digite 'Bearer' [espaço] e depois seu token JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// ----------------------
// Validação da chave JWT
// ----------------------
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException(
        "A chave JWT (Jwt:Key) deve ter no mínimo 32 caracteres (256 bits). " +
        $"Tamanho atual: {jwtKey?.Length ?? 0}"
    );
}

// ----------------------
// Configuração do JWT
// ----------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ----------------------
// Middleware
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de exceção **antes** dos endpoints
app.UseMiddleware<ExceptionMiddleware>();

app.UseMetricServer(); // expõe /metrics
app.UseHttpMetrics(); // coleta métricas HTTP (latência, status, etc.)

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

//var consumer = new RabbitMQConsumer();
//consumer.Consumir();

app.Run();
