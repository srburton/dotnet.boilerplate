using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Infra.Integration.RabbitMq.Attributes;
using App.Infra.Integration.RabbitMq.Factories;
using App.Infra.Integration.RabbitMq.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleInjector;

namespace App.Infra.Integration.RabbitMq.Core
{
    internal class RabbitMqCore
    {
        public static Container Ioc;
        public static PersistentConnection Connection;

        public RabbitMqCore(Container ioc, IPersistentConnection persistentConnection, IEventHandlerModuleFactory eventHandlerFactory)
        {
            Connection = (PersistentConnection)persistentConnection;
            Ioc = ioc;
        }

        static string _prefixDlqMake(string value)
            => $"{value}-dlq";

        public static void BuildContext(IModel channel, EventBusAttribute option)
        {
            var argEx = new Dictionary<string, object>();
            var argQu = new Dictionary<string, object>();

            argQu.Add("x-dead-letter-exchange", _prefixDlqMake(option.Exchange));

            channel.ExchangeDeclare(option.Exchange,
                                    option.Type,
                                    option.Durable,
                                    option.AutoDelete,
                                    argEx);

            channel.QueueDeclare(option.Queue,
                                 option.Durable,
                                 option.Exclusive,
                                 option.AutoDelete,
                                 argQu);

            channel.QueueBind(option.Queue,
                              option.Exchange,
                              option.RoutingKey);
        }

        public static void DeadLetter(IModel channel, EventBusAttribute option)
        {
            var argEx = new Dictionary<string, object>();

            var argQu = new Dictionary<string, object>();
            argQu.Add("x-dead-letter-exchange", option.Exchange);
            argQu.Add("x-message-ttl", (int)option.Ttl.TotalMilliseconds);

            channel.ExchangeDeclare(_prefixDlqMake(option.Exchange),
                                   ExchangeType.Fanout,
                                   option.Durable,
                                   option.AutoDelete,
                                   argEx);

            channel.QueueDeclare(_prefixDlqMake(option.Queue),
                                 option.Durable,
                                 option.Exclusive,
                                 option.AutoDelete,
                                 argQu);

            channel.QueueBind(_prefixDlqMake(option.Queue),
                              _prefixDlqMake(option.Exchange),
                              option.RoutingKey);
        }

        public static async Task ProcessEvent(string body, Type eventType, Type eventHandleType, BasicDeliverEventArgs args)
        {
            object eventHandler = Ioc.GetInstance(eventHandleType);
            if (eventHandler == null)
                throw new InvalidOperationException(eventHandleType.Name);

            object integrationEvent = JsonConvert.DeserializeObject(body, eventType);
            Type concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
            await (Task)concreteType.GetMethod(nameof(IEventHandler<IEvent>.Handle))
                                    .Invoke(eventHandler, new object[] { integrationEvent/*, new MessageEventArgs(body, args.Redelivered)*/ });
        }

        public static long GetAttempts(IDictionary<string, object> headers)
        {
            try
            {
                if (headers == null || !headers.ContainsKey("x-death"))
                    throw new ArgumentNullException();

                var xDeathData = (List<object>)headers["x-death"];

                if (xDeathData.Count < 1)
                    throw new ArgumentException();

                var xDeathMostRecentMetaData = (Dictionary<string, object>)xDeathData[0];

                if (!xDeathMostRecentMetaData.ContainsKey("count"))
                    throw new ArgumentException();

                return (long)xDeathMostRecentMetaData["count"];
            }
            catch (Exception)
            {
                return 0;
            }            
        }
    }
}
