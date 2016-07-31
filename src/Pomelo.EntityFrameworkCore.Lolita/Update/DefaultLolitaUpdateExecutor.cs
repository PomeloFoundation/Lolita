using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class DefaultLolitaUpdateExecutor : ILolitaUpdateExecutor
    {
        public DefaultLolitaUpdateExecutor(ICurrentDbContext CurrentDbContext, ISqlGenerationHelper SqlGenerationHelper, IDbSetFinder DbSetFinder)
        {
            sqlGenerationHelper = SqlGenerationHelper;
            dbSetFinder = DbSetFinder;
            context = CurrentDbContext.Context;
        }

        private ISqlGenerationHelper sqlGenerationHelper;
        private IDbSetFinder dbSetFinder;
        private DbContext context;

        public virtual string GenerateSql<TEntity>(LolitaSetting<TEntity> lolita, RelationalQueryModelVisitor visitor) where TEntity : class, new()
        {
            var sb = new StringBuilder("UPDATE ");
            sb.Append(lolita.Table)
                .AppendLine()
                .Append("SET ")
                .Append(string.Join(", \r\n    ", lolita.Operations))
                .AppendLine()
                .Append(ParseWhere(visitor, lolita.Table))
                .Append(sqlGenerationHelper.StatementTerminator);

            return sb.ToString();
        }

        protected virtual string ParseWhere(RelationalQueryModelVisitor visitor, string Table)
        {
            var sql = visitor.Queries.First().ToString();
            var pos = sql.IndexOf("WHERE");
            if (pos < 0)
                return "";
            return sql.Substring(pos)
                .Replace(sqlGenerationHelper.DelimitIdentifier(visitor.CurrentParameter.Name), Table);
        }

        public virtual int Execute(DbContext db, string sql, object[] param)
        {
            return db.Database.ExecuteSqlCommand(sql, param);
        }

        public Task<int> ExecuteAsync(DbContext db, string sql, CancellationToken cancellationToken = default(CancellationToken), params object[] param)
        {
            return db.Database.ExecuteSqlCommandAsync(sql, cancellationToken, param);
        }
    }
}
