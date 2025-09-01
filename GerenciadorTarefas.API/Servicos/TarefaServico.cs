using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorTarefas.API.Servicos
{
    public class TarefaServico
    {
        private readonly TarefaDbContext _db;

        public TarefaServico(TarefaDbContext db)
        {
            _db = db;
        }

        // Listar todas
        public async Task<List<TarefaDto>> ListarAsync()
        {
            return await _db.Tarefas
                .Select(t => new TarefaDto
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    Categoria = t.Categoria,
                    CriadaEm = t.DataCriacao
                }).ToListAsync();
        }

        // Obter por Id
        public async Task<TarefaDto?> ObterPorIdAsync(Guid id)
        {
            var t = await _db.Tarefas.FindAsync(id);
            if (t == null) return null;
            return new TarefaDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Categoria = t.Categoria,
                CriadaEm = t.DataCriacao
            };
        }

        // Criar
        public async Task<TarefaDto> CriarAsync(TarefaCriarDto dto)
        {
            var tarefa = new Tarefa
            {
                Id = Guid.NewGuid(),
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Categoria = dto.Categoria,
                DataCriacao = DateTime.UtcNow
            };

            _db.Tarefas.Add(tarefa);
            await _db.SaveChangesAsync();

            return new TarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Categoria = tarefa.Categoria,
                CriadaEm = tarefa.DataCriacao
            };
        }

        // Atualizar
        public async Task<TarefaDto?> AtualizarAsync(Guid id, TarefaAtualizarDto dto)
        {
            var tarefa = await _db.Tarefas.FindAsync(id);
            if (tarefa == null) return null;

            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.Categoria = dto.Categoria;

            await _db.SaveChangesAsync();

            return new TarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Categoria = tarefa.Categoria,
                CriadaEm = tarefa.DataCriacao
            };
        }

        // Deletar
        public async Task<bool> DeletarAsync(Guid id)
        {
            var tarefa = await _db.Tarefas.FindAsync(id);
            if (tarefa == null) return false;

            _db.Tarefas.Remove(tarefa);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
