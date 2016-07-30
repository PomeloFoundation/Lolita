using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LolitaConsoleAppSample.Models;

namespace LolitaConsoleAppSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddDbContext<LolitaContext>(x => 
            {
                x.UseMySql("server=localhost;database=lolita;uid=root;pwd=19931101");
                x.UseLolita();
            });
            var services = collection.BuildServiceProvider();
            var db = services.GetRequiredService<LolitaContext>();
            db.Database.EnsureCreated();

            Console.WriteLine("** Bulk updating **");
            var row_updated = db.Articles
                .Where(x => x.Id <= 10)
                .Where(x => DateTime.Now >= x.Time)
                .SetField(x => x.Title).Prepend("[old] ")
                .Update();

            Console.WriteLine();
            Console.WriteLine("** Bulk deleting **");

            var row_updated2 = db.Articles
                .Where(x => db.Users.OrderBy(y => y.Id).Take(2).Select(y=>y.Id).Contains(x.Id))
                .Delete();

            Console.Read();
        }
    }
}
