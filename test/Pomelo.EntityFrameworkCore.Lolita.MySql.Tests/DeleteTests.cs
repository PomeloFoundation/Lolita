using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;
using Xunit;

namespace Pomelo.EntityFrameworkCore.Lolita.MySql.Tests
{
    public class DeleteTests
    {
        [Fact]
        public void delete_with_simple_where_predicate()
        {
            using (var db = new MySqlContext())
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .GenerateBulkDeleteSql();

                Assert.Equal(@"DELETE FROM `Posts`
WHERE `Posts`.`Id` = 1;", sql);
            }
        }
        
        [Fact]
        public void delete_with_complex_where_predicate()
        {
            using (var db = new MySqlContext())
            {
                var time = Convert.ToDateTime("2016-01-01");
                var sql = db.Users
                    .Where(x => db.Posts.Count(y => y.UserId == x.Id) == 0)
                    .Where(x => x.Role == UserRole.Member)
                    .GenerateBulkDeleteSql();

                Assert.Equal(@"DELETE FROM `Users`
WHERE ((
    SELECT CAST(COUNT(*) AS UNSIGNED)
    FROM `Posts` AS `y`
    WHERE `y`.`UserId` = `Users`.`Id`
) = 0) AND (`Users`.`Role` = 0);", sql);
            }
        }
    }
}
