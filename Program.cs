using System;
using System.Text;
using RabbitMQ.Client;

namespace rabbit_sender
{
    class Program
    {
        static void Main(string[] args)
        {
            int messagesToAdd = 1;

            if (args.Length > 0 && args[0] != null)
            {
                _ = int.TryParse(args[0], out messagesToAdd);
            }

            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("samplequeue", true, false, false, null);

                    for (var i = 0; i < messagesToAdd; i++)
                    {
                        var message = CreateMessage(Guid.NewGuid().ToString());
                        channel.BasicPublish("", "samplequeue", false, null, message);
                    }

                    Console.WriteLine($"Added {messagesToAdd} message(s) to the queue.");
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
