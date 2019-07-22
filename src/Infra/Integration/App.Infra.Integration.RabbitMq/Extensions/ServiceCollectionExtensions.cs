using System;
using SimpleInjector;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Builder;
using App.Infra.Integration.RabbitMq.Modules;
using App.Infra.Integration.RabbitMq.Factories;
using App.Infra.Integration.RabbitMq.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using App.Infra.Integration.RabbitMq.Core;

namespace App.Infra.Integration.RabbitMq.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="eventBusOptionAction"></param>
        /// <returns></returns>
        public static void AddRabbitMQEventBus(this Container container,
                                               string connectionString,
                                               Action<ConnectionConfigurationBuild> eventBusOptionAction)
        {
            ConnectionConfiguration configuration = new ConnectionConfiguration();
            ConnectionConfigurationBuild configurationBuild = new ConnectionConfigurationBuild(configuration);

            eventBusOptionAction?.Invoke(configurationBuild);

            container.RegisterSingleton<IPersistentConnection>(() =>
            {
                IConnectionFactory factory = new ConnectionFactory
                {
                    AutomaticRecoveryEnabled = configuration.AutomaticRecoveryEnabled,
                    NetworkRecoveryInterval = configuration.NetworkRecoveryInterval,
                    Uri = new Uri(connectionString),
                };

                var connection = new PersistentConnection(configuration, factory);

                connection.TryConnect();

                return connection;
            });            
            
            container.RegisterSingleton<IEventHandlerModuleFactory, EventHandlerModuleFactory>();
            container.RegisterSingleton<RabbitMqCore, RabbitMqCore>();
            
            foreach (Type mType in typeof(IEvent).GetAssemblies())
            {
                container.Register(mType);
                
                foreach (Type hType in typeof(IEventHandler<>).GetMakeGenericType(mType))
                {
                    container.Register(hType);
                }
            }            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void RabbitMQAutoSubscribe(this IApplicationBuilder app, Container container)
        {
            EventBusService eventBus = container.GetRequiredService<EventBusService>();

            foreach (Type mType in typeof(IEvent).GetAssemblies())
            {
                foreach (Type hType in typeof(IEventHandler<>).GetMakeGenericType(mType))
                {
                    eventBus.Subscribe(mType, hType);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="moduleOptions"></param>
        public static void RabbitMQEventBusModule(this IApplicationBuilder app, Action<ModuleOption> moduleOptions)
        {
            IEventHandlerModuleFactory factory = app.ApplicationServices.GetRequiredService<IEventHandlerModuleFactory>();
            ModuleOption moduleOption = new ModuleOption(factory, app.ApplicationServices);
            moduleOptions?.Invoke(moduleOption);
        }
    }
}
