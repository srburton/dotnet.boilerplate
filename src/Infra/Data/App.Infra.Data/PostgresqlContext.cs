using App.Domain.Interfaces;
using App.Infra.Bootstrap;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Data
{
    public class PostgresqlContext: DbContext, ISingleton<PostgresqlContext>
    {

    }
}
