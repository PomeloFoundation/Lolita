using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.Lolita.Update;
using Pomelo.EntityFrameworkCore.Lolita.Delete;

namespace Microsoft.EntityFrameworkCore
{
    public class LolitaDbOptionExtension : IDbContextOptionsExtension
    {
        public string LogFragment => "Pomelo.EFCore.Lolita";

        public bool ApplyServices(IServiceCollection services)
        {
            services
                .AddScoped<IFieldParser, DefaultFieldParser>()
                .AddScoped<ISetFieldSqlGenerator, DefaultSetFieldSqlGenerator>()
                .AddScoped<ILolitaUpdateExecutor, DefaultLolitaUpdateExecutor>()
                .AddScoped<ILolitaDeleteExecutor, DefaultLolitaDeleteExecutor>();

            return true;
        }

        public long GetServiceProviderHashCode()
        {
            return 86216188623901;
        }

        public void Validate(IDbContextOptions options)
        {
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