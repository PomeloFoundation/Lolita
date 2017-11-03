using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Pomelo.EntityFrameworkCore.Lolita
{
    public class LolitaSetting<TEntity> 
        where TEntity : class
    {
        public IQueryable<TEntity> Query { get; set; }

        public string FullTable { get; set; }

        public string ShortTable { get; set; }

        public IList<string> Operations { get; set; } = new List<string>();

        public IList<object> Parameters { get; set; } = new List<object>();

        public TService GetService<TService>() => Query.GetService<TService>();
    }
}
