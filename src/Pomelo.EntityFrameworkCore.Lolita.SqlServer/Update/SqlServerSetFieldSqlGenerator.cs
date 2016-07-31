using Microsoft.EntityFrameworkCore.Storage;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class SqlServerSetFieldSqlGenerator : DefaultSetFieldSqlGenerator
    {
        public SqlServerSetFieldSqlGenerator(ISqlGenerationHelper x) : base(x) { }

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
                    return $"{operation.Field} = {operation.Field}+{{{operation.Index}}}";
                case "Prepend":
                    return $"{operation.Field} = {{{operation.Index}}}+{operation.Field}";
                case "AddMilliseconds":
                    return $"{operation.Field} = DATEADD(ms, {{{operation.Index}}}, {operation.Field})";
                case "AddSeconds":
                    return $"{operation.Field} = DATEADD(ss, {{{operation.Index}}}, {operation.Field})";
                case "AddMinutes":
                    return $"{operation.Field} = DATEADD(mi, {{{operation.Index}}}, {operation.Field})";
                case "AddHours":
                    return $"{operation.Field} = DATEADD(hh, {{{operation.Index}}}, {operation.Field})";
                case "AddDays":
                    return $"{operation.Field} = DATEADD(dd, {{{operation.Index}}}, {operation.Field})";
                case "AddMonths":
                    return $"{operation.Field} = DATEADD(mm, {{{operation.Index}}}, {operation.Field})";
                case "AddYears":
                    return $"{operation.Field} = DATEADD(yy, {{{operation.Index}}}, {operation.Field})";
            }
            return string.Empty;
        }
    }
}
