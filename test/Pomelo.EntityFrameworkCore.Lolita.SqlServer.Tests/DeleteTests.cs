using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pomelo.EntityFrameworkCore.Lolita.SqlServer.Tests
{
    public class DeleteTests
    {
        [Fact]
        public void delete_with_simple_where_predicate()
        {
            using (var db = new SqlServerContext())
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .GenerateBulkDeleteSql();

                Assert.Equal(@"DELETE FROM [Posts]
WHERE [Posts].[Id] = 1;", sql, false, true, false);
            }
        }

        [Fact]
        public void delete_with_complex_where_predicate()
        {
            using (var db = new SqlServerContext())
            {
                var time = Convert.ToDateTime("2016-01-01");
                var sql = db.Users
                    .Where(x => db.Posts.Count(y => y.UserId == x.Id) == 0)
                    .Where(x => x.Role == UserRole.Member)
                    .GenerateBulkDeleteSql();

                Assert.Equal(@"DELETE FROM [Users]
WHERE ((
    SELECT COUNT(*)
    FROM [Posts] AS [y]
    WHERE [y].[UserId] = [Users].[Id]
) = 0) AND ([Users].[Role] = 0);", sql, false, true, false);
            }
        }
    }
}
