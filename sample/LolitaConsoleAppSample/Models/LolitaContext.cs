using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LolitaConsoleAppSample.Models
{
    public class LolitaContext : DbContext
    {
        public LolitaContext(DbContextOptions opt) : base(opt) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Article> Articles { get; set; }
    }
}
