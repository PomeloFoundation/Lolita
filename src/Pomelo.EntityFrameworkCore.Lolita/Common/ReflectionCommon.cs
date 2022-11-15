using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pomelo.EntityFrameworkCore.Lolita
{
    internal static class ReflectionCommon
    {
        public static FieldInfo QueryCompilerOfEntityQueryProvider => typeof(EntityQueryProvider).GetRuntimeFields().First(x => x.Name == "_queryCompiler");
        public static FieldInfo QueryContextFactoryOfQueryCompiler => typeof(QueryCompiler).GetRuntimeFields().Single(x => x.Name == "_queryContextFactory");
        public static PropertyInfo DependenciesOfQueryContextFactory => typeof(RelationalQueryContextFactory).GetRuntimeProperties().Single(x => x.Name == "Dependencies");
        public static FieldInfo ServiceScopeOfDbContext => typeof(DbContext).GetRuntimeFields().Single(x => x.Name == "_serviceScope");
        //public static PropertyInfo RootProviderOfServiceScope => typeof()


        public static PropertyInfo DependenciesOfDatabase => typeof(Database).GetRuntimeProperties().Single(x => x.Name == "Dependencies");
        public static TypeInfo QueryCompilerTypeInfo => typeof(QueryCompiler).GetTypeInfo();
        public static MethodInfo CreateQueryParserMethod => QueryCompilerTypeInfo.DeclaredMethods.Single(x => x.Name == "CreateQueryParser");
        public static PropertyInfo NodeTypeProvider => QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");
        public static FieldInfo DependenciesOfQueryCompilerContextFactory => typeof(QueryCompilationContextFactory).GetRuntimeFields().Single(x => x.Name == "_dependencies");
    }
}
