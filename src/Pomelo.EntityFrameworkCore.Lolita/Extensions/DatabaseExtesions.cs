using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;
using Remotion.Linq;

namespace Pomelo.EntityFrameworkCore.Lolita
{
    internal static class DatabaseExtesions
    {
        public static EntityQueryModelVisitor CreateVisitor(this Database self, QueryModel qm)
        {
            var databaseTypeInfo = typeof(Database).GetTypeInfo();
            var _queryCompilationContextFactory = (IQueryCompilationContextFactory)databaseTypeInfo.DeclaredFields.Single(x => x.Name == "_queryCompilationContextFactory").GetValue(self);
            return _queryCompilationContextFactory.Create(async: false).CreateQueryModelVisitor();
        }
    }
}
