namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public interface IRabbitMqAuthMessageSender
    {
        void SendMessage(Object message, string queueName);
    }
}