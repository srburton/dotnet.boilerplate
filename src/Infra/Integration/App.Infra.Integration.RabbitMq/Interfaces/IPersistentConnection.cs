using RabbitMQ.Client;
using App.Infra.Integration.RabbitMq.Modules;

namespace App.Infra.Integration.RabbitMq.Interfaces
{
    public interface IPersistentConnection
    {
        /// <summary>
        /// 
        /// </summary>
        ConnectionConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        string Endpoint { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool TryConnect();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IModel CreateModel();
    }
}
