using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;
using Xunit;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    public class UpdateTests
    {
        [Fact]
        public void update_with_value()
        {
            using (var db = new SqlServerContext())
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .SetField(x => x.IsPinned).WithValue(true)
                    .SetField(x => x.IsHighlighted).WithValue(true)
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE [Posts]
SET [Posts].[IsPinned] = {0}, 
    [Posts].[IsHighlighted] = {1}
WHERE [Posts].[Id] = 1;", sql);
            }
        }

        [Fact]
        public void update_plus()
        {
            using (var db = new SqlServerContext())
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .SetField(x => x.PV).Plus(1)
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE [Posts]
SET [Posts].[PV] = [Posts].[PV] + {0}
WHERE [Posts].[Id] = 1;", sql);
            }
        }

        [Fact]
        public void update_prepend()
        {
            using (var db = new SqlServerContext())
            {
                var time = DateTime.Now.AddDays(-30);
                var sql = db.Posts
                    .Where(x => x.Time <= time)
                    .SetField(x => x.Title).Prepend("[Old] ")
                    .GenerateBulkUpdateSql();
                Assert.True(sql.IndexOf(@"UPDATE [Posts]
SET [Posts].[Title] = {0}+[Posts].[Title]
WHERE [Posts].[Time] <= '") >= 0);
            }
        }

        [Fact]
        public void update_add_days()
        {
            using (var db = new SqlServerContext())
            {
                var sql = db.Users
                    .Where(x => x.Id == 1)
                    .SetField(x => x.RememberMeExpire).AddDays(7)
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE [Users]
SET [Users].[RememberMeExpire] = DATEADD(dd, {0}, [Users].[RememberMeExpire])
WHERE [Users].[Id] = 1;", sql);
            }
        }

        [Fact]
        public void update_with_complex_where_predicate()
        {
            using (var db = new SqlServerContext())
            {
                var time = Convert.ToDateTime("2016-01-01");
                var sql = (from x in db.Users
                           where (from y in db.Posts where (y.IsHighlighted || y.IsPinned) && y.Time >= time select y.Id).Contains(x.Id)
                           && x.Role == UserRole.Member
                           select x)
                    .SetField(x => x.Role).WithValue(UserRole.VIP)
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE [Users]
SET [Users].[Role] = {0}
WHERE [Users].[Id] IN (
    SELECT [y].[Id]
    FROM [Posts] AS [y]
    WHERE (([y].[IsHighlighted] = 1) OR ([y].[IsPinned] = 1)) AND ([y].[Time] >= '2016-01-01T00:00:00.000')
) AND ([Users].[Role] = 0);", sql);
            }
        }
    }
}
