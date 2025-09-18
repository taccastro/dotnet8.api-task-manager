using GerenciadorTarefas.API.Modelos.Dados;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GerenciadorTarefas.API.Repositorios
{
    public class TarefaRepositorioPostgres : ITarefaRepositorio
    {
        private readonly TarefaDbContext _contexto;
        private readonly IDistributedCache _cache;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters = { new JsonStringEnumConverter() }
        };

        public TarefaRepositorioPostgres(TarefaDbContext contexto, IDistributedCache cache)
        {
            Console.WriteLine(" Usando TarefaRepositorioPostgres com Redis");
            _contexto = contexto;
            _cache = cache;
        }

        // --- LISTAR TODAS ---
        public Task<List<Tarefa>> ListarTodasTarefas()
        {
            // Chama a versão com forçar refresh como false
            return ListarTodasTarefas(forcarRefresh: false);
        }

        public async Task<List<Tarefa>> ListarTodasTarefas(bool forcarRefresh = false)
        {
            string cacheKey = "tarefas:all";

            if (!forcarRefresh)
            {
                var cached = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cached))
                {
                    Console.WriteLine(" Retornando tarefas do cache");
                    return JsonSerializer.Deserialize<List<Tarefa>>(cached, _jsonOptions)!;
                }
            }

            // BREAKPOINT garantido aqui
            Console.WriteLine(" Buscando tarefas no banco...");
            var tarefas = await _contexto.Tarefas.ToListAsync();

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(tarefas, _jsonOptions),
                new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return tarefas;
        }

        // --- BUSCAR POR CATEGORIA OU PRIORIDADE ---
        public async Task<List<Tarefa>> BuscarTarefas(string? categoria = null, int? prioridade = null, int pagina = 1, int tamanhoPagina = 20)
        {
            IQueryable<Tarefa> query = _contexto.Tarefas;

            if (!string.IsNullOrEmpty(categoria))
                query = query.Where(t => t.Categoria.ToLower() == categoria.ToLower());

            if (prioridade.HasValue)
                query = query.Where(t => t.Prioridade == prioridade.Value);

            query = query.OrderByDescending(t => t.Prioridade)
                         .ThenBy(t => t.DataCriacao)
                         .Skip((pagina - 1) * tamanhoPagina)
                         .Take(tamanhoPagina);

            return await query.ToListAsync();
        }

        // --- OBTER POR ID ---
        public async Task<Tarefa?> ObterTarefaPorId(Guid id)
        {
            string cacheKey = $"tarefas:{id}";

            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                Console.WriteLine($" Retornando tarefa {id} do cache");
                return JsonSerializer.Deserialize<Tarefa>(cached, _jsonOptions)!;
            }

            var tarefa = await _contexto.Tarefas.FindAsync(id);
            if (tarefa != null)
            {
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(tarefa, _jsonOptions),
                    new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            return tarefa;
        }

        // --- ADICIONAR ---
        public async Task AdicionarTarefa(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            tarefa.DataCriacao = DateTime.UtcNow;

            await _contexto.Tarefas.AddAsync(tarefa);
            await _contexto.SaveChangesAsync();

            // Invalida cache
            await _cache.RemoveAsync("tarefas:all");
            Console.WriteLine(" Cache tarefas:all removido após adicionar");
        }

        // --- ATUALIZAR ---
        public async Task AtualizarTarefa(Tarefa tarefa)
        {
            _contexto.Tarefas.Update(tarefa);
            await _contexto.SaveChangesAsync();

            await _cache.RemoveAsync("tarefas:all");
            await _cache.RemoveAsync($"tarefas:{tarefa.Id}");
            Console.WriteLine($" Cache removido para tarefas:all e tarefas:{tarefa.Id}");
        }

        // --- REMOVER ---
        public async Task RemoverTarefa(Guid id)
        {
            var tarefa = await _contexto.Tarefas.FindAsync(id);
            if (tarefa != null)
            {
                _contexto.Tarefas.Remove(tarefa);
                await _contexto.SaveChangesAsync();

                await _cache.RemoveAsync("tarefas:all");
                await _cache.RemoveAsync($"tarefas:{id}");
                Console.WriteLine($" Tarefa {id} removida e cache invalidado");
            }
        }
    }
}
