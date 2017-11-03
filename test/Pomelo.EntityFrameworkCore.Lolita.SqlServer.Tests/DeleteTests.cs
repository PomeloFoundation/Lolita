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
        public void delete_without_where_predicate()
        {
            using (var db = new SqlServerContext())
            {
                var sql = db.Posts
                    .GenerateBulkDeleteSql();

                Assert.Equal(@"DELETE FROM [Posts]
;", sql, false, true, false);
            }
        }

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

        [Fact]
        public void delete_with_default_schema()
        {
            using (var db = new DefaultSchemaContext("someDefaultSchema"))
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .GenerateBulkDeleteSql();

                Assert.Equal(@"DELETE FROM [someDefaultSchema].[Posts]
WHERE [Posts].[Id] = 1;", sql, false, true, false);
            }
        }

        [Fact]
        public void delete_with_schema_from_fluent_API()
        {
            using (var db = new FluentApiContext("someApiSchema"))
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .GenerateBulkDeleteSql();

                Assert.Equal(@"DELETE FROM [someApiSchema].[Posts]
WHERE [Posts].[Id] = 1;", sql, false, true, false);
            }
        }

        [Fact]
        public void delete_with_schema_from_data_annotation()
        {
            using (var db = new DataAnnotationContext())
            {
                var sql = db.Products
                    .Where(x => x.Id == 1)
                    .GenerateBulkDeleteSql();

                Assert.Equal(@"DELETE FROM [someAttributeSchema].[Products]
WHERE [Products].[Id] = 1;", sql, false, true, false);
            }
        }

        [Fact]
        public void delete_with_schema_both_from_fluent_API_and_default()
        {
            using (var db = new FluentApiDefaultSchemaContext("someDefaultSchema", "someApiSchema"))
            {
                var sql = db.Posts
                    .Where(x => x.Id == 1)
                    .GenerateBulkDeleteSql();

                // Fluent API takes precedence over default
                Assert.Equal(@"DELETE FROM [someApiSchema].[Posts]
WHERE [Posts].[Id] = 1;", sql, false, true, false);
            }
        }

        [Fact]
        public void delete_with_schema_both_from_data_annotation_and_default()
        {
            using (var db = new DataAnnotationDefaultSchemaContext("someDefaultSchema"))
            {
                var sql = db.Products
                    .Where(x => x.Id == 1)
                    .GenerateBulkDeleteSql();

                // Data annotation takes precedence over default
                Assert.Equal(@"DELETE FROM [someAttributeSchema].[Products]
WHERE [Products].[Id] = 1;", sql, false, true, false);
            }
        }

        [Fact]
        public void delete_with_schema_both_from_fluent_API_and_data_annotation()
        {
            using (var db = new FluentApiDataAnnotationContext("someApiSchema"))
            {
                var sql = db.Products
                    .Where(x => x.Id == 1)
                    .GenerateBulkDeleteSql();

                // Fluent API takes precedence over data annotation
                Assert.Equal(@"DELETE FROM [someApiSchema].[Products]
WHERE [Products].[Id] = 1;", sql, false, true, false);
            }
        }
    }
}
