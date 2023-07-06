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
       // static  double x = 100;
        //static  double y = 100;
        static readonly double z = 1000;

        static readonly int speed = 850;

        static int id = 1;
        
        static int trackId = 1;

        private static double xAdv = (Constants.XEND - Constants.XSTART) / Constants.TRACKPERIOD;
        private static double yAdv = (Constants.YEND - Constants.YSTART) /Constants.TRACKPERIOD;
        
        private static double x = Constants.XSTART;
        private static double y = Constants.YSTART;


        static int alertscounter = 0;

        /*List<Point> intPoints;
        List<Point> extPoints;*/

        ReddisHandler reddis;

        // RabbitMqSender rabbitMQSender = (RabbitMqSender)RabbitMqFactory.Instance.getInstance(Constants.SENDER, Constants.RAW_FLIGHTS_EXCHANGE);


        public DataCreator()
        {
            //intPoints = createIntPoints();
           // extPoints = createExtPoints();
            reddis = new ReddisHandler();
        }
   /*     public void send2Rabbit()
        {
            
            int secondsCounter = 0;

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X))
            {

                List<string> arrayList = createData();
                DateTime startTime = DateTime.Now;


                // Console.WriteLine("arrayList " + arrayList.Count);
                int listCount = 0;
                foreach (byte[] body in arrayList)
                {
                    listCount++;
                    //rabbitMQSender.Send(body);

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

                Console.WriteLine("Processed {0} during {1} seconds, expected alerts {2}", listCount * secondsCounter, secondsCounter,alertscounter);



            }
        }*/
        public void send2Reddis()
        {

            int secondsCounter = 0;

            reddis.connect();

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X))
            {

                List<string> arrayList = createData();
                DateTime startTime = DateTime.Now;


                // Console.WriteLine("arrayList " + arrayList.Count);
                int listCount = 0;
                foreach(string body in arrayList)
                {
                    listCount++;
                    Console.WriteLine(body);
                    reddis.send2Set("flightList",body);
                    //reddis.push2List("flightList",body);
                   // rabbitMQSender.Send(body);

                }

                DateTime endTime = DateTime.Now;

                TimeSpan ts = (endTime - startTime);

                Console.WriteLine("Elapsed Time is {0} ms", ts.TotalMilliseconds);

                if (ts.TotalMilliseconds < 1000)
                {
                    int ms = (int)Math.Round((1000 - ts.TotalMilliseconds), 0);
                    Console.WriteLine("going to sleep for {0} milliseconds", ms);
                    Thread.Sleep(ms);
                }

                secondsCounter++;

                Console.WriteLine("Processed {0} during {1} seconds, expected alerts {2}", listCount * secondsCounter, secondsCounter, alertscounter);



            }
        }

        private  List<string> createData()
        {

            List<string> arrayList = new List<string>();


          for (int i = 0; i < Constants.MSG_PER_SEC; i++)
            {
               // double newX = 0;
               // double newY = 0;
                double newZ = z;


                JsonObject flightJson = new JsonObject();
                if ((id % 15) == 0)
                {
                    newZ = 1001;
                    alertscounter++;
                }
                /*else if ((id % 10) == 0)
                {
                    //newX = 42;
                   // newY = 42;
                    alertscounter++;

                }
                else #1#*/
              
                    
                    //Point p = intPoints[intPointsIndex];
                    //newX = p.X;
                    //newY = p.Y;

                    /*if (++intPointsIndex >= intPoints.Count )
                    {
                        intPointsIndex = 0;
                    }*/
                 
                    alertscounter++;

                
               // else
               // {
                  
                    /*
                    Point p = extPoints[outPointsIndex];
                    newX = p.X;
                    newY = p.Y;
                    if (++outPointsIndex >= extPoints.Count)
                    {
                        outPointsIndex = 0;
                    }*/
                //}


                flightJson.Add("Id", id++);
                flightJson.Add("trackId",trackId);
                flightJson.Add("location", new JsonObject()
                  {   {"type", "Point"},
                      { "coordinates", new JsonArray() { x, y } },

                  });
                flightJson.Add("altitude", newZ);

                flightJson.Add("speed", speed);
                // Console.WriteLine(flightJson.ToString());

               // byte[] body = Encoding.Default.GetBytes(flightJson.ToString());
                arrayList.Add(flightJson.ToString());
            }

            if (trackId == Constants.TRACKPERIOD + 1)
            {
                trackId = 1;
                x = Constants.XSTART;
                y = Constants.YSTART;
            }
            else
            {
                trackId++;
                x = Math.Round((x + xAdv),2);
                y = Math.Round((y +yAdv),2);
            }


            return arrayList;


        }

        /*private List<Point> createIntPoints()
        {
            List<Point> points = new List<Point>();

            points.Add(new Point(1.5, 1.5));
            points.Add(new Point(1.5, 2));
            points.Add(new Point(1.5, 1));

            return points;

        }*/

        /*private List<Point> createExtPoints()
        {
            List<Point> points = new List<Point>();

            points.Add(new Point(0.5, 1.5));
            points.Add(new Point(1.5, 2.5));
            points.Add(new Point(1.5, 3));
            points.Add(new Point(1.5, 0.5));

            return points;
        }*/
    }
}
