using App.Infra.Integration.RabbitMq.Attributes;
using App.Infra.Integration.RabbitMq.Interfaces;

namespace App.Application.Authentication.Events
{
    [EventBus(Exchange = "authentication", Queue = "login", RoutingKey = "serviceName.applicationName.methodName")]
    public class TestRabbitMq: IEvent
    {
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
