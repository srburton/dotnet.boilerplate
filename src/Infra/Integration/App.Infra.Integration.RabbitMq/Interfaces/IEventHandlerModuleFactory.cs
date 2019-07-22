using App.Infra.Integration.RabbitMq.Modules;

namespace App.Infra.Integration.RabbitMq.Interfaces
{
    public interface IEventHandlerModuleFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        void TryAddMoudle(IModuleHandle module);
        /// <summary>
        /// 
        /// </summary>
        void PubliushEvent(EventBusArgs e);
        /// <summary>
        /// 
        /// </summary>
        void SubscribeEvent(EventBusArgs e);
    }
}
