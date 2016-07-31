using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class PostgreSQLOperationSqlGenerator : IOperationSqlGenerator
    {
        public virtual string TranslateToSql(OperationInfo operation)
        {
            switch (operation.Type)
            {
                case "WIthValue":
                    return $"SET {operation.Field} = {{{ operation.Index }}}";
                case "Plus":
                    return $"SET {operation.Field} = {operation.Field} + {{{ operation.Index }}}";
                case "Subtract":
                    return $"SET {operation.Field} = {operation.Field} - {{{ operation.Index }}}";
                case "Multiply":
                    return $"SET {operation.Field} = {operation.Field} * {{{ operation.Index }}}";
                case "Divide":
                    return $"SET {operation.Field} = {operation.Field} / {{{ operation.Index }}}";
                case "Mod":
                    return $"SET {operation.Field} = {operation.Field} % {{{ operation.Index }}}";
                case "Append":
                    return $"SET {operation.Field} = '{operation.Field}'||'{{{operation.Index}}}'";
                case "Prepend":
                    return $"SET {operation.Field} = '{{{operation.Index}}}'||'{operation.Field}'";
                case "AddMilliseconds":
                    throw new NotImplementedException();
                case "AddSeconds":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{{{operation.Index}}} Seconds'";
                case "AddMinutes":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{{{operation.Index}}} Minutes'";
                case "AddHours":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{{{operation.Index}}} Hours'";
                case "AddDays":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{{{operation.Index}}} Days'";
                case "AddMonths":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{{{operation.Index}}} Months '";
                case "AddYears":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{{{operation.Index}}} Years'";
            }
            return string.Empty;
        }
    }
}
