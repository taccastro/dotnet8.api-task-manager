namespace GerenciadorTarefas.API.Modelos
{
    public class Tarefa
    {
        public Guid Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public required string Categoria { get; set; }
        public int Prioridade { get; set; } = 1; // 1 = baixa, 5 = alta
        public bool Concluida { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
