using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.Lolita.Tests.Models;
using Xunit;

namespace Pomelo.EntityFrameworkCore.Lolita.MySql.Tests
{
    public class UpdateTests
    {
        [Fact(Skip = "Skipped")]
        public void update_with_value()
        {
            using (var db = new MySqlContext())
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .SetField(x => x.IsPinned).WithValue(true)
                    .SetField(x => x.IsHighlighted).WithValue(true)
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE `Posts`
SET `Posts`.`IsPinned` = {0}, 
    `Posts`.`IsHighlighted` = {1}
WHERE `Posts`.`Id` = 1;", sql, false, true, false);
            }
        }

        [Fact(Skip = "Skipped")]
        public void update_plus()
        {
            using (var db = new MySqlContext())
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .SetField(x => x.PV).Plus(1)
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE `Posts`
SET `Posts`.`PV` = `Posts`.`PV` + {0}
WHERE `Posts`.`Id` = 1;", sql, false, true, false);
            }
        }

        [Fact(Skip = "Skipped")]
        public void update_prepend()
        {
            using (var db = new MySqlContext())
            {
                var sql = db.Posts
                    .Where(x => x.Time <= DateTime.Now)
                    .SetField(x => x.Title).Prepend("[Old] ")
                    .GenerateBulkUpdateSql();
                
                Assert.Equal(@"UPDATE `Posts`
SET `Posts`.`Title` = CONCAT({0}, `Posts`.`Title`)
WHERE `Posts`.`Time` <= CURRENT_TIMESTAMP();", sql, false, true, false);
            }
        }

        [Fact(Skip = "Skipped")]
        public void update_add_days()
        {
            using (var db = new MySqlContext())
            {
                var sql = db.Users
                    .Where(x => x.Id == 1)
                    .SetField(x => x.RememberMeExpire).AddDays(7)
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE `Users`
SET `Users`.`RememberMeExpire` = DATE_ADD(`Users`.`RememberMeExpire`, INTERVAL {0} day)
WHERE `Users`.`Id` = 1;", sql, false, true, false);
            }
        }

        [Fact(Skip = "Skipped")]
        public void update_with_complex_where_predicate()
        {
            using (var db = new MySqlContext())
            {
                var time = Convert.ToDateTime("2016-01-01");
                var sql = (from x in db.Users
                 where (from y in db.Posts where (y.IsHighlighted || y.IsPinned) && y.Time >= time select y.Id).Contains(x.Id)
                 && x.Role == UserRole.Member select x)
                    .SetField(x => x.Role).WithValue(UserRole.VIP)
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE `Users`
SET `Users`.`Role` = {0}
WHERE `Users`.`Id` IN (
    SELECT `y`.`Id`
    FROM `Posts` AS `y`
    WHERE ((`y`.`IsHighlighted` = TRUE) OR (`y`.`IsPinned` = TRUE)) AND (`y`.`Time` >= TIMESTAMP '2016-01-01 00:00:00.0000000')
) AND (`Users`.`Role` = 0);", sql, false, true, false);
            }
        }

        [Fact(Skip = "Skipped")]
        public void update_with_sql()
        {
            using (var db = new MySqlContext())
            {
                var sql = db.Posts
                    .SetField(x => x.IsHighlighted).WithSQL((x, y) => $"{ y.DelimitIdentifier("IsPinned") }")
                    .GenerateBulkUpdateSql();

                Assert.Equal(@"UPDATE `Posts`
SET `Posts`.`IsHighlighted` = `IsPinned`
;", sql, false, true, false);
            }
        }
    }
}
