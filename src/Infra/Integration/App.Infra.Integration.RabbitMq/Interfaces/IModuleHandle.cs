using App.Infra.Integration.RabbitMq.Modules;
using System.Threading.Tasks;

namespace App.Infra.Integration.RabbitMq.Interfaces
{
    public interface IModuleHandle
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Task PublishEvent(EventBusArgs e);
        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Task SubscribeEvent(EventBusArgs e);
    }
}
