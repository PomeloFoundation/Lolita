using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class MySqlSetFieldSqlGenerator : DefaultSetFieldSqlGenerator
    {
        public MySqlSetFieldSqlGenerator(ISqlGenerationHelper x) : base(x) { }

        public override string TranslateToSql(SetFieldInfo operation)
        {
            switch (operation.Type)
            {
                case "WithValue":
                    return $"{operation.Field} = {{{ operation.Index }}}";
                case "Plus":
                    return $"{operation.Field} = {operation.Field} + {{{ operation.Index }}}";
                case "Subtract":
                    return $"{operation.Field} = {operation.Field} - {{{ operation.Index }}}";
                case "Multiply":
                    return $"{operation.Field} = {operation.Field} * {{{ operation.Index }}}";
                case "Divide":
                    return $"{operation.Field} = {operation.Field} / {{{ operation.Index }}}";
                case "Mod":
                    return $"{operation.Field} = {operation.Field} % {{{ operation.Index }}}";
                case "Append":
                    return $"{operation.Field} = CONCAT({operation.Field}, {{{operation.Index}}})";
                case "Prepend":
                    return $"{operation.Field} = CONCAT({{{operation.Index}}}, {operation.Field})";
                case "AddMilliseconds":
                    return $"{operation.Field} = DATE_ADD({operation.Field}, INTERVAL {{{operation.Index}}} microsecond)";
                case "AddSeconds":
                    return $"{operation.Field} = DATE_ADD({operation.Field}, INTERVAL {{{operation.Index}}} second)";
                case "AddMinutes":
                    return $"{operation.Field} = DATE_ADD({operation.Field}, INTERVAL {{{operation.Index}}} minute)";
                case "AddHours":
                    return $"{operation.Field} = DATE_ADD({operation.Field}, INTERVAL {{{operation.Index}}} hour)";
                case "AddDays":
                    return $"{operation.Field} = DATE_ADD({operation.Field}, INTERVAL {{{operation.Index}}} day)";
                case "AddMonths":
                    return $"{operation.Field} = DATE_ADD({operation.Field}, INTERVAL {{{operation.Index}}} month)";
                case "AddYears":
                    return $"{operation.Field} = DATE_ADD({operation.Field}, INTERVAL {{{operation.Index}}} year)";
            }
            return string.Empty;
        }
    }
}
