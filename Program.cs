using System;
using System.Text;
using log4net;
using ironiclensflare.logger;
using RabbitMQ.Client;

namespace rabbit_sender
{
    class Program
    {
        private static readonly ILog _logger = Logger.GetLogger();

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
                _logger.Info("Connecting to RabbitMQ");
                using (var channel = connection.CreateModel())
                {
                    _logger.Debug("Declaring queue");
                    channel.QueueDeclare("samplequeue", true, false, false, null);

                    _logger.Info($"Found {messagesToAdd} message(s) to add");
                    for (var i = 0; i < messagesToAdd; i++)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var message = CreateMessage(guid);
                        _logger.Debug($"Added {guid} to the queue");
                        channel.BasicPublish("", "samplequeue", false, null, message);
                    }

                    _logger.Info($"Added {messagesToAdd} message(s) to the queue.");
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
