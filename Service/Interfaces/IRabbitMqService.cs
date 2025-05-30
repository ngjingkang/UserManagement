namespace Service.Interfaces
{
    public interface IRabbitMqService
    {
        Task PublishAsync(string exchange, string routingKey, string message);
    }

}