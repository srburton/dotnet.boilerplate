using System;
using Microsoft.Extensions.Configuration;

namespace App.Infra.Integration.SendGrid.Models
{
    internal class Option
    {
        public string Apikey { get; set; }

        public string[] From { get; set; }

        public string FromRandom()
        {
            Random rand = new Random();
            int index = rand.Next(From.Length);
            return From[index];
        }

        public static Option Parse(IConfiguration configuration)
            => configuration.GetSection(nameof(SendGrid))
                            .Get<Option>();
    }
}
