using App.Bootstrap;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Data
{
    public class PostgresqlContext: DbContext, ISingleton<PostgresqlContext>
    {

    }
}
