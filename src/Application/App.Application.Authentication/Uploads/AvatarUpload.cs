using Amazon;
using Amazon.S3;
using App.Infra.Integration.Aws.Interfaces;

namespace App.Application.Authentication.Uploads
{
    public struct AvatarUpload : IBucketAws
    {
        public string Bucket 
            => "bomprahoje";

        public string[] Path 
            => new string[] { "user", "avatar" } ;

        public RegionEndpoint Region 
            => RegionEndpoint.USEast1;

        public S3CannedACL Acl 
            => S3CannedACL.PublicRead;

    }
}
