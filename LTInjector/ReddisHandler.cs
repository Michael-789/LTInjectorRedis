using Newtonsoft.Json;
using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Aggregation;
using NRedisStack.Search.Literals.Enums;
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

        public void send2Set(string key, string json)
        {
            //db.StringSet(key, json);
            db.StringAppend(key, json);
            var ft = db.FT();
            //RPUSH myKey '{"key":"value"}'
           // ft.Create()
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
