namespace App.Infra.Integration.Sentry.Models
{
    internal class Debug
    {
        public string Title { get; set; }

        public object Payload { get; set; }

        public Debug(string title, object payload)
        {
            Title = title;
            Payload = payload;
        }
    }
}
