using System.Reflection;
using System.Text;
using LTInjector.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LTInjector.RabbitMQ
{

    public class RabbitMqReceiver : RabbitMqAbs
    {
        private static RabbitMqReceiver instance;

        public static RabbitMqReceiver getInstance(string exchangeName, string routingKey = "#")
        {
       
                if (instance == null)
                {
                    instance = new RabbitMqReceiver(exchangeName, routingKey);
                }
                return instance;
            
        }
        private string Queue { get; set; }

        private RabbitMqReceiver(string exchangeName, string routingKey = "#") : base(exchangeName, routingKey)
        {
            var queueDeclareOk = Channel.QueueDeclare(string.Empty, exclusive: true, autoDelete: true);
            var generatedQueueName = queueDeclareOk.QueueName;
            Channel.QueueBind(generatedQueueName, exchangeName, routingKey);
            Queue = generatedQueueName;
        }

        public void Receive<T>(Action<T> callback)
        {
            var consumer = new EventingBasicConsumer(Channel);
            Console.WriteLine(Assembly.GetEntryAssembly().GetName().Name +
                              " start and waiting for messages from exchange " + Exchange);
            consumer.Received += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                if (Exchange != ea.Exchange) return;
                Type typeParameterType = typeof(T);
                string strObj = Encoding.UTF8.GetString(body);
                T obj;
                if (typeof(T) != typeof(string))
                { 
                    obj = JsonConvert.DeserializeObject<T>(strObj);
                }
                else
                {
                    obj = (T)(object)strObj;
                }
                if (obj != null) callback(obj);
            };

            Channel.BasicConsume(queue: Queue,
                autoAck: false,
                consumer: consumer);
        }
    }
}