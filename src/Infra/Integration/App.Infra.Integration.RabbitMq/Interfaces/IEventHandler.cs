using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.Integration.RabbitMq.Interfaces
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task Handle(TEvent message);
    }
}
