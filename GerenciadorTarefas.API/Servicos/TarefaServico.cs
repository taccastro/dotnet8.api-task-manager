using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Repositorios;

namespace GerenciadorTarefas.API.Servicos
{
    public class TarefaServico
    {
        private readonly ITarefaRepositorio _repositorio;

        public TarefaServico(ITarefaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        // Listar todas
        public async Task<List<TarefaDto>> ListarAsync()
        {
            var tarefas = await _repositorio.ListarTodasTarefas();
            return tarefas.Select(t => new TarefaDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Categoria = t.Categoria,
                DataCriacao = t.DataCriacao
            }).ToList();
        }

        // Obter por Id
        public async Task<TarefaDto?> ObterPorIdAsync(Guid id)
        {
            var t = await _repositorio.ObterTarefaPorId(id);
            if (t == null) return null;

            return new TarefaDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Categoria = t.Categoria,
                DataCriacao = t.DataCriacao
            };
        }

        // Criar
        public async Task<TarefaDto> CriarAsync(TarefaCriarDto dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Categoria = dto.Categoria
            };

            await _repositorio.AdicionarTarefa(tarefa);

            return new TarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Categoria = tarefa.Categoria,
                DataCriacao = tarefa.DataCriacao
            };
        }

        // Atualizar
        public async Task<TarefaDto?> AtualizarAsync(Guid id, TarefaAtualizarDto dto)
        {
            var tarefa = await _repositorio.ObterTarefaPorId(id);
            if (tarefa == null) return null;

            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.Categoria = dto.Categoria;

            await _repositorio.AtualizarTarefa(tarefa);

            return new TarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Categoria = tarefa.Categoria,
                DataCriacao = tarefa.DataCriacao
            };
        }

        // Deletar
        public async Task<bool> DeletarAsync(Guid id)
        {
            var tarefa = await _repositorio.ObterTarefaPorId(id);
            if (tarefa == null) return false;

            await _repositorio.RemoverTarefa(id);
            return true;
        }
    }
}
