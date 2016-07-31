using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class SqlServerOperationSqlGenerator : IOperationSqlGenerator
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
                    return $"SET {operation.Field} = '{operation.Field}'+'{{{operation.Index}}}'";
                case "Prepend":
                    return $"SET {operation.Field} = '{{{operation.Index}}}'+'{operation.Field}'";
                case "AddMilliseconds":
                    return $"SET {operation.Field} = DATEADD('ss', {{{operation.Index}}}, {operation.Field})";
                case "AddSeconds":
                    return $"SET {operation.Field} = DATEADD('ms', {{{operation.Index}}}, {operation.Field})";
                case "AddMinutes":
                    return $"SET {operation.Field} = DATEADD('mi', {{{operation.Index}}}, {operation.Field})";
                case "AddHours":
                    return $"SET {operation.Field} = DATEADD('hh', {{{operation.Index}}}, {operation.Field})";
                case "AddDays":
                    return $"SET {operation.Field} = DATEADD('dd', {{{operation.Index}}}, {operation.Field})";
                case "AddMonths":
                    return $"SET {operation.Field} = DATEADD('mm', {{{operation.Index}}}, {operation.Field})";
                case "AddYears":
                    return $"SET {operation.Field} = DATEADD('yy', {{{operation.Index}}}, {operation.Field})";
            }
            return string.Empty;
        }
    }
}
