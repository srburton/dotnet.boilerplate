using System;
using System.Threading.Tasks;
using App.Application.Authentication.Repositories;
using App.Application.Authentication.Uploads;
using App.Bootstrap;
using App.Domain.Entities;
using App.Infra.Implementation.GeoLocation;
using App.Infra.Integration.Aws;
using Microsoft.AspNetCore.Http;

namespace App.Application.Authentication
{
    public class Login: IApplication<Login>
    {
        readonly S3AwsService _s3;
        readonly LoginRepository _repository;

        public Login(IRepository<LoginRepository> repository,
                     IService<S3AwsService> s3)
        {
            _s3 = (S3AwsService)s3;
            _repository = (LoginRepository)repository;
        }

        public async Task UplaodAsync(IFormFile file)
        {
            var name  = await _s3.Upload<AvatarUpload>(file);

            var content = await _s3.Download<AvatarUpload>(name);            

            await _s3.Delete<AvatarUpload>(name);
        }

        public User Profile()
        {
            return _repository.Find();
        }
    }
}
