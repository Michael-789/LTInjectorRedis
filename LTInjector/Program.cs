// See https://aka.ms/new-console-template for more information

using LTInjector;
using LTInjector.RabbitMQ;

Console.WriteLine("Hello,please enter 1 to start");
int start = Convert.ToInt32(Console.ReadLine());
if (start == 1)
{
    Console.WriteLine("Press X to stop");


    RabbitMqReceiver rabbitMQReceiver = (RabbitMqReceiver)RabbitMqFactory.Instance.getInstance(Constants.RECIEVER, Constants.ALERTS_EXCHANGE);

        //new RabbitMqReceiver(Constants.ALERTS_EXCHANGE);
    var alert = new Alert();

    rabbitMQReceiver.Receive<string>(alert.getAlert);
    new DataCreator().send2Rabbit();
}