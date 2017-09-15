using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace send
{
    class Program
    {
        public static void Main(string[] args)    
        {
            var factory = new ConnectionFactory() { HostName = "localhost",Port= 5672};
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",type: "direct");

                Pedido pedido=new Pedido();
                while(true)
                {
                    Console.Write("--------------------------\n\tNUEVO PEDIDO\n--------------------------\n");
                    Console.Write("Tipo: ");
                    pedido.tipo=Console.ReadLine();
                    Console.Write("Nombre: ");
                    pedido.prod.nombre=Console.ReadLine();
                    Console.Write("Precio: ");
                    pedido.prod.precio=float.Parse(Console.ReadLine());
                    Console.Write("Cantidad: ");
                    pedido.prod.cantidad=Int32.Parse(Console.ReadLine());

                    string message = JsonConvert.SerializeObject(pedido);

                    var body = Encoding.UTF8.GetBytes(message);
                    //severity=tipo de pedido
                    channel.BasicPublish(exchange: "direct_logs",routingKey: pedido.tipo,basicProperties: null,body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", pedido.tipo, message);

                    Console.WriteLine("Continuar?!(salir): ");
                    if(Console.ReadLine()=="salir")break;
                }
            }
            Console.WriteLine("Fin.");
        }
    }
    public class Pedido
    {
        public string tipo;
        public Producto prod;
        public Pedido()
        {
            this.prod=new Producto();
        }
    }
    public class Producto
    {
        public string nombre;
        public float precio;
        public int cantidad;
    }
}