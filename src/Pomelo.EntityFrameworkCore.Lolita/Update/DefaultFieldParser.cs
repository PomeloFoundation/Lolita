using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class DefaultFieldParser : IFieldParser
    {
        private static FieldInfo EntityTypesField = typeof(Model).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_entityTypes");

        public DefaultFieldParser(ICurrentDbContext CurrentDbContext, ISqlGenerationHelper SqlGenerationHelper, IDbSetFinder DbSetFinder)
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
            if (!string.IsNullOrEmpty(field.Table))
                sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Table))
                    .Append(".");
            sb.Append(sqlGenerationHelper.DelimitIdentifier(field.Column));
            return sb.ToString();
        }

        public virtual string ParseFullTable(SqlFieldInfo field)
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

        public virtual string ParseShortTable(SqlFieldInfo field)
        {
            return sqlGenerationHelper.DelimitIdentifier(field.Table);
        }

        protected virtual string GetTableName(EntityType type)
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

        protected virtual string GetSchemaName(EntityType type)
        {
            string schema = null;

            // first, try to get schema from fluent API or data annotation
            IAnnotation anno = type.FindAnnotation("Relational:Schema");
            if (anno != null)
                schema = anno.Value.ToString();
            if (schema == null)
            {
                // otherwise, try to get schema from context default
                anno = context.Model.FindAnnotation("Relational:DefaultSchema");
                if (anno != null)
                    schema = anno.Value.ToString();
            }
            // TODO: ideally, switch to `et.Relational().Schema`, to cover all cases at once

            return schema;
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
            var entities = (IDictionary<string, EntityType>)EntityTypesField.GetValue(context.Model);
            var et = entities.Where(x => x.Value.ClrType == typeof(TEntity)).Single().Value;
            ret.Table = GetTableName(et);
            ret.Schema = GetSchemaName(et);

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
