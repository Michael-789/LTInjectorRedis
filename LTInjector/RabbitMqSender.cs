using System.Text;
using RabbitMQ.Client;

namespace LTRuleEngine.RabbitMQ
{
    internal class RabbitMqSender : RabbitMqAbs
    {
        public RabbitMqSender(string exchangeName, string routingKey = "#") : base(exchangeName, routingKey)
        {
        }

        public void Send(string msg)
        {
            byte[] body = Encoding.Default.GetBytes(msg);

            Send(body);
        }

        public void Send(byte[] body)
        {

            Channel.BasicPublish(Exchange,
                RoutingKey,
                basicProperties: null,
                body: body);
        }
    }
}