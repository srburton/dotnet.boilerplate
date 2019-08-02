using App.Application.Authentication.Events;
using App.Infra.Integration.RabbitMq.Interfaces;
using System;
using System.Threading.Tasks;

namespace App.Application.Authentication.Handlers
{
    public class TestRabbitMqHandle : IEventHandler<TestRabbitMq>
    {
        public Task Handle(TestRabbitMq message)
        {
            if (message.Name.Contains("Renato"))
                throw new Exception("Not found!");

            return Task.CompletedTask;
        }
    }
}
