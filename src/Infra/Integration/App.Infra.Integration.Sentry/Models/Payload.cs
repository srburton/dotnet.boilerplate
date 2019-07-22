using Newtonsoft.Json;
using SharpRaven.Data;
using System;

namespace App.Infra.Integration.Sentry.Models
{
    internal class Payload
    {
        public object Data { get; set; }

        public Exception Exception { get; set; }

        public Payload(object data, Exception exception = null)
        {
            Data = Data;
            Exception = exception;
        }

        public SentryMessage Message()
            => new SentryMessage(JsonConvert.SerializeObject(this));
    }
}
