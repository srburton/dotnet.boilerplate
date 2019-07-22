using System;
using SharpRaven;
using SharpRaven.Data;
using App.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using App.Infra.Bootstrap.Attributes;
using Microsoft.Extensions.Configuration;
using App.Infra.Integration.Sentry.Models;
using App.Infra.Bootstrap;

namespace App.Infra.Integration.Sentry
{
    [Transient]
    public class SentryService : IService<SentryService>, ILog<SentryService>
    {
        readonly RavenClient _client;

        readonly Option _option;

        public SentryService(IConfiguration configuration, IHostingEnvironment environment)
        {
            _option = Option.Parse(configuration);

            _client = new RavenClient(_option.Dsn);
           
            _client.Tags.Add("environment", environment.EnvironmentName);
            _client.Tags.Add("applicationName", environment.ApplicationName);
        }

        public void Debug(string title, object obj, Exception exc = null)
            => Process(new Payload(new Debug(title, obj), exc), ErrorLevel.Debug);

        public void Error(string str)
            => Process(new Payload(str), ErrorLevel.Error);

        public void Fatal(Exception exc)
            => Process(new Payload(default(object), exc), ErrorLevel.Fatal);

        public void Information(string str)
            => Process(new Payload(str), ErrorLevel.Info);

        public void Warning(string str)
            => Process(new Payload(str), ErrorLevel.Warning);

        private void Process(Payload payload, ErrorLevel level)
        {
            var sEvent = new SentryEvent(payload.Message());

            sEvent.Level = level;

            _client.Capture(sEvent);
        }
    }
}
