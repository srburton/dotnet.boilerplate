using System;

namespace App.Infra.Integration.RabbitMq.Modules
{
    public class MessageEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string Original { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool Redelivered { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="redelivered"></param>
        public MessageEventArgs(string original, bool redelivered)
        {
            Original = original ?? throw new ArgumentNullException(nameof(original));
            Redelivered = redelivered;
        }
    }
}
