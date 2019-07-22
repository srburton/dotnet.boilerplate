using Microsoft.Extensions.Configuration;

namespace App.Infra.Integration.Aws.Models
{    
    internal class Option
    {
        public string PublicKey { get; set; }

        public string SecretKey { get; set; }

        public OptionS3 S3 { get; set; }

        public static Option Parse(IConfiguration configuration)
            => configuration.GetSection(nameof(Aws))
                            .Get<Option>();
    }
}
