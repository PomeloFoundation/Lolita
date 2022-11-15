using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.Lolita.Update;
using Pomelo.EntityFrameworkCore.Lolita.Delete;
using Pomelo.EntityFrameworkCore.Lolita.Common;

namespace Microsoft.EntityFrameworkCore
{
    public class SqlServerLolitaDbOptionExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo info;

        public DbContextOptionsExtensionInfo Info
        {
            get
            {
                if (info == null)
                {
                    info = new LolitaDbContextOptionsExtensionInfo(this, "Pomelo.EFCore.Lolita.SQLServer", 82356113);
                }
                return info;
            }
        }

        public void ApplyServices(IServiceCollection services)
        {
            services
                .AddScoped<ISetFieldSqlGenerator, SqlServerSetFieldSqlGenerator>();
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }

    public static class LolitaDbOptionExtensions
    {
        public static DbContextOptionsBuilder UseSqlServerLolita(this DbContextOptionsBuilder self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new SqlServerLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptionsBuilder<TContext> UseSqlServerLolita<TContext>(this DbContextOptionsBuilder<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new SqlServerLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions UseSqlServerLolita(this DbContextOptions self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new SqlServerLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions<TContext> UseSqlServerLolita<TContext>(this DbContextOptions<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new SqlServerLolitaDbOptionExtension());
            return self;
        }
    }
}