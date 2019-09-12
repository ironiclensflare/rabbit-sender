using System;
using System.Text;
using ironiclensflare.logger;
using log4net;
using RabbitMQ.Client;

namespace rabbit_sender {
    public interface IChannel {
        void SendMessage (string message);
    }

    public class Channel : IChannel, IDisposable {
        private readonly string _queueName;
        private static readonly ILog _logger = Logger.GetLogger ();
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Channel (string queueName) {
            _logger.Info ($"Creating channel for {queueName}");
            _queueName = queueName;
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            _connection = connectionFactory.CreateConnection ();
            _channel = _connection.CreateModel ();
            _channel.QueueDeclare (_queueName, true, false, false, null);
        }

        public void SendMessage (string message) {
            _logger.Info ($"Sending {message} to the queue...");
            _channel.BasicPublish ("", _queueName, false, null, ConvertToByteArray (message));
        }

        private byte[] ConvertToByteArray (string input) {
            return Encoding.UTF8.GetBytes (input);
        }

        private bool disposedValue = false;

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _logger.Debug ($"Disposing of channel {_queueName}");
                    _channel.Dispose ();
                    _connection.Dispose ();
                }

                disposedValue = true;
            }
        }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }
    }
}