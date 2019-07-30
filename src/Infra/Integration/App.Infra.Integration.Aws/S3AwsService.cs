using System;
using Amazon;
using Amazon.S3;
using System.IO;
using Amazon.S3.Transfer;
using App.Bootstrap;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using App.Infra.Integration.Aws.Models;
using App.Infra.Integration.Aws.Interfaces;
using App.Infra.Integration.Aws.Exceptions;
using App.Bootstrap.Attributes;
using Microsoft.Extensions.Configuration;
using App.Infra.Integration.Aws.Extensions;
using Amazon.S3.Model;

namespace App.Infra.Integration.Aws
{
    [Transient]
    public sealed class S3AwsService : IService<S3AwsService>
    {
        readonly Option _option;

        public S3AwsService(IConfiguration configuration)
        {
            _option = Option.Parse(configuration);
        }

        private AmazonS3Client _client(AmazonS3Config config)
            => new AmazonS3Client(_option.PublicKey, _option.SecretKey, config);

        private AmazonS3Client _client(RegionEndpoint region)
            => new AmazonS3Client(_option.PublicKey, _option.SecretKey, region);
     
        public async Task<bool> Delete<T>(string fileName)
            where T : IBucketAws
        {
            try
            {
                var config = (T)Activator.CreateInstance(typeof(T));

                return await Delete(fileName,
                                    config.Bucket,
                                    config.Path,
                                    config.Region as RegionEndpoint);
            }
            catch (Exception e)
            {
                throw new BucketException(e.Message);
            }
        }
        public async Task<bool> Delete(string[] path, string fileName)
        {
            try
            {
                if (_option.S3 != null || !_option.S3.IsValid)
                    throw new BucketException("Not implemented in AppSetting. [ENV]. JSON in session  'AWS' The default settings of the bucket.");

                return await Delete(fileName,
                                    _option.S3.Bucket,
                                    path,
                                    _option.S3.Region as RegionEndpoint);
            }
            catch (Exception e)
            {
                throw new BucketException(e.Message);
            }
        }
        public async Task<bool> Delete(string fileName, string bucket, string[] path, RegionEndpoint region)
        {
            using (var client = _client(region))
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucket,
                    Key = fileName.ToBucketDirectory(path)
                };

                DeleteObjectResponse response = await client.DeleteObjectAsync(deleteObjectRequest);

                return response.HttpStatusCode.IsSuccess();
            }           
        }

        public async Task<Stream> Download<T>(string fileName)
           where T : IBucketAws
        {
            try
            {
                var config = (T)Activator.CreateInstance(typeof(T));

                return await Download(fileName,
                                      config.Bucket,
                                      config.Path,
                                      config.Region as RegionEndpoint);
            }
            catch (Exception e)
            {
                throw new BucketException(e.Message);
            }
        }
        public async Task<Stream> Download(string[] path, string fileName)
        {
            try
            {
                if (_option.S3 != null || !_option.S3.IsValid)
                    throw new BucketException("Not implemented in AppSetting. [ENV]. JSON in session  'AWS' The default settings of the bucket.");

                return await Download(fileName,
                                      _option.S3.Bucket,
                                      path,
                                      _option.S3.Region as RegionEndpoint);
            }
            catch (Exception e)
            {
                throw new BucketException(e.Message);
            }
        }
        public async Task<Stream> Download(string fileName, string bucket, string[] path, RegionEndpoint region)
        {
            using (var client = _client(region))
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = fileName.ToBucketDirectory(path)
                };

                GetObjectResponse response = await client.GetObjectAsync(request);
                MemoryStream stream = new MemoryStream();
                response.ResponseStream.CopyTo(stream);

                return stream;
            }
        }

        public async Task<string> Upload<T>(IFormFile file, bool unique = true)
            where T : IBucketAws
        {
            try
            {
                var config = (T)Activator.CreateInstance(typeof(T));

                return await Upload(config.Bucket,
                                    config.Path,
                                    file,
                                    config.Region,
                                    config.Acl,
                                    unique);
            }
            catch (Exception e)
            {
                throw new BucketException(e.Message);
            }
        }
        public async Task<string> Upload(string[] path, IFormFile file, bool unique = true)
        {
            try
            {
                if (_option.S3 != null || !_option.S3.IsValid)
                    throw new BucketException("Not implemented in AppSetting. [ENV]. JSON in session  'AWS' The default settings of the bucket.");

                return await Upload(_option.S3.Bucket, path,
                             file,
                             _option.S3.Region as RegionEndpoint,
                             _option.S3.Acl as S3CannedACL,
                             unique);
            }
            catch (Exception e)
            {
                throw new BucketException(e.Message);
            }
        }
        public async Task<string> Upload(string bucket, string[] path, IFormFile file, RegionEndpoint region, S3CannedACL acl, bool unique)
        {
            using (var client = _client(region))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var fileName = (unique) ? file.GetUniqueFileName() : file.FileName;

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = bucket,
                        Key = fileName.ToBucketDirectory(path),
                        InputStream = newMemoryStream,
                        CannedACL = acl
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return fileName;
                }
            }
        }
    }
}