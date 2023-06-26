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
        //static readonly double x = 0.5;
        //static readonly double y = 1.5;
        static readonly double z = 1000;

        static readonly int speed = 850;

        static int id = 1;

        List<Point> intPoints;
        List<Point> extPoints;


        RabbitMqSender rabbitMQSender = (RabbitMqSender)RabbitMqFactory.Instance.getInstance(Constants.SENDER, Constants.RAW_FLIGHTS_EXCHANGE);


        public DataCreator()
        {
            intPoints = createIntPoints();
            extPoints = createExtPoints();
        }
        public void send2Rabbit()
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

        private  List<byte[]> createData()
        {

            List<byte[]> arrayList = new List<byte[]>();


            int intPointsIndex = 0;
            int outPointsIndex = 0;
            for (int i = 0; i < Constants.MSG_PER_SEC; i++)
            {
                double newX = 0;
                double newY = 0;
                double newZ = z;


                JsonObject flightJson = new JsonObject();
                if ((id % 15) == 0)
                {
                    newZ = 1001;
                }
                else if ((id % 10) == 0)
                {
                    newX = 42;
                    newY = 42;
                }
                else if ((id % 5) == 0)
                {
                    
                    Point p = intPoints[intPointsIndex];
                    newX = p.X;
                    newY = p.Y;

                    if (++intPointsIndex >= intPoints.Count )
                    {
                        intPointsIndex = 0;
                    }
                }
                else
                {
                  
                    Point p = extPoints[outPointsIndex];
                    newX = p.X;
                    newY = p.Y;
                    if (++outPointsIndex >= extPoints.Count)
                    {
                        outPointsIndex = 0;
                    }
                }


                flightJson.Add("Id", id++);
                flightJson.Add("location", new JsonObject()
                  {   {"type", "Point"},
                      { "coordinates", new JsonArray() { newX, newY } },

                  });
                flightJson.Add("altitude", newZ);

                flightJson.Add("speed", speed);
                Console.WriteLine(flightJson.ToString());

                byte[] body = Encoding.Default.GetBytes(flightJson.ToString());
                arrayList.Add(body);
            }

            return arrayList;


        }

        private List<Point> createIntPoints()
        {
            List<Point> points = new List<Point>();

            points.Add(new Point(1.5, 1.5));
            points.Add(new Point(1.5, 2));
            points.Add(new Point(1.5, 1));

            return points;

        }

        private List<Point> createExtPoints()
        {
            List<Point> points = new List<Point>();

            points.Add(new Point(0.5, 1.5));
            points.Add(new Point(1.5, 2.5));
            points.Add(new Point(1.5, 3));
            points.Add(new Point(1.5, 0.5));

            return points;
        }
    }
}
