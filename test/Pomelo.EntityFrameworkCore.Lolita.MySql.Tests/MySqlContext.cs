using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;

namespace Pomelo.EntityFrameworkCore.Lolita.MySql.Tests
{
    public class MySqlContext : TestContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=somedb;uid=someuser;pwd=somepwd;");
            optionsBuilder.UseMySqlLolita();
        }
    }
}
