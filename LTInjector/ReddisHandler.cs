using Newtonsoft.Json;
using StackExchange.Redis;

namespace LTInjector
{
    internal class ReddisHandler
    {
        ConnectionMultiplexer redis;
        IDatabase db;

        public ReddisHandler() { }

        public void connect()
        {
            redis = ConnectionMultiplexer.Connect("localhost");
            db = redis.GetDatabase();
        }

        public void push2List(string listName, string json)
        {
            db.ListRightPush(listName, json);
        }

        public void popFromList(string listName)
        {
            while (true)
            {
                var obj = db.ListLeftPop(listName);
                if (obj.HasValue)
                {
                    //var json = result.ToString();
                   // var jsonObj = new GeoJsonWriter().Write(obj);
                    //var jsonObject = JsonConvert.DeserializeObject(jsonObj);
                   // Console.WriteLine($"Popped JSON object: {jsonObj}");
                }
                else
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("No JSON object available. Waiting for 1 seconds...");
                }
            }
        }


    }
}
