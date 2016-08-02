using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class DefaultSetFieldSqlGenerator : ISetFieldSqlGenerator
    {
        public DefaultSetFieldSqlGenerator(ISqlGenerationHelper SqlGenerationHelper)
        {
            sqlGenerationHelper = SqlGenerationHelper;
        }

        protected ISqlGenerationHelper sqlGenerationHelper;

        public virtual string TranslateToSql(SetFieldInfo operation)
        {
            switch(operation.Type)
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
                    throw new NotSupportedException("Relational field setter does not support this operation.");
                case "Prepend":
                    throw new NotSupportedException("Relational field setter does not support this operation.");
                case "AddMilliseconds":
                    throw new NotSupportedException("Relational field setter does not support this operation.");
                case "AddSeconds":
                    throw new NotSupportedException("Relational field setter does not support this operation.");
                case "AddMinutes":
                    throw new NotSupportedException("Relational field setter does not support this operation.");
                case "AddHours":
                    throw new NotSupportedException("Relational field setter does not support this operation.");
                case "AddDays":
                    throw new NotSupportedException("Relational field setter does not support this operation.");
                case "AddMonths":
                    throw new NotSupportedException("Relational field setter does not support this operation.");
                case "AddYears":
                    throw new NotSupportedException("Relational field setter does not support this operation.");
            }
            return string.Empty;
        }
    }
}
