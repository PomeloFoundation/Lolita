using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.Lolita.Update;
using Pomelo.EntityFrameworkCore.Lolita.Delete;
using Pomelo.EntityFrameworkCore.Lolita.Common;

namespace Microsoft.EntityFrameworkCore
{
    public class PostgreSQLLolitaDbOptionExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo info;

        public DbContextOptionsExtensionInfo Info
        {
            get
            {
                if (info == null)
                {
                    info = new LolitaDbContextOptionsExtensionInfo(this, "Pomelo.EFCore.Lolita.PostgreSQL", 573829023);
                }
                return info;
            }
        }

        public void ApplyServices(IServiceCollection services)
        {
            services
                .AddScoped<ISetFieldSqlGenerator, PostgreSQLSetFieldSqlGenerator>();
        }

        public void Validate(IDbContextOptions options)
        {
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

        public static DbContextOptionsBuilder<TContext> UsePostgreSQLLolita<TContext>(this DbContextOptionsBuilder<TContext> self) where TContext : DbContext
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

        public static DbContextOptions<TContext> UsePostgreSQLLolita<TContext>(this DbContextOptions<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new PostgreSQLLolitaDbOptionExtension());
            return self;
        }
    }
}