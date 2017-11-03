using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    class DefaultSchemaContext : SqlServerContext
    {
        public DefaultSchemaContext(string schemaName)
        {
            this.schemaName = schemaName;
        }

        private readonly string schemaName;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(schemaName);
        }
    }
}
