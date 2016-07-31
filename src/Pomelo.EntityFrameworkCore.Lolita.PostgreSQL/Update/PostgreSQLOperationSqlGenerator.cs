using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class PostgreSQLSetFieldSqlGenerator : DefaultSetFieldSqlGenerator
    {
        public PostgreSQLSetFieldSqlGenerator(ISqlGenerationHelper x) : base(x) { }

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
                    return $"SET {operation.Field} = '{operation.Field}'||'{{{operation.Index}}}'";
                case "Prepend":
                    return $"SET {operation.Field} = '{{{operation.Index}}}'||'{operation.Field}'";
                case "AddMilliseconds":
                    throw new NotImplementedException();
                case "AddSeconds":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{ operation.Value } Seconds'";
                case "AddMinutes":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Minutes'";
                case "AddHours":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Hours'";
                case "AddDays":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Days'";
                case "AddMonths":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Months'";
                case "AddYears":
                    return $"SET {operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Years'";
            }
            return string.Empty;
        }
    }
}
