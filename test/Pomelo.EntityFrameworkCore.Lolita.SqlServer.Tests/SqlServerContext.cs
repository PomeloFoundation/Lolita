using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    public class SqlServerContext : TestContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=localhost;database=somedb;uid=someuser;pwd=somepwd;");
            optionsBuilder.UseSqlServerLolita();
        }
    }
}
