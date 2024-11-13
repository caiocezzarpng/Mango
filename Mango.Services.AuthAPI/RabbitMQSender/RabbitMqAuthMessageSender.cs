using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public class RabbitMqAuthMessageSender : IRabbitMqAuthMessageSender
    {
        private readonly string _hostName = "localhost";
        private readonly string _username = "guest";
        private readonly string _password = "guest";
        private IConnection _connection;

        public async void SendMessage(object message, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password,
            };

            _connection = await factory.CreateConnectionAsync();

            await using var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queueName);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
        }
    }
}