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
builder.Services.AddSingleton<ITarefaRepositorio, TarefaRepositorioMemoria>();
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

// ----------------------
// Teste r�pido do banco
// ----------------------
//using var scope = app.Services.CreateScope();
//var db = scope.ServiceProvider.GetRequiredService<TarefaDbContext>();

//try
//{
//    db.Tarefas.Add(new Tarefa
//    {
//        Titulo = "Teste inicial",
//        Descricao = "Primeira tarefa",
//        Categoria = "Geral" 
//    });

//    await db.SaveChangesAsync();
//    Console.WriteLine("Tarefa salva com sucesso!");
//}
//catch (DbUpdateException ex)
//{
//    Console.WriteLine($"Erro ao salvar a tarefa: {ex.Message}");
//    if (ex.InnerException != null)
//        Console.WriteLine($"Detalhe interno: {ex.InnerException.Message}");
}

var count = await db.Tarefas.CountAsync();
    Console.WriteLine($"Total de tarefas no banco: {count}");


// ----------------------
// Rodar aplica��o
// ----------------------
app.Run();
