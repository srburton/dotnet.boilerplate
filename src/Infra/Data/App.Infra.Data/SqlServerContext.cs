using App.Infra.Bootstrap;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Data
{
    public class SqlServerContext : DbContext, ISingleton<SqlServerContext>
    {

    }
}
