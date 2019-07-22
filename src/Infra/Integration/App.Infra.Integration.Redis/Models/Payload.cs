using System;

namespace App.Infra.Integration.Redis.Models
{
    internal class Payload
    {
        public object Data { get; set; }

        public DateTime Expiration { get; set; }
    }
}
