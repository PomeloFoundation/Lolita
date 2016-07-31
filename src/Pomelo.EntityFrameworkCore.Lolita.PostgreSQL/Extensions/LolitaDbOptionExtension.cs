using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.Lolita.Update;
using Pomelo.EntityFrameworkCore.Lolita.Delete;

namespace Microsoft.EntityFrameworkCore
{
    public class PostgreSQLLolitaDbOptionExtension : IDbContextOptionsExtension
    {
        public void ApplyServices(IServiceCollection services)
        {
            services
                .AddScoped<IOperationSqlGenerator, PostgreSQLOperationSqlGenerator>();
        }
    }

    public static class LolitaDbOptionExtensions
    {
        public static DbContextOptionsBuilder UsePostgreSQLLolita(this DbContextOptionsBuilder self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new PostgreSQLLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions UsePostgreSQLLolita(this DbContextOptions self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new PostgreSQLLolitaDbOptionExtension());
            return self;
        }
    }
}