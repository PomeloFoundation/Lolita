using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public interface IFieldParser
    {
        SqlFieldInfo VisitField<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> exp) where TEntity: class, new();

        string ParseField(SqlFieldInfo field);

        string ParseFullTable(SqlFieldInfo field);

        string ParseShortTable(SqlFieldInfo field);
    }
}
