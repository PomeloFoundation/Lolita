using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.Lolita.Update;
using Pomelo.EntityFrameworkCore.Lolita.Delete;

namespace Microsoft.EntityFrameworkCore
{
    public class SqliteLolitaDbOptionExtension : IDbContextOptionsExtension
    {
        public void ApplyServices(IServiceCollection services)
        {
            services
                .AddScoped<ISetFieldSqlGenerator, SqliteSetFieldSqlGenerator>();
        }
    }

    public static class LolitaDbOptionExtensions
    {
        public static DbContextOptionsBuilder UseSqliteLolita(this DbContextOptionsBuilder self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new SqliteLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptionsBuilder<TContext> UseSqliteLolita<TContext>(this DbContextOptionsBuilder<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new SqliteLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions UseSqliteLolita(this DbContextOptions self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new SqliteLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions<TContext> UseSqliteLolita<TContext>(this DbContextOptions<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new SqliteLolitaDbOptionExtension());
            return self;
        }
    }
}