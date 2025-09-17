namespace GerenciadorTarefas.API.Modelos.Dados
{
    public class LogEvento
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Acao { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public string Detalhes { get; set; } = string.Empty;
    }
}
