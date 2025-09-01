namespace GerenciadorTarefas.API.Modelos.Dados
{
    public class TarefaDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}
