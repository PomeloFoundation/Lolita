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
                    return $"SET {operation.Field} = '{operation.Field}'||'{{{operation.Index}}}'";
                case "Prepend":
                    return $"SET {operation.Field} = '{{{operation.Index}}}'||'{operation.Field}'";
                case "AddMilliseconds":
                    throw new NotImplementedException();
                case "AddSeconds":
                    return $"SET {operation.Field} = date({operation.Field},'NNN.NNNN seconds', {{{operation.Index}}})";
                case "AddMinutes":
                    return $"SET {operation.Field} = date({operation.Field},'NNN minutes', {{{operation.Index}}})";
                case "AddHours":
                    return $"SET {operation.Field} = date({operation.Field},'NNN hours', {{{operation.Index}}})";
                case "AddDays":
                    return $"SET {operation.Field} = date({operation.Field},'NNN days', {{{operation.Index}}})";
                case "AddMonths":
                    return $"SET {operation.Field} = date({operation.Field},'NNN months', {{{operation.Index}}})";
                case "AddYears":
                    return $"SET {operation.Field} = date({operation.Field},'NNN years', {{{operation.Index}}})";
            }
            return string.Empty;
        }
    }
}
