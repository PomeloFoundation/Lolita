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
                case "WithSQL":
                    return $"{ operation.Field } = { operation.Value }";
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
                    return $"{operation.Field} = {operation.Field} || {{{operation.Index}}}";
                case "Prepend":
                    return $"{operation.Field} = {{{operation.Index}}} || {operation.Field}";
                case "AddMilliseconds":
                    throw new NotImplementedException();
                case "AddSeconds":
                    return $"{operation.Field} = {operation.Field} + INTERVAL '{ operation.Value } Seconds'";
                case "AddMinutes":
                    return $"{operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Minutes'";
                case "AddHours":
                    return $"{operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Hours'";
                case "AddDays":
                    return $"{operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Days'";
                case "AddMonths":
                    return $"{operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Months'";
                case "AddYears":
                    return $"{operation.Field} = {operation.Field} + INTERVAL '{ operation.Value }  Years'";
            }
            return string.Empty;
        }
    }
}
