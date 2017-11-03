using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    public class DataAnnotationDefaultSchemaContext : DefaultSchemaContext
    {
        public DataAnnotationDefaultSchemaContext(string defaultSchemaName)
            : base(defaultSchemaName)
        { }

        public DbSet<Product> Products { get; set; }
    }
}
