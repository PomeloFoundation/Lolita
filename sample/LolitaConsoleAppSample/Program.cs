using System;
using System.Linq;
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
                x.UseMySql("server=localhost;database=lolita;uid=root;pwd=123456", 
                    ServerVersion.AutoDetect("server=localhost;database=lolita;uid=root;pwd=123456"));
                x.UseMySqlLolita();
            });
            var services = collection.BuildServiceProvider();
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LolitaContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Users.Add(new User 
            {
                Id = 1,
                Username = "User 1",
                Password = "123456"
            });

            db.Users.Add(new User
            {
                Id = 2,
                Username = "User 2",
                Password = "123456"
            });

            db.Articles.Add(new Article
            {
                Title = "Article #1",
                UserId = 1,
                Content = "Test Content",
                IsPinned = false,
                Time = DateTime.UtcNow
            });

            db.Articles.Add(new Article
            {
                Title = "Article #2",
                UserId = 2,
                Content = "Test Content",
                IsPinned = false,
                Time = DateTime.UtcNow
            });

            db.Articles.Add(new Article
            {
                Title = "Article #3",
                UserId = 2,
                Content = "Test Content",
                IsPinned = false,
                Time = DateTime.UtcNow
            });

            db.Articles.Add(new Article
            {
                Title = "Article #4",
                UserId = 1,
                Content = "Test Content",
                IsPinned = false,
                Time = DateTime.UtcNow
            });

            db.SaveChanges();

            var row_updated = db.Articles
                .Where(x => x.Id <= 10)
                .Where(x => DateTime.UtcNow >= x.Time)
                .SetField(x => x.Title).Prepend("[old] ")
                .SetField(x => x.IsPinned).WithValue(true)
                .Update();
            
            var row_updated2 = db.Articles
                .Where(x => db.Users.Where(y => y.Id % 2 == 0).Select(y => y.Id).Contains(x.Id))
                .Delete();

            Console.WriteLine("Lolita finished...");
            Console.Read();
        }
    }
}
