using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading;
using System.Threading.Tasks;

namespace Pomelo.EntityFrameworkCore.Lolita.Delete
{
    public class DefaultLolitaDeleteExecutor : ILolitaDeleteExecutor
    {
        public DefaultLolitaDeleteExecutor(ICurrentDbContext CurrentDbContext, ISqlGenerationHelper SqlGenerationHelper, IDbSetFinder DbSetFinder)
        {
            sqlGenerationHelper = SqlGenerationHelper;
            dbSetFinder = DbSetFinder;
            context = CurrentDbContext.Context;
        }

        private ISqlGenerationHelper sqlGenerationHelper;
        private IDbSetFinder dbSetFinder;
        private DbContext context;

        protected virtual string GetTableName<TEntity>()
        {
            string schema = null, table = null;
            var tableAttr = typeof(TEntity).GetTypeInfo().GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                table = tableAttr.Name;
                schema = tableAttr.Schema;
            }
            else
            {
                var setName = dbSetFinder.FindSets(context).SingleOrDefault(x => x.ClrType == typeof(TEntity)).Name;
                if (string.IsNullOrEmpty(setName))
                {
                    throw new ArgumentNullException(typeof(TEntity).Name);
                }
                table = setName;
            }

            if (schema != null)
                return $"{sqlGenerationHelper.DelimitIdentifier(schema)}.{sqlGenerationHelper.DelimitIdentifier(table)}";
            else
                return sqlGenerationHelper.DelimitIdentifier(table);
        }

        public virtual string GenerateSql<TEntity>(IQueryable<TEntity> lolita, RelationalQueryModelVisitor visitor) where TEntity : class, new()
        {
            var sb = new StringBuilder("DELETE FROM ");
            var model = lolita.ElementType;

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
