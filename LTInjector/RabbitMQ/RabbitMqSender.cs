using System.Text;
using LTInjector.RabbitMQ;
using RabbitMQ.Client;

namespace LTInjector.RabbitMQ
{
    internal class RabbitMqSender : RabbitMqAbs
    {
        private static RabbitMqSender instance;

        public static RabbitMqSender getInstance(string exchangeName, string routingKey = "#")
        {

            if (instance == null)
            {
                instance = new RabbitMqSender(exchangeName, routingKey);
            }
            return instance;

        }
        private RabbitMqSender(string exchangeName, string routingKey = "#") : base(exchangeName, routingKey)
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