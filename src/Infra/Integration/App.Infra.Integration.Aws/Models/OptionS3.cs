using Amazon;
using Amazon.S3;

namespace App.Infra.Integration.Aws.Models
{
    internal class OptionS3
    {
        public string Bucket { get; set; }

        /// <summary>
        ///  Region Name:
        ///  <para>https://docs.aws.amazon.com/pt_br/general/latest/gr/rande.html</para>
        ///  <para>https://docs.aws.amazon.com/sdkfornet1/latest/apidocs/html/T_Amazon_RegionEndpoint.htm</para>
        /// </summary>        
        private RegionEndpoint _region;
        public object Region {
            get => _region;
            set
            {
                switch ((value as string).ToLower())
                {
                    case "us-east-1":
                        _region = RegionEndpoint.USEast1;
                        break;
                    case "us-east-2":
                        _region = RegionEndpoint.USEast2;
                        break;
                    case "us-west-1":
                        _region = RegionEndpoint.USWest1;
                        break;
                    case "us-west-2":
                        _region = RegionEndpoint.USWest2;
                        break;
                    case "ap-east-1":
                        _region = default(RegionEndpoint);
                        break;
                    case "ap-south-1":
                        _region = RegionEndpoint.APSouth1;
                        break;
                    case "ap-northeast-1":
                        _region = RegionEndpoint.APNortheast1;
                        break;
                    case "ap-northeast-2":
                        _region = RegionEndpoint.APNortheast2;
                        break;
                    case "ap-northeast-3":
                        _region = RegionEndpoint.APNortheast3;
                        break;
                    case "ap-southeast-1":
                        _region = RegionEndpoint.APSoutheast1;
                        break;
                    case "ap-southeast-2":
                        _region = RegionEndpoint.APSoutheast2;
                        break;
                    case "ca-central-1":
                        _region = RegionEndpoint.CACentral1;
                        break;
                    case "cn-north-1":
                        _region = RegionEndpoint.CNNorth1;
                        break;
                    case "cn-northwest-1":
                        _region = RegionEndpoint.CNNorthWest1;
                        break;
                    case "eu-central-1":
                        _region = RegionEndpoint.EUCentral1;
                        break;
                    case "eu-west-1":
                        _region = RegionEndpoint.EUWest1;
                        break;
                    case "eu-west-2":
                        _region = RegionEndpoint.EUWest2;
                        break;
                    case "eu-west-3":
                        _region = RegionEndpoint.EUWest3;
                        break;
                    case "eu-north-1":
                        _region = RegionEndpoint.EUNorth1;
                        break;
                    case "sa-east-1":
                        _region = RegionEndpoint.SAEast1;
                        break;
                    case "us-gov-east-1":
                        _region = RegionEndpoint.USGovCloudEast1;
                        break;
                    case "us-gov-west-1":
                        _region = RegionEndpoint.USGovCloudWest1;
                        break;
                }
            }
        }
        /// <summary>
        /// https://docs.aws.amazon.com/sdkfornet1/latest/apidocs/html/T_Amazon_S3_Model_S3CannedACL.htm
        /// </summary>
        public S3CannedACL _acl { get; set; }
        public object Acl
        {
            get => _acl;
            set
            {
                switch ((value as string).ToLower())
                {
                    case "private":
                        _acl = S3CannedACL.Private;
                        break;
                    case "publicread":
                        _acl = S3CannedACL.PublicRead;
                        break;
                    case "publicreadwrite":
                        _acl = S3CannedACL.PublicReadWrite;
                        break;
                    case "authenticatedread":
                        _acl = S3CannedACL.AuthenticatedRead;
                        break;
                    case "bucketownerread":
                        _acl = S3CannedACL.BucketOwnerRead;
                        break;
                    case "bucketownerfullcontrol":
                        _acl = S3CannedACL.BucketOwnerFullControl;
                        break;
                    default:
                        _acl = S3CannedACL.NoACL;
                        break;
                }
            }
        }

        public bool IsValid
            => (!string.IsNullOrEmpty(Bucket) && Acl != null && Region != null);
    }
}
