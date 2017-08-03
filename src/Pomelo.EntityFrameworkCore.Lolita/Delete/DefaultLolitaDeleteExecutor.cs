using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Pomelo.EntityFrameworkCore.Lolita.Delete
{
    public class DefaultLolitaDeleteExecutor : ILolitaDeleteExecutor
    {
        private static FieldInfo EntityTypesField = typeof(Model).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_entityTypes");

        public DefaultLolitaDeleteExecutor(ICurrentDbContext CurrentDbContext, ISqlGenerationHelper SqlGenerationHelper, IDbSetFinder DbSetFinder)
        {
            sqlGenerationHelper = SqlGenerationHelper;
            dbSetFinder = DbSetFinder;
            context = CurrentDbContext.Context;
        }

        private ISqlGenerationHelper sqlGenerationHelper;
        private IDbSetFinder dbSetFinder;
        private DbContext context;

        protected virtual string ParseTableName(EntityType type)
        {
            string tableName;
            var anno = type.FindAnnotation("Relational:TableName");
            if (anno != null)
                tableName = anno.Value.ToString();
            else
            {
                var prop = dbSetFinder.FindSets(context).SingleOrDefault(y => y.ClrType == type.ClrType);
                if (!prop.Equals(default(DbSetProperty)))
                    tableName = prop.Name;
                else
                    tableName = type.ClrType.Name;
            }
            return tableName;
        }

        protected virtual string GetTableName<TEntity>()
        {
            string schema = null;
            var entities = (IDictionary<string, EntityType>)EntityTypesField.GetValue(context.Model);
            var et = entities.Where(x => x.Value.ClrType == typeof(TEntity)).Single().Value;

            var tableAttr = typeof(TEntity).GetTypeInfo().GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
                schema = tableAttr.Schema;
            if (schema != null)
                return $"{sqlGenerationHelper.DelimitIdentifier(schema)}.{sqlGenerationHelper.DelimitIdentifier(ParseTableName(et))}";
            else
                return sqlGenerationHelper.DelimitIdentifier(ParseTableName(et));
        }

        public virtual string GenerateSql<TEntity>(IQueryable<TEntity> lolita) where TEntity : class, new()
        {
            var sb = new StringBuilder("DELETE FROM ");
            var model = lolita.ElementType;
            var visitor = lolita.CompileQuery();

            var table = GetTableName<TEntity>();
            sb.Append(table)
                .AppendLine()
                .Append(ParseWhere(visitor, table))
                .Append(sqlGenerationHelper.StatementTerminator);

            return sb.ToString();
        }

        protected virtual string ParseWhere(RelationalQueryModelVisitor visitor, string Table)
        {
            if (visitor == null || visitor.Queries.Count == 0)
                return "";
            var sql = visitor.Queries.First().ToString();
            var pos = sql.IndexOf("WHERE");
            if (pos < 0)
                return "";
            return sql.Substring(pos)
                .Replace(sqlGenerationHelper.DelimitIdentifier(visitor.CurrentParameter.Name), Table);
        }

        public virtual int Execute(DbContext db, string sql)
        {
            return db.Database.ExecuteSqlCommand(sql);
        }

        public Task<int> ExecuteAsync(DbContext db, string sql, CancellationToken cancellationToken = default(CancellationToken))
        {
            return db.Database.ExecuteSqlCommandAsync(sql, cancellationToken);
        }
    }
}
