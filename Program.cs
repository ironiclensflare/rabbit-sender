using System;
using log4net;
using ironiclensflare.logger;

namespace rabbit_sender
{
    class Program
    {
        private static readonly ILog _logger = Logger.GetLogger();

        static void Main(string[] args)
        {
            var messagesToAdd = GetMessagesToAdd(args);

            using (var channel = new Channel("samplequeue"))
            {
                for (var i = 0; i < messagesToAdd; i++)
                {
                    var guid = Guid.NewGuid().ToString();
                    channel.SendMessage(guid);
                }
            }
        }

        static int GetMessagesToAdd(string[] args)
        {
            int messagesToAdd = 1;
            if (args.Length > 0 && args[0] != null)
            {
                _ = int.TryParse(args[0], out messagesToAdd);
            }

            _logger.Info($"Will add {messagesToAdd} message(s) to the queue.");
            return messagesToAdd;
        }
    }
}
