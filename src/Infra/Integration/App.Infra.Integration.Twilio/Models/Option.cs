using System;
using Microsoft.Extensions.Configuration;

namespace App.Infra.Integration.Twilio.Models
{
    internal class Option
    {
        public string AccountSid { get; set; }

        public string AuthToken { get; set; }

        public string[] From { get; set; }

        public string FromRandom()
        {
            Random rand = new Random();
            int index = rand.Next(From.Length);
            return From[index];
        }

        public static Option Parse(IConfiguration configuration)
            => configuration.GetSection(nameof(Twilio))
                            .Get<Option>();
    }
}
