using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Pomelo.EntityFrameworkCore.Lolita.Common;
using Microsoft.EntityFrameworkCore.Metadata;

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

        protected virtual string ParseTableName(IEntityType type)
        {
            string tableName;
            var anno = type.FindAnnotation("Relational:TableName");
            if (anno != null)
                tableName = anno.Value?.ToString();
            else
            {
                var prop = dbSetFinder.FindSets(context.GetType()).SingleOrDefault(y => y.ClrType == type.ClrType);
                if (!prop.Equals(default(DbSetProperty)))
                    tableName = prop.Name;
                else
                    tableName = type.ClrType.Name;
            }
            return tableName;
        }

        protected virtual string GetTableName(IEntityType et)
        {
            return sqlGenerationHelper.DelimitIdentifier(ParseTableName(et));
        }

        protected virtual string GetFullTableName(IEntityType et)
        {
            string schema = null;

            // first, try to get schema from fluent API or data annotation
            IAnnotation anno = et.FindAnnotation("Relational:Schema");
            if (anno != null)
                schema = anno.Value?.ToString();
            if (schema == null)
            {
                // otherwise, try to get schema from context default
                anno = context.Model.FindAnnotation("Relational:DefaultSchema");
                if (anno != null)
                    schema = anno.Value?.ToString();
            }
            // TODO: ideally, switch to `et.Relational().Schema`, covering all cases
            if (schema != null)
                return $"{sqlGenerationHelper.DelimitIdentifier(schema)}.{sqlGenerationHelper.DelimitIdentifier(ParseTableName(et))}";
            else
                return sqlGenerationHelper.DelimitIdentifier(ParseTableName(et));
        }

        public virtual string GenerateSql<TEntity>(IQueryable<TEntity> lolita, out IEnumerable<object> parameters) where TEntity : class, new()
        {
            var executed = lolita.Provider.Execute<IEnumerable>(lolita.Expression);
            var relationalQueryContext = executed.GetType().GetRuntimeFields().FirstOrDefault(x => x.Name == "_relationalQueryContext")?.GetValue(executed);
            if (relationalQueryContext == null)
            {
                throw new InvalidOperationException("The query is not a EF Core query.");
            }

            var _parameters = (Dictionary<string, object>)relationalQueryContext.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "ParameterValues")?.GetValue(relationalQueryContext);
            parameters = _parameters.Values;

            var sb = new StringBuilder();
            var model = lolita.ElementType;

            var entities = context.Model.GetEntityTypes();
            var et = entities.Where(x => x.ClrType == typeof(TEntity)).Single();

            var table = GetTableName(et);
            var fullTable = GetFullTableName(et);
            var where = ParseWhere(lolita, table, out var currentParameter);
            var innerJoins = ParseInnerJoins(lolita, table, currentParameter, out var joinnedTables);
            sb.Append("DELETE ")
                .Append(string.Join(", ", new[] { fullTable }.Concat(joinnedTables) ))
                .Append(" FROM ")
                .Append(fullTable)
                .AppendLine()
                .Append(innerJoins)
                .Append(where)
                .Append(sqlGenerationHelper.StatementTerminator);

            var ret = sb.ToString();
            var i = 0;
            foreach (var param in _parameters)
            {
                var appendIndex = i;
                var src = sqlGenerationHelper.GenerateParameterNamePlaceholder(param.Key);
                ret = ret.Replace(src, $"{{{appendIndex}}}");
                ++i;
            }

            return ret;
        }

        protected virtual string ParseInnerJoins<TEntity>(IQueryable<TEntity> query, string table, string currentParameter, out IEnumerable<string> joinnedTables)
        {
            var _joinnedTables = new List<string>();
            joinnedTables = _joinnedTables;
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
                sql = sql
                    .Substring(innerJoinPos, wherePos - innerJoinPos)
                    .Replace(currentParameter, table);
            }
            else
            {
                sql = sql
                    .Substring(innerJoinPos)
                    .Replace(currentParameter, table);
            }

            var splited = sql
                .Replace("\r", "")
                .Replace("\n", " ")
                .Split(' ');

            var asIndexes = new List<int>();
            for (var i = 0; i < splited.Length; ++i)
            {
                if (splited[i].ToUpper() == "AS")
                {
                    asIndexes.Add(i);
                }
            }

            foreach (var index in asIndexes)
            {
                if (index + 1 < splited.Length)
                {
                    _joinnedTables.Add(splited[index + 1]);
                }
            }

            return sql;
        }

        protected virtual string ParseWhere<TEntity>(IQueryable<TEntity> query, string table, out string currentParameter)
        {
            currentParameter = null;
            if (query == null)
                return "";
            var sql = query.ToQueryString();
            var pos = sql.IndexOf("WHERE");
            if (pos < 0)
                return "";

            var line = sql.Split('\n').First(x => x.Contains("FROM") && x.Contains("AS"));
            var splited = line.Split(' ');
            var _currentParameter = splited[3].Trim();
            currentParameter = _currentParameter;
            var ret = sql.Substring(pos).Replace(currentParameter, table);
            return ret;
        }

        public virtual int Execute(DbContext db, string sql, object[] parameters)
        {
            return db.Database.ExecuteSqlRaw(sql, parameters);
        }

        public Task<int> ExecuteAsync(DbContext db, string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }
    }
}
