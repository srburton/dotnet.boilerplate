using System;
using App.Infra.Integration.RabbitMq.Interfaces;

namespace App.Infra.Integration.RabbitMq.Modules
{
    public sealed class ModuleOption
    {
        public IServiceProvider ApplicationServices;
        private readonly IEventHandlerModuleFactory handlerFactory;        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlerFactory"></param>
        public ModuleOption(IEventHandlerModuleFactory handlerFactory, IServiceProvider applicationServices)
        {
            this.handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            ApplicationServices = applicationServices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        public void AddModule(IModuleHandle module)
        {
            handlerFactory.TryAddMoudle(module);
        }
    }
}
