using Amazon;
using Amazon.S3;

namespace App.Infra.Integration.Aws.Interfaces
{
    public interface IBucketAws
    {
        string Bucket { get; }

        /// <summary>
        /// Example:
        /// <para>string[] Path =>  new string[] {"user","avatar"}</para>
        /// Output:
        /// <para>/user/avatar/[file-name]</para>
        /// </summary>
        string[] Path { get; }

        RegionEndpoint Region { get; }

        S3CannedACL Acl { get; }
    }
}
