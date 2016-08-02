using System;
using System.Linq;
using Pomelo.EntityFrameworkCore.Lolita;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public static class GetServiceExtensions
    {
        public static TService GetService<TService>(this IQueryable self)
        {
            if (self.GetType().GetTypeInfo().GetGenericTypeDefinition() == typeof(EntityQueryable<>))
            {
                var queryCompiler = (QueryCompiler)ReflectionCommon.QueryCompilerOfEntityQueryProvider.GetValue(self.Provider);
                var database = (RelationalDatabase)ReflectionCommon.DataBaseOfQueryCompiler.GetValue(queryCompiler);
                var qccf = (QueryCompilationContextFactory)ReflectionCommon.QueryCompilationContextFactoryOfDatabase.GetValue(database);
                var context = (DbContext)ReflectionCommon.DbContextOfQueryCompilationContextFactory.GetValue(qccf);
                return context.GetService<TService>();
            }
            else if (self.GetType().GetTypeInfo().GetGenericTypeDefinition() == typeof(InternalDbSet<>))
            {
                var context = (DbContext)self.GetType().GetTypeInfo().DeclaredFields.Single(x => x.Name == "_context").GetValue(self);
                return context.GetService<TService>();
            }
            else
            {
                throw new NotSupportedException(self.GetType().Name);
            }
        }
    }
}
