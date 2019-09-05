using System;
using System.Text;
using RabbitMQ.Client;

namespace rabbit_sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("samplequeue", true, false, false, null);
                    channel.BasicPublish("", "samplequeue", false, null, CreateMessage("Hello"));
                    Console.WriteLine("Added message to the queue.");
                }
            }
        }

        static byte[] CreateMessage(string text)
        {
            var body = Encoding.UTF8.GetBytes(text);
            return body;
        }
    }
}
