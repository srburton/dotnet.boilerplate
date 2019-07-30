using System;
using System.Text;
using RabbitMQ.Client;
using App.Bootstrap;
using App.Domain.Interfaces;
using RabbitMQ.Client.Events;
using App.Infra.Integration.RabbitMq.Core;
using App.Bootstrap.Attributes;
using App.Infra.Integration.RabbitMq.Attributes;
using App.Infra.Integration.RabbitMq.Extensions;

namespace App.Infra.Integration.RabbitMq
{
    [Singleton]
    public class EventBusService : IService<EventBusService>, IEventBus<EventBusService>
    {
        public void Publish<TMessage>(TMessage message)
        {
            using (var channel = RabbitMqCore.Connection.CreateModel())
            {
                var option = EventBusAttribute.Parse(typeof(TMessage));
                var body = message.Serialize().GetBytes();

                RabbitMqCore.BuildContext(channel, option);
                RabbitMqCore.DeadLetter(channel, option);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.DeliveryMode = option.DeliveryMode;                

                channel.BasicPublish(option.Exchange,
                                     option.RoutingKey,
                                     option.Mandatory,
                                     properties,
                                     body);
            }
        }

        public void Subscribe<TEvent, THandler>()
            => Subscribe(typeof(TEvent), typeof(THandler));

        internal void Subscribe(Type mType, Type hType)
        {
            var attributes = mType.GetCustomAttributes(typeof(EventBusAttribute), true);

            var channel = RabbitMqCore.Connection.CreateModel();
            
            foreach (var attribute in attributes)
            {
                if (attribute is EventBusAttribute option)
                {
                    RabbitMqCore.BuildContext(channel, option);
                    RabbitMqCore.DeadLetter(channel, option);

                    channel.BasicQos(0, 1, false);

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                    consumer.Received += async (model, ea) =>
                    {
                        long attempts = RabbitMqCore.GetAttempts(ea.BasicProperties.Headers);
                        string body = Encoding.UTF8.GetString(ea.Body);

                        try
                        {
                            if (attempts > option.Retry)
                                throw new AccessViolationException("Number of attempts exceeded.");

                            await RabbitMqCore.ProcessEvent(body, mType, hType, ea);

                            channel.BasicAck(ea.DeliveryTag, multiple: false);
                        }
                        catch (AccessViolationException)
                        {
                            channel.BasicReject(ea.DeliveryTag, option.RejectRequeue);
                        }
                        catch (Exception)
                        {
                            channel.BasicNack(ea.DeliveryTag, false, false);
                        }
                    };

                    channel.BasicConsume(option.Queue,
                                         autoAck: false,
                                         consumer: consumer);
                }
            }
        }
    }
}