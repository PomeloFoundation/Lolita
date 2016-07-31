using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class SqliteSetFieldSqlGenerator : DefaultSetFieldSqlGenerator
    {
        public SqliteSetFieldSqlGenerator(ISqlGenerationHelper x) : base(x) { }

        public override string TranslateToSql(SetFieldInfo operation)
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
                    return $"SET {operation.Field} = {operation.Field} || {{{operation.Index}}}";
                case "Prepend":
                    return $"SET {operation.Field} = {{{operation.Index}}} || {operation.Field}";
                case "AddMilliseconds":
                    throw new NotSupportedException("Sqlite does not support million seconds operation of a datetime type.");
                case "AddSeconds":
                    if (Convert.ToInt64(operation.Value) > 0)
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '+{ operation.Value } SECONDS')";
                    else
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '{ operation.Value } SECONDS')";
                case "AddMinutes":
                    if (Convert.ToInt64(operation.Value) > 0)
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '+{ operation.Value } MINUTES')";
                    else
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '{ operation.Value } MINUTES')";
                case "AddHours":
                    if (Convert.ToInt64(operation.Value) > 0)
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '+{ operation.Value } HOURS')";
                    else
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '{ operation.Value } HOURS')";
                case "AddDays":
                    if (Convert.ToInt64(operation.Value) > 0)
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '+{ operation.Value } DAYS')";
                    else
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '{ operation.Value } DAYS')";
                case "AddMonths":
                    if (Convert.ToInt64(operation.Value) > 0)
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '+{ operation.Value } MONTHS')";
                    else
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '{ operation.Value } MONTHS')";
                case "AddYears":
                    if (Convert.ToInt64(operation.Value) > 0)
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '+{ operation.Value } YEARS')";
                    else
                        return $"SET {operation.Field} = DATETIME({operation.Field}, '{ operation.Value } YEARS')";
            }
            return string.Empty;
        }
    }
}
