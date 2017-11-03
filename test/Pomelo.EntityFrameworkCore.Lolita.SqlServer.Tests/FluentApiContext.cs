using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    public class FluentApiContext : SqlServerContext
    {
        public FluentApiContext(string schemaName)
        {
            this.schemaName = schemaName;
        }

        private readonly string schemaName;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().ToTable("Posts", schemaName);
        }
    }
}
