using App.Bootstrap;
using App.Domain.Entities;
using App.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace App.Application.Authentication.Repositories
{
    public class LoginRepository: MysqlProxy, IRepository<LoginRepository>
    {
        protected DbSet<User> _user { get; set; }

        public User Find()
        {
            return _user.FirstOrDefault(x => x.Email.Equals("roberto@teste.com"));
        }
    }
}
