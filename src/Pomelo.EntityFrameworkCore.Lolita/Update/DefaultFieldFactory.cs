using System;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Internal;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class DefaultFieldFactory : IFieldFactory
    {
        public DefaultFieldFactory(ICurrentDbContext CurrentDbContext, ISqlGenerationHelper SqlGenerationHelper, IDbSetFinder DbSetFinder)
        {
            sqlGenerationHelper = SqlGenerationHelper;
            dbSetFinder = DbSetFinder;
            context = CurrentDbContext.Context;
        }

        private ISqlGenerationHelper sqlGenerationHelper;
        private IDbSetFinder dbSetFinder;
        private DbContext context;

        public virtual string ParseField(SqlFieldInfo field)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(field.Database))
                sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Database))
                    .Append(".");
            if (!string.IsNullOrEmpty(field.Schema))
                sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Schema))
                    .Append(".");
            if (!string.IsNullOrEmpty(field.Table))
                sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Table))
                    .Append(".");
            sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Column));
            return sb.ToString();
        }

        public virtual string ParseTable(SqlFieldInfo field)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(field.Database))
                sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Database))
                    .Append(".");
            if (!string.IsNullOrEmpty(field.Schema))
                sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Schema))
                    .Append(".");
            sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Table));
            return sb.ToString();
        }

        public virtual SqlFieldInfo VisitField<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> exp)
            where TEntity : class,new()
        {
            var ret = new SqlFieldInfo();

            // Getting table name and schema name
            if (exp.Parameters.Count != 1)
            {
                throw new ArgumentException("Too many parameters in the expression.");
            }
            var param = exp.Parameters.Single();
            var tableAttr = param.Type.GetTypeInfo().GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                ret.Table = tableAttr.Name;
                ret.Schema = tableAttr.Schema;
            }
            else
            {
                var setName = dbSetFinder.FindSets(context).SingleOrDefault(x => x.ClrType == typeof(TEntity)).Name;
                if (string.IsNullOrEmpty(setName))
                {
                    throw new ArgumentNullException(typeof(TEntity).Name);
                }
                ret.Table = setName;
            }

            // Getting field name
            var body = exp.Body as MemberExpression;
            if (body == null)
            {
                throw new NotSupportedException(exp.Body.GetType().Name);
            }
            var columnAttr = body.Member.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr != null)
            {
                ret.Column = columnAttr.Name;
            }
            else
            {
                ret.Column = body.Member.Name;
            }

            return ret;
        }
    }
}
