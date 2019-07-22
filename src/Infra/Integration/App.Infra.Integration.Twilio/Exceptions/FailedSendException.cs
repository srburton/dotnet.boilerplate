using System;

namespace App.Infra.Integration.Twilio.Exceptions
{
    public class FailedSendException : Exception
    {
        public string[] Numbers { get => Message.Split(","); }

        public FailedSendException(string[] number)
            : base(string.Join(",", number))
        {
        }
    }
}
