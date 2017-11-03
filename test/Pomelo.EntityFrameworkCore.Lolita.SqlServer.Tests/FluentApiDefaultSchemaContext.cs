using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    public class FluentApiDefaultSchemaContext : DefaultSchemaContext
    {
        public FluentApiDefaultSchemaContext(string defaultSchemaName, string schemaName)
            : base(defaultSchemaName)
        {
            this.schemaName = schemaName;
        }

        protected readonly string schemaName;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().ToTable("Posts", schemaName);
        }
    }
}
