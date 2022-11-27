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
using System.Collections.Generic;

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
            var where = ParseWhere(query, lolita.ShortTable, out var currentParameter);
            sb.Append(lolita.FullTable)
                .AppendLine()
                .Append(ParseInnerJoins(query, lolita.ShortTable, currentParameter))
                .Append("SET ")
                .Append(string.Join($", { Environment.NewLine }    ", lolita.Operations))
                .AppendLine()
                .Append(where)
                .Append(sqlGenerationHelper.StatementTerminator);

            return sb.ToString();
        }

        protected virtual string ParseInnerJoins<TEntity>(IQueryable<TEntity> query, string table, string currentParameter)
        {
            if (query == null)
            {
                return "";
            }
            var sql = query.ToQueryString();
            var innerJoinPos = sql.IndexOf("INNER JOIN");
            if (innerJoinPos < 0)
            {
                return "";
            }
            var wherePos = sql.IndexOf("WHERE");

            if (wherePos >= 0)
            {
                return sql
                    .Substring(innerJoinPos, wherePos - innerJoinPos)
                    .Replace(currentParameter, table);
            }
            else
            {
                return sql
                    .Substring(innerJoinPos)
                    .Replace(currentParameter, table);
            }
        }

        protected virtual string ParseWhere<TEntity>(IQueryable<TEntity> query, string table, out string currentParameter)
        {
            if (query == null)
            {
                currentParameter = null;
                return "";
            }
            var sql = query.ToQueryString();
            var pos = sql.IndexOf("WHERE");
            if (pos < 0)
            {
                currentParameter = null;
                return "";
            }

            var line = sql.Split('\n').First(x => x.Contains("FROM") && x.Contains("AS"));
            var splited = line.Split(' ');
            currentParameter = splited[3].Trim();
            var ret = sql.Substring(pos).Replace(currentParameter, table);
            return ret;
        }

        public virtual int Execute(DbContext db, string sql, object[] param)
        {
            return db.Database.ExecuteSqlRaw(sql, param);
        }

        public Task<int> ExecuteAsync(DbContext db, string sql, object[] param, CancellationToken cancellationToken = default(CancellationToken))
        {
            return db.Database.ExecuteSqlRawAsync(sql, param, cancellationToken);
        }
    }
}
