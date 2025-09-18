using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefas.API.Modelos.Dados
{
    public class TarefaAtualizarDto
    {
        [Required]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        public int Prioridade { get; set; } = 0;
    }
}
