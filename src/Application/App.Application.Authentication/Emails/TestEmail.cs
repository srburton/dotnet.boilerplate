using App.Infra.Integration.SendGrid.Attributes;

namespace App.Application.Authentication.Emails
{
    [SendGrid(templateId: "d-1f2058556f24460ba8e1c96fc2f7ad5f")]
    public class TestEmail
    {
        public string Name { get; set; }

        public string LastName { get; set; }
    }
}
