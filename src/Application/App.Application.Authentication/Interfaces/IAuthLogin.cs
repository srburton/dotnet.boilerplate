using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Application.Authentication.Interfaces
{
    public interface IAuthLogin<T>
    {
        object GetToken(string email, string password);
        Task UploadAsync(IFormFile file);
    }
}
