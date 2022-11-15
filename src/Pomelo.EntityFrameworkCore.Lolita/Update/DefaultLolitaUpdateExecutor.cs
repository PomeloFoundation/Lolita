using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pomelo.EntityFrameworkCore.Lolita.Common;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class DefaultLolitaUpdateExecutor : ILolitaUpdateExecutor
    {
        public DefaultLolitaUpdateExecutor(
            ICurrentDbContext CurrentDbContext,
            ISqlGenerationHelper SqlGenerationHelper, 
            IDbSetFinder DbSetFinder)
        {
            sqlGenerationHelper = SqlGenerationHelper;
            dbSetFinder = DbSetFinder;
            context = CurrentDbContext.Context;
        }

        private ISqlGenerationHelper sqlGenerationHelper;
        private IDbSetFinder dbSetFinder;
        private DbContext context;

        public virtual string GenerateSql<TEntity>(LolitaSetting<TEntity> lolita, IQueryable<TEntity> query) where TEntity : class, new()
        {
            var sb = new StringBuilder("UPDATE ");
            sb.Append(lolita.FullTable)
                .AppendLine()
                .Append("SET ")
                .Append(string.Join($", { Environment.NewLine }    ", lolita.Operations))
                .AppendLine()
                .Append(ParseWhere(query, lolita.ShortTable))
                .Append(sqlGenerationHelper.StatementTerminator);

            return sb.ToString();
        }

        protected virtual string ParseWhere<TEntity>(IQueryable<TEntity> query, string table)
        {
            if (query == null)
                return "";
            var sql = query.ToQueryString();
            var pos = sql.IndexOf("WHERE");
            if (pos < 0)
                return "";

            var line = sql.Split('\n').First(x => x.Contains("FROM") && x.Contains("AS"));
            var splited = line.Split(' ');
            var currentParameter = splited[3].Trim();
            var ret = sql.Substring(pos).Replace(currentParameter, table);
            return ret;
        }

        public virtual int Execute(DbContext db, string sql, object[] param)
        {
            return db.Database.ExecuteSqlRaw(sql, param);
        }

        public Task<int> ExecuteAsync(DbContext db, string sql, CancellationToken cancellationToken = default(CancellationToken), params object[] param)
        {
            return db.Database.ExecuteSqlRawAsync(sql, cancellationToken, param);
        }
    }
}
