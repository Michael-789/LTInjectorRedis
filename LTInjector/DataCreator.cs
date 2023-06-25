using LTInjector.RabbitMQ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LTInjector
{
    internal class DataCreator
    {
        static readonly int x = 750;
        static readonly int y = 500;
        static readonly int z = 1000;

        static readonly int speed = 850;

        static int id = 1;

        //static RabbitMqSender rabbitMQHandler = new RabbitMqSender(Constants.RAW_FLIGHTS_EXCHANGE);
         RabbitMqSender rabbitMQSender = (RabbitMqSender)RabbitMqFactory.Instance.create(Constants.SENDER, Constants.RAW_FLIGHTS_EXCHANGE);


        public  void send2Rabbit()
        {
            
            int secondsCounter = 0;

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X))
            {

                List<byte[]> arrayList = createData();
                DateTime startTime = DateTime.Now;

                // Console.WriteLine("arrayList " + arrayList.Count);
                int listCount = 0;
                foreach (byte[] body in arrayList)
                {
                    listCount++;
                    rabbitMQSender.Send(body);

                }

                DateTime endTime = DateTime.Now;

                TimeSpan ts = (endTime - startTime);

                //Console.WriteLine("Elapsed Time is {0} ms", ts.TotalMilliseconds);

                if (ts.TotalMilliseconds < 1000)
                {
                    int ms = (int)Math.Round((1000 - ts.TotalMilliseconds), 0);
                    //Console.WriteLine("going to sleep for {0} milliseconds", ms);
                    Thread.Sleep(ms);
                }

                secondsCounter++;

                Console.WriteLine("Processed {0} during {1} seconds", listCount * secondsCounter, secondsCounter);



            }
        }

        private static List<byte[]> createData()
        {

            List<byte[]> arrayList = new List<byte[]>();



            for (int i = 0; i < Constants.MSG_PER_SEC; i++)
            {
                int newX = x, newY = y, newZ = z;
                JsonObject flightJson = new JsonObject();
                if ((id % 10) == 0)
                {
                    newZ = 1001;
                }
                else if ((id % 5) == 0)
                {
                    newX = 42;
                    newY = 42;
                }

                flightJson.Add("Id", id++);

                flightJson.Add("location", new JsonObject()
                    {
                        { "longitude", newX },
                        { "latitude", newY },
                        { "altitude", newZ }
                    });

                flightJson.Add("speed", speed);
                //Console.WriteLine(flightJson.ToString());

                byte[] body = Encoding.Default.GetBytes(flightJson.ToString());
                arrayList.Add(body);
            }

            return arrayList;


        }
    }
}
