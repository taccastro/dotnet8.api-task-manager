using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GerenciadorTarefas.API.Modelos.Dados
{
    public static class SeedTarefas
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters = { new JsonStringEnumConverter() }
        };

        public static async Task PopularAsync(TarefaDbContext contexto, IDistributedCache cache)
        {
            // Busca tarefas existentes
            var tarefas = await contexto.Tarefas.ToListAsync();

            // Se não houver, adiciona seed inicial
            if (tarefas.Count == 0)
            {
                tarefas = new List<Tarefa>
                {
                    new Tarefa { Titulo = "Aprender Redis", Descricao = "Estudar cache distribuído com .NET", Categoria = "Estudo", DataCriacao = DateTime.UtcNow },
                    new Tarefa { Titulo = "Configurar Docker", Descricao = "Instalar e rodar Redis via Docker", Categoria = "Infra", DataCriacao = DateTime.UtcNow },
                    new Tarefa { Titulo = "Implementar Seed", Descricao = "Popular dados iniciais no banco", Categoria = "Desenvolvimento", DataCriacao = DateTime.UtcNow }
                };

                await contexto.Tarefas.AddRangeAsync(tarefas);
                await contexto.SaveChangesAsync();
            }

            // Atualiza cache Redis
            string cacheKeyAll = "tarefas:all";
            await cache.SetStringAsync(cacheKeyAll, JsonSerializer.Serialize(tarefas, _jsonOptions),
                new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            foreach (var tarefa in tarefas)
            {
                string cacheKey = $"tarefas:{tarefa.Id}";
                await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(tarefa, _jsonOptions),
                    new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            Console.WriteLine("✅ Seed de tarefas populada no PostgreSQL e Redis!");
        }
    }
}
