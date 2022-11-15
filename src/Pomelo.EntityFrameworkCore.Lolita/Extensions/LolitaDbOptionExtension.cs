using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.Lolita.Update;
using Pomelo.EntityFrameworkCore.Lolita.Delete;
using Pomelo.EntityFrameworkCore.Lolita.Common;

namespace Microsoft.EntityFrameworkCore
{
    public class LolitaDbOptionExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo info;

        public DbContextOptionsExtensionInfo Info 
        {
            get 
            {
                if (info == null)
                {
                    info = new LolitaDbContextOptionsExtensionInfo(this, "Pomelo.EFCore.Lolita", 563786132);
                }
                return info;
            }
        }

        public void Validate(IDbContextOptions options)
        {
        }

        void IDbContextOptionsExtension.ApplyServices(IServiceCollection services)
        {
            services
                .AddScoped<IFieldParser, DefaultFieldParser>()
                .AddScoped<ISetFieldSqlGenerator, DefaultSetFieldSqlGenerator>()
                .AddScoped<ILolitaUpdateExecutor, DefaultLolitaUpdateExecutor>()
                .AddScoped<ILolitaDeleteExecutor, DefaultLolitaDeleteExecutor>();
        }
    }

    public static class LolitaDbOptionExtensions
    {
        public static DbContextOptionsBuilder UseLolita(this DbContextOptionsBuilder self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptionsBuilder<TContext> UseLolita<TContext>(this DbContextOptionsBuilder<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions UseLolita(this DbContextOptions self)
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            return self;
        }

        public static DbContextOptions<TContext> UseLolita<TContext>(this DbContextOptions<TContext> self) where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)self).AddOrUpdateExtension(new LolitaDbOptionExtension());
            return self;
        }
    }
}