using GerenciadorTarefas.API.Middlewares;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Repositorios;
using GerenciadorTarefas.API.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

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

// ----------------------
// Seed de tarefas (popula PostgreSQL e Redis)
// ----------------------
using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<TarefaDbContext>();
    var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
    await SeedTarefas.PopularAsync(contexto, cache);
}

app.Run();
