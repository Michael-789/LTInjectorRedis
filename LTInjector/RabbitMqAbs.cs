using RabbitMQ.Client;

namespace LTRuleEngine.RabbitMQ
{
    public abstract class RabbitMqAbs
    {
        internal readonly IModel Channel;
        internal readonly string Exchange;
        internal readonly string RoutingKey;

        protected RabbitMqAbs(string exchange, string routingKey = "#")
        {
            Exchange = exchange;
            RoutingKey = routingKey;

            var factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            var connection = factory.CreateConnection();
            Channel = connection.CreateModel();

            Channel.ExchangeDeclare(exchange, type: ExchangeType.Topic);
        }
    }
}