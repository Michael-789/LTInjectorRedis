// See https://aka.ms/new-console-template for more information

using EltaDataSimulator;
using EltaParser;

Console.WriteLine("Hello,please enter 1 to start");
int start = Convert.ToInt32(Console.ReadLine());
if (start == 1)
{
    Console.WriteLine("Press X to stop");

    RabbitMQHandler rabbitMQHandler = new RabbitMQHandler();
    string[] queues = { "alerts" };
    string[] exchages = { "alerts" };
    rabbitMQHandler.Receive(queues, exchages, "#");
    DataCreator.send2Rabbit();
}