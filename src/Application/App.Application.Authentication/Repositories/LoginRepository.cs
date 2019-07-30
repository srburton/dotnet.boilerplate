using App.Domain.Entities;
using App.Bootstrap;
using App.Infra.Data;
using System.Linq;

namespace App.Application.Authentication.Repositories
{
    public class LoginRepository
    {
        readonly MysqlContext _context;

        public LoginRepository(ISingleton<MysqlContext> context)
        {
            _context = (MysqlContext)context;           
        }

        public User Find()
        {
            return _context.Set<User>()
                           .FirstOrDefault(x => x.Id > 0);
        }
    }
}
