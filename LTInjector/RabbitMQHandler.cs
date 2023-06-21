using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.Metrics;
using System.Text;

namespace EltaParser
{
    internal class RabbitMQHandler
    {

        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        int counter = 0;
        // string exchange;

        public RabbitMQHandler()
        {
            factory = new ConnectionFactory { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            //this.exchange = exchange;
           // channel.ExchangeDeclare(exchange, type: ExchangeType.Topic);
        }
        public  void Send(string exchange,string routingKey, byte[] body)
        {
            channel.ExchangeDeclare(exchange, type: ExchangeType.Topic);
        

            channel.BasicPublish(exchange,
                     routingKey,
                     basicProperties: null,
                     body: body);

  
           // string mystr = Encoding.UTF8.GetString(body);
          // Console.WriteLine($" [x] Sent {mystr}");
        
        }

        public void Receive(string[] queues, string[] exchanges, string routingKey)
        {

            counter = 0;

            List<string> queueNamesList = new List<string>();

            for (int i = 0; i < queues.Length; i++)
            {
                channel.ExchangeDeclare(exchanges[i], type: ExchangeType.Topic);

                var queueName = channel.QueueDeclare(queues[i]);

                queueNamesList.Add(queueName);

                channel.QueueBind(queueName, exchanges[i], routingKey);
            }


           // Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var data = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                if ("alerts" == ea.Exchange)
                {
                    Console.WriteLine("ALERT!! '{0}", data);
                    Console.WriteLine("recieved {0} alerts ", ++counter);
                }


            };

            foreach (string queueName in queueNamesList)
            {
                channel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer);
            }



        }
       
  


    }
}
