namespace GerenciadorTarefas.API.Servicos
{
    public interface IRabbitMQPublisher
    {
        void Publish(string message);
    }
}
