using App.Bootstrap;
using Microsoft.AspNetCore.Mvc;
using App.Application.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]    
    public class ValuesController : ControllerBase
    {
        readonly Login _login;

        public ValuesController(IApplication<Login> login)
        {
            _login = (Login)login;
        }

        public IActionResult Get()
        {            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm(Name = "avatar")] IFormFile avatar)
        {
            await _login.UplaodAsync(avatar);

            return Ok();
        }
    }
}
