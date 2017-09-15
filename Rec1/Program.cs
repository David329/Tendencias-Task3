﻿using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rec1
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost",Port= 5672 };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",type: "direct");
                var queueName = channel.QueueDeclare().QueueName;

                var severity = (args.Length > 0) ? args[0] : "info";
                // if(args.Length < 1)
                // {
                //     Console.Error.WriteLine("Usage: {0} [info] [warning] [error]",Environment.GetCommandLineArgs()[0]);
                //     Console.WriteLine(" Press [enter] to exit.");
                //     Console.ReadLine();
                //     Environment.ExitCode = 1;
                //     return;
                // }

                //foreach(var severity in args)//si hay mas colas
                //{
                    channel.QueueBind(queue: queueName,exchange: "direct_logs",routingKey: severity);
                //}

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",routingKey, message);
                    var response = Http.Post("http://localhost:8080/usuarios", new NameValueCollection() {
                        { "home", message }
                    });
                    Console.WriteLine("Lusho: {0}",System.Text.Encoding.UTF8.GetString(response));
                };
                channel.BasicConsume(queue: queueName,autoAck: true,consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
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
