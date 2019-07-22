using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace App.Infra.Integration.Aws.Extensions
{
    internal static class DynamicExtension
    {
        public static string GetUniqueFileName(this IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            var unique = Guid.NewGuid()
                             .ToString()
                             .Replace("-", string.Empty);

            return $"{unique}{extension}";
        }

        public static string ToBucketDirectory(this string fileName, string[] path)
            => $"{string.Join("/", path ?? new string[] { })}/{fileName}";

        public static bool IsSuccess(this HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.Accepted:
                    return true;
                default:
                    return false;
            }
        }
    }
}
