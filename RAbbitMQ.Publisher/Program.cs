using RabbitMQ.Client;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace RAbbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }




    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://vxdvxesj:VT6vpW9ieHhgHroI29tDgiM2yqHaXezh@toad.rmq.cloudamqp.com/vxdvxesj");



            using (var connnection = factory.CreateConnection())
            {


                var channel = connnection.CreateModel();

                //Bir önceki örnekte queue oluşturmuştuk. Bu sefer sadece Exchange oluşturuyorum.
                channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

                //Topic Exchange de direktExchange ye göre çok karmaşık routelamalar mümkün. Bu nedenle random metodu ile farklı tipte route lama yapılmasını sağlamak istiyorum.
                Random rnd = new Random();
                foreach (var item in Enumerable.Range(1, 125))
                {

                    LogNames log1 = (LogNames)rnd.Next(1, 5);
                    LogNames log2 = (LogNames)rnd.Next(1, 5);
                    LogNames log3 = (LogNames)rnd.Next(1, 5);

                    var routeKey = $"{log1}.{log2}.{log3}"; // Her bir log um ilgili routuna gitmeli
                    string message = $"log-type:{log1}-{log2}-{log3}"; // Aldığı LogName i message değişkenine atıca.
                    var messageBody = Encoding.UTF8.GetBytes(message);// Değişken ile gelen LogName i Kuyruğa yönlendirmek üzere Byte[] alıcam



                    channel.BasicPublish("logs-topic", routeKey, null, messageBody);

                    Console.WriteLine($"Log gönderilmiştir : {message}");

                }

                Console.ReadKey();
            }
        }
    }

}
