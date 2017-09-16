using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rec3
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "166.62.89.37",Port= 8080 };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",type: "direct");
                var queueName = channel.QueueDeclare().QueueName;

                var severity =  "cash";

                //foreach(var severity in args)//si hay mas colas
                //{
                    channel.QueueBind(queue: queueName,exchange: "direct_logs",routingKey: severity);
                //}

                Console.WriteLine(" [*] Esperando los mensaje, Canal: cash");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",routingKey, message);
                    var response = Http.Post("http://localhost:8080/pedidos", new NameValueCollection() {
                        { "pedido", message }
                    });
                };
                channel.BasicConsume(queue: queueName,autoAck: true,consumer: consumer);

                Console.WriteLine(" Presiona [enter] para salir");
                Console.ReadLine();
            }
        }
    }
    public class Pedido
    {
        public string tipo;
        public Producto prod;
    }
    public class Producto
    {
        public string nombre;
        public float precio;
        public int cantidad;
    }
    public static class Http
    {
        public static byte[] Post(string uri, NameValueCollection pairs)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(uri, pairs);
            }
            return response;
        }
    }
}
