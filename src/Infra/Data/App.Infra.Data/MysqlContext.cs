using App.Domain.Entities;
using App.Domain.Interfaces;
using App.Bootstrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Infra.Data
{
    public class MysqlContext: DbContext, ISingleton<MysqlContext>
    {
        readonly IConfiguration _configuration;

        public DbSet<User> User { get; set; }

        public MysqlContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

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
