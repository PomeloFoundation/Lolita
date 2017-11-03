using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    public class DefaultSchemaContext : SqlServerContext
    {
        public DefaultSchemaContext(string defaultSchemaName)
        {
            this.defaultSchemaName = defaultSchemaName;
        }

        protected readonly string defaultSchemaName;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(defaultSchemaName);
        }
    }
}
