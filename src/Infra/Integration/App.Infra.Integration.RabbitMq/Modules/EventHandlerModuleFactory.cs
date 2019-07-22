using System;
using System.Collections.Generic;
using App.Infra.Integration.RabbitMq.Interfaces;

namespace App.Infra.Integration.RabbitMq.Modules
{
    internal class EventHandlerModuleFactory : IEventHandlerModuleFactory
    {
        private readonly List<IModuleHandle> modules;
        private readonly object sync_root = new object();        

        public EventHandlerModuleFactory()
        {
            modules = new List<IModuleHandle>();
        }
        /// <summary>
        /// 
        /// </summary>
        public void PubliushEvent(EventBusArgs e)
        {
            lock (sync_root)
            {
                foreach (IModuleHandle model in modules)
                {
                    model.PublishEvent(e);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SubscribeEvent(EventBusArgs e)
        {
            lock (sync_root)
            {
                foreach (IModuleHandle model in modules)
                {
                    model.SubscribeEvent(e);
                }
            }
        }

        public void TryAddMoudle(IModuleHandle module)
        {
            lock (sync_root)
            {
                modules.Add(module);
            }
        }
    }
}
