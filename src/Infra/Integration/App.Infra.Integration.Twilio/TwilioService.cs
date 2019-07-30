using System;
using Twilio;
using System.Linq;
using Twilio.Types;
using App.Bootstrap;
using App.Domain.Interfaces;
using System.Collections.Generic;
using Twilio.Rest.Api.V2010.Account;
using App.Bootstrap.Attributes;
using Microsoft.Extensions.Configuration;
using App.Infra.Integration.Twilio.Models;
using App.Infra.Integration.Twilio.Exceptions;
using static Twilio.Rest.Api.V2010.Account.Call.FeedbackSummaryResource;

namespace App.Infra.Integration.Twilio
{
    [Transient]
    public class TwilioService : IService<TwilioService>, INotification<TwilioService>
    {
        readonly Option _option;

        public TwilioService(IConfiguration configuration)
        {
            _option = Option.Parse(configuration);

            TwilioClient.Init(_option.AccountSid, _option.AuthToken);
        }

        public void Send(string to, string body)
            => Process(string.Empty, new string[] { to }, body);

        public void Send(string from, string to, string body)
            => Process(from, new string[] { to }, body);

        public void Send(string[] tos, string body)
            => Process(string.Empty, tos, body);

        public void Send<B>(string to, B body)
            => Process(string.Empty, new string[] { to }, body);

        public void Send<B>(string from, string to, B body)
            => Process(from, new string[] { to }, body);

        public void Send<B>(string[] tos, B body)
            => Process(string.Empty, tos, body);

        private void Process<B>(string from, string[] tos, B body)
        {
            var failNumber = new List<string>();

            var nFrom = FromPhoneAddress(from, _option.FromRandom());
            var nTos = tos.Select(x => new PhoneNumber(x))
                          .ToList();

            foreach (var to in nTos)
            {
                try
                {
                    var response = MessageResource.Create(new CreateMessageOptions(to)
                    {
                        From = nFrom,
                        Body = body.ToString()
                    });

                    if (response.Status == StatusEnum.Failed)
                        failNumber.Add(to.ToString());
                }
                catch (Exception)
                {
                    failNumber.Add(to.ToString());
                }
            }

            if (failNumber.Count > 0)
                throw new FailedSendException(failNumber.ToArray());
        }

        private PhoneNumber FromPhoneAddress(string from, string fromOption)
        {
            if (!string.IsNullOrEmpty(from))
            {
                return new PhoneNumber(from);
            }
            else if (!string.IsNullOrEmpty(fromOption))
            {
                return new PhoneNumber(fromOption);
            }

            throw new Exception("Twilio (from) not found!");
        }
    }
}
