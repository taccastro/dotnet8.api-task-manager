using GerenciadorTarefas.API.Middlewares;
using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Repositorios;
using GerenciadorTarefas.API.Servicos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Configura��o do banco
// ----------------------
builder.Services.AddDbContext<TarefaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BancoPostgreSQL")));

// ----------------------
// Configura��o do Redis
// ----------------------
var redisServidor = builder.Configuration.GetSection("Redis")["Servidor"];
var redisPorta = builder.Configuration.GetSection("Redis")["Porta"];
// Aqui voc� pode configurar servi�os Redis se precisar

// ----------------------
// Configura��o do RabbitMQ
// ----------------------
var rabbitHost = builder.Configuration.GetSection("RabbitMQ")["Host"];
var rabbitPorta = builder.Configuration.GetSection("RabbitMQ")["Porta"];
var rabbitUsuario = builder.Configuration.GetSection("RabbitMQ")["Usuario"];
var rabbitSenha = builder.Configuration.GetSection("RabbitMQ")["Senha"];
// Aqui voc� pode configurar servi�os RabbitMQ se precisar

// ----------------------
// Servi�os internos
// ----------------------
// Para usar PostgreSQL
builder.Services.AddScoped<ITarefaRepositorio, TarefaRepositorioPostgres>();

// OU para usar em mem�ria
/*builder.Services.AddSingleton<ITarefaRepositorio, TarefaRepositorioMemoria>(*/);


builder.Services.AddScoped<TarefaServico>();

// ----------------------
// Controllers e Swagger
// ----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----------------------
// Middleware
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();


app.Run();
