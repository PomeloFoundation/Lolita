using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pomelo.EntityFrameworkCore.Lolita
{
    internal static class ReflectionCommon
    {
        public static FieldInfo QueryCompilerOfEntityQueryProvider = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        public static TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();
        public static MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");
        public static PropertyInfo NodeTypeProvider = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");
        public static FieldInfo DataBaseOfQueryCompiler = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        public static PropertyInfo QueriesOfRelationalQueryModelVisitor = typeof(RelationalQueryModelVisitor).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Queries");
        public static FieldInfo QueryCompilationContextFactoryOfDatabase = typeof(Database).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompilationContextFactory");
        public static FieldInfo DbContextOfQueryCompilationContextFactory = typeof(QueryCompilationContextFactory).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_context");
    }
}
