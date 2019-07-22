using Microsoft.Extensions.Configuration;
using System;

namespace App.Infra.Integration.Sentry.Models
{
    internal class Option
    {
        public string Dsn { get; set; }

        public static Option Parse(IConfiguration configuration)
            => configuration.GetSection(nameof(Sentry))
                            .Get<Option>();
    }
}
