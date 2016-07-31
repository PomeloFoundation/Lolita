using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pomelo.EntityFrameworkCore.Lolita.Tests.Models
{
    public class TestContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<Catalog> Catalogs { get; set; }
        
        public DbSet<Post> Posts { get; set; }
    }
}
