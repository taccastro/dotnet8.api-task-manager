using RabbitMQ.Client;
using System.Text;

namespace GerenciadorTarefas.API.Servicos
{
    public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;

        public RabbitMQPublisher(string hostName = "localhost", int porta = 5672, string usuario = "guest", string senha = "guest")
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = hostName,
                Port = porta,
                UserName = usuario,
                Password = senha
            };

            // Conexão e canal síncronos (funciona no .NET 8)
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara a fila no construtor
            _channel.QueueDeclare(
                queue: "tarefas",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public void Publish(string mensagem)
        {
            var body = Encoding.UTF8.GetBytes(mensagem);

            _channel.BasicPublish(
                exchange: "",
                routingKey: "tarefas",
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"✅ Mensagem enviada: {mensagem}");
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}