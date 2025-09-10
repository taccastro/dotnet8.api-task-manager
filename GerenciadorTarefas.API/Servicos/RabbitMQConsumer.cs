using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GerenciadorTarefas.API.Servicos
{
    public class RabbitMQConsumer : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConsumer(string hostName = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara fila
            _channel.QueueDeclare(
                queue: "tarefas",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public void Consumir()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensagem = Encoding.UTF8.GetString(body);
                Console.WriteLine($"📥 Mensagem recebida: {mensagem}");

                // Aqui você pode processar a mensagem e atualizar memória ou banco
            };

            _channel.BasicConsume(queue: "tarefas", autoAck: true, consumer: consumer);

            Console.WriteLine("🐇 Consumer iniciado, aguardando mensagens...");
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
