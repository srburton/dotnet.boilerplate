using System;
using System.Reflection;

namespace App.Infra.Integration.RabbitMq.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventBusAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Queue { get; set; } = "queue-demo";
        /// <summary>
        /// 
        /// </summary>
        public string Exchange { get; set; } = "exchange-demo";
        /// <summary>
        /// 
        /// </summary>
        public string RoutingKey { get; set; } = "routekey-demo";
        /// <summary>
        ///  Use:
        ///  EventBusAttribute.ExchangeTypes
        /// </summary>
        public string Type { get; set; } = ExchangeTypes.Topic;
        /// <summary>
        ///   Non-persistent (1) or persistent (2).
        /// </summary>
        public byte DeliveryMode { get; set; } = 2;
        /// <summary>
        /// 
        /// </summary>
        public bool Durable { get; set; } = true;
        /// <summary>
        /// https://www.rabbitmq.com/ttl.html        
        /// </summary>
        public TimeSpan Ttl { get; set; } = TimeSpan.FromMinutes(1);
        /// <summary>
        /// 
        /// </summary>
        public bool Exclusive { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public bool AutoDelete { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Mandatory { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        public bool RejectRequeue { get; set; } = false;
        /// <summary>
        /// Number of attempts to reprocess the message
        /// </summary>
        public int Retry { get; set; } = 2;
        /// <summary>
        /// 
        /// </summary>
        public EventBusAttribute()
        {
            RoutingKey = "";
        }
        /// <summary>
        /// 
        /// </summary>
        public struct ExchangeTypes
        {
            //
            // Summary:
            //     Exchange type used for AMQP direct exchanges.
            public const string Direct = "direct";
            //
            // Summary:
            //     Exchange type used for AMQP fanout exchanges.
            public const string Fanout = "fanout";
            //
            // Summary:
            //     Exchange type used for AMQP headers exchanges.
            public const string Headers = "headers";
            //
            // Summary:
            //     Exchange type used for AMQP topic exchanges.
            public const string Topic = "topic";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EventBusAttribute Parse(Type type)
           => type.GetTypeInfo()
                  .GetCustomAttribute<EventBusAttribute>() ?? throw new ArgumentNullException($"Not exist attribute [EventBus(...)] in {type.GetType().Name}");
    }
}
