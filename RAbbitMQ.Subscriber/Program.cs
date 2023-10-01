using Microsoft.AspNet.SignalR.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace RAbbitMQ.Subscriber
{

    class Program
    {
        static void Main(string[] args)
        {


            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://vxdvxesj:VT6vpW9ieHhgHroI29tDgiM2yqHaXezh@toad.rmq.cloudamqp.com/vxdvxesj");



            using (var connnection = factory.CreateConnection())
            {

                var channel = connnection.CreateModel();



                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                // consumer kendien kuyruk oluşturacak.
                var queueName = channel.QueueDeclare().QueueName;
                var routeKey = "*.Error.*";// Örnek bir route lama . Kullanıcının nasıl birşey isteyeceğini bilemeyiz. O yüzden kuyruk oluşturma işi consumer da
                channel.QueueBind(queueName, "logs-topic", routeKey);

                channel.BasicConsume(queueName, false, consumer);
                Console.WriteLine("Log alınıyor........");

                consumer.Received += (object sender, BasicDeliverEventArgs e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());
                    Console.WriteLine("Gelen Mesaj =======>" + message);
                   


                    channel.BasicAck(e.DeliveryTag, false);
                };

                Console.ReadKey();

            }

        }


    }
}

