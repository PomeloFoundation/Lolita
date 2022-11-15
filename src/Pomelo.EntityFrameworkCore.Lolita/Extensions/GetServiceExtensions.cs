using System;
using System.Linq;
using Pomelo.EntityFrameworkCore.Lolita;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.Lolita.Update;
using static System.Formats.Asn1.AsnWriter;

namespace Microsoft.EntityFrameworkCore
{
    internal static class GetServiceExtensions
    {
        public static TService GetService<TService>(this IQueryable self)
        {
            if (self.GetType().GetTypeInfo().GetGenericTypeDefinition() == typeof(EntityQueryable<>))
            {
                var queryCompiler = (QueryCompiler)ReflectionCommon.QueryCompilerOfEntityQueryProvider.GetValue(self.Provider);
                var queryContextFactory = (RelationalQueryContextFactory)ReflectionCommon.QueryContextFactoryOfQueryCompiler.GetValue(queryCompiler);
                var dependencies = (QueryContextDependencies)ReflectionCommon.DependenciesOfQueryContextFactory.GetValue(queryContextFactory);
                var dbContext = dependencies.CurrentContext.Context;
                var serviceScope = (IServiceScope)ReflectionCommon.ServiceScopeOfDbContext.GetValue(dbContext);
                return serviceScope.ServiceProvider.GetService<TService>();
            }
            else if (self.GetType().GetTypeInfo().GetGenericTypeDefinition() == typeof(InternalDbSet<>))
            {
                var dbContext = (DbContext)self.GetType().GetTypeInfo().DeclaredFields.Single(x => x.Name == "_context").GetValue(self);
                var serviceScope = (IServiceScope)ReflectionCommon.ServiceScopeOfDbContext.GetValue(dbContext);
                return serviceScope.ServiceProvider.GetService<TService>();
            }
            else
            {
                throw new NotSupportedException(self.GetType().Name);
            }
        }
    }
}
