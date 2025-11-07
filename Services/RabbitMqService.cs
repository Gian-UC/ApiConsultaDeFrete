using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EnvioRapidoApi.Services
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName = "fila_calculo_frete";

        public RabbitMqService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RabbitMQ__Host") ?? "rabbitmq"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void PublicarMensagem(object mensagem)
        {
            var json = JsonSerializer.Serialize(mensagem);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"📨 Mensagem enviada: {json}");
        }
    }
}
