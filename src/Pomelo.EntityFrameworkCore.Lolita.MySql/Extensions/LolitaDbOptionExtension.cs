using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.Lolita.Update;
using Pomelo.EntityFrameworkCore.Lolita.Delete;
using Pomelo.EntityFrameworkCore.Lolita.Common;

namespace Microsoft.EntityFrameworkCore
{
    public class MySqlLolitaDbOptionExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo info;

        public DbContextOptionsExtensionInfo Info
        {
            get
            {
                if (info == null)
                {
                    info = new LolitaDbContextOptionsExtensionInfo(this, "Pomelo.EFCore.Lolita.MySql", 478646435);
                }
                return info;
            }
        }


        public void ApplyServices(IServiceCollection services)
        {
            services
                .AddScoped<ISetFieldSqlGenerator, MySqlSetFieldSqlGenerator>();
        }

        public long GetServiceProviderHashCode()
        {
            return 86216188623902;
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }

    public static class LolitaDbOptionExtensions
    {
        public static DbContextOptionsBuilder UseMySqlLolita(this DbContextOptionsBuilder self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new MySqlLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptionsBuilder<TContext> UseMySqlLolita<TContext>(this DbContextOptionsBuilder<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new MySqlLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions UseMySqlLolita(this DbContextOptions self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new MySqlLolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions<TContext> UseMySqlLolita<TContext>(this DbContextOptions<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new MySqlLolitaDbOptionExtension());
            return self;
        }
    }
}