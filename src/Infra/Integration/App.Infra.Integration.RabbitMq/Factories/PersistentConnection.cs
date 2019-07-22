using App.Infra.Integration.RabbitMq.Interfaces;
using App.Infra.Integration.RabbitMq.Modules;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace App.Infra.Integration.RabbitMq.Factories
{
    internal sealed class PersistentConnection : IPersistentConnection
    {
        public ConnectionConfiguration Configuration { get; }

        readonly IConnectionFactory _connectionFactory;

        private IConnection _connection;
        private bool _disposed;
        private readonly object sync_root = new object();
        public string Endpoint => _connection?.Endpoint.ToString();

        public PersistentConnection(ConnectionConfiguration configuration, IConnectionFactory connectionFactory)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            try
            {
                if (_disposed)
                    return;

                _disposed = true;

                _connection.Dispose();
            }
            catch (IOException ex)
            {
                //_logger.LogCritical(ex.ToString());
            }
        }


        public bool TryConnect()
        {
            lock (sync_root)
            {
                RetryPolicy policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(Configuration.FailReConnectRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        //_logger.LogWarning(ex.ToString());
                    });

                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection(clientProvidedName: Configuration.ClientProvidedName);
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    return true;
                }
                else
                {
                    //_logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    return false;
                }
            }
        }
        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed)
                return;

            //_logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed)
                return;

            //_logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed)
                return;

            //_logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}
