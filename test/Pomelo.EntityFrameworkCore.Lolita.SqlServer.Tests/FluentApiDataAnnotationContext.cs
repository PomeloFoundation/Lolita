using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    public class FluentApiDataAnnotationContext : DataAnnotationContext
    {
        public FluentApiDataAnnotationContext(string schemaName)
        {
            this.schemaName = schemaName;
        }

        private readonly string schemaName;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().ToTable("Products", schemaName);
        }
    }
}
