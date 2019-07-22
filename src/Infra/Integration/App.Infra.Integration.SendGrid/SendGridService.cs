using System;
using SendGrid;
using System.Linq;
using SendGrid.Helpers.Mail;
using App.Domain.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using App.Infra.Integration.SendGrid.Models;
using App.Infra.Integration.SendGrid.Attributes;
using App.Infra.Bootstrap;

namespace App.Infra.Integration.SendGrid
{
    public class SendGridService : IService<SendGridService>, INotification<SendGridService>
    {
        readonly SendGridClient _client;

        readonly Option _option;

        public SendGridService(IConfiguration configuration)
        {
            _option = Option.Parse(configuration);

            _client = new SendGridClient(_option.Apikey);
        }

        public void Send(string to, string body)
            => Process(string.Empty, new string[] { to }, body);

        public void Send(string from, string to, string body)
            => Process(from, new string[] { to }, body);

        public void Send<B>(string to, B body)
            => Process(string.Empty,new string[] { to }, body);

        public void Send<B>(string from, string to, B body)
            => Process(from, new string[] { to }, body);

        public void Send(string[] tos, string body)
            => Process(string.Empty, tos, body);

        public void Send<B>(string[] tos, B body)
            => Process(string.Empty, tos, body);

        private void Process<B>(string from, string[] tos, B body)
        {
            var attr = SendGridAttribute.Parse(body.GetType());

            var nFrom = FromEmailAddress(from, _option.FromRandom(), attr.From);
            var nTemplateId = attr.TemplateId;
            var nTos = tos.Select(x => new EmailAddress(x))
                          .ToList();

            _client.SendEmailAsync(new SendGridMessage()
            {
                From = nFrom,
                TemplateId = nTemplateId,
                Personalizations = new List<Personalization>()
                {
                   new Personalization()
                   {
                       TemplateData = body,
                       Tos = nTos
                   }
                }
            });
        }

        private EmailAddress FromEmailAddress(string from, string fromOption, string fromAttr)
        {
            if (!string.IsNullOrEmpty(from))
            {
                return new EmailAddress(from);
            }
            else if (!string.IsNullOrEmpty(fromAttr))
            {
                return new EmailAddress(fromAttr);
            }
            else if (!string.IsNullOrEmpty(fromOption))
            {
                return new EmailAddress(fromOption);
            }

            throw new Exception("Sendgrid (from) not found!");
        }
    }
}
