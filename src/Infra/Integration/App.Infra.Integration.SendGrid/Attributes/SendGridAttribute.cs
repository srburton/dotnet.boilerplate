using System;
using System.Reflection;

namespace App.Infra.Integration.SendGrid.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SendGridAttribute : Attribute
    {
        private string _from;
        private string _templateId;

        public SendGridAttribute(string templateId, string from = "")
        {
            _templateId = templateId;
            _from = from;
        }

        public virtual string From
        {
            get { return _from; }
        }

        public virtual string TemplateId
        {
            get { return _templateId; }
        }

        public static SendGridAttribute Parse(Type type)
           => type.GetTypeInfo()
                  .GetCustomAttribute<SendGridAttribute>() ?? throw new ArgumentNullException($"Not exist attribute [SendGrid()] in {type.GetType().Name}");
    }
}
