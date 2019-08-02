using App.Bootstrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Infra.Data
{
    public class MysqlProxy : DbContext
    {
        readonly IConfiguration _configuration = Ioc.Get<IConfiguration>();

        public MysqlProxy() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connection = _configuration.GetSection("connectionString")
                                               .GetValue<string>("mysql");

                optionsBuilder.UseMySql(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => base.OnModelCreating(modelBuilder);
    }
}
