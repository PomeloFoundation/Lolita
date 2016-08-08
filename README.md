# Lolita

[![Travis build status](https://img.shields.io/travis/PomeloFoundation/Lolita.svg?label=travis-ci&branch=master&style=flat-square)](https://travis-ci.org/PomeloFoundation/Lolita)
[![AppVeyor build status](https://img.shields.io/appveyor/ci/Kagamine/Lolita/master.svg?label=appveyor&style=flat-square)](https://ci.appveyor.com/project/Kagamine/lolita/branch/master) [![NuGet](https://img.shields.io/nuget/v/Pomelo.EntityFrameworkCore.Lolita.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.Lolita/) [![Join the chat at https://gitter.im/PomeloFoundation/Home](https://badges.gitter.im/PomeloFoundation/Home.svg)](https://gitter.im/PomeloFoundation/Home?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

A light-weight extension which provides bulk update and delete operations for Entity Framework Core.

## Getting Started

① Add `Pomelo.EntityFrameworkCore.Lolita` package into your `project.json`. There are many different special versions for different EF database providers:

- Pomelo.EntityFrameworkCore.Lolita.MySql
- Pomelo.EntityFrameworkCore.Lolita.SqlServer
- Pomelo.EntityFrameworkCore.Lolita.PostgreSQL
- Pomelo.EntityFrameworkCore.Lolita.Sqlite

② Configure your DbContext

For ASP.NET Core developers, you can Use lolita extensions when adding the DbContext into services collection:

```c#
services.AddDbContext<Models.SampleContext>(x =>
{
    x.UseMySql("server=localhost;database=lolita;uid=root;pwd=yourpwd;");
    x.UseMySqlLolita();
});
```

For .NET Core developers, you can override the OnConfiguring of DbContext to use lolita:

```c#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseMySql("server=localhost;database=lolita;uid=root;pwd=yourpwd;");
    optionsBuilder.UseMySqlLolita();
    base.OnConfiguring(optionsBuilder);
}
```

③ There are many different extended methods for updating a column or bulk deleting.

```c#
using Microsoft.EntityFrameworkCore;
```

Updating:

```c#
db.Posts
  .Where(x => x.Time <= DateTime.Now)
  .SetField(x => x.IsPinned).WithValue(false)
  .Update();
```

You can also use the following methods to update a field:

| Method | SQL | Hint |
|--------|-----|------|
|WithValue| SET [x] = @value |  |
|Plus| SET [x] = [x] + @value | Numeric only |
|Subtract| SET [x] = [x] - @value | Numeric only |
|Multiply| SET [x] = [x] * @value | Numeric only |
|Divide| SET [x] = [x] / @value | Numeric only |
|Mod| SET [x] = [x] % @value | Numeric only |
|Prepend| SET [x] = @value + [x] | String only |
|Append| SET [x] = [x] + @value | String only |
|AddMilliseconds| SET [x] = DATEADD(ms, @value, [x]) | DateTime only|
|AddSeconds| SET [x] = DATEADD(ss, @value, [x]) | DateTime only|
|AddMinutes| SET [x] = DATEADD(mi, @value, [x]) | DateTime only|
|AddHours| SET [x] = DATEADD(hh, @value, [x]) | DateTime only|
|AddDays| SET [x] = DATEADD(dd, @value, [x]) | DateTime only|
|AddMonths| SET [x] = DATEADD(mm, @value, [x]) | DateTime only|
|AddYears| SET [x] = DATEADD(yy, @value, [x]) | DateTime only|

Deleting:

```c#
db.Users
  .Where(x => db.Posts.Count(y => y.UserId == x.Id) == 0)
  .Where(x => x.Role == UserRole.Member)
  .Delete();
```

## Contribute

One of the easiest ways to contribute is to participate in discussions and discuss issues. You can also contribute by submitting pull requests with code changes.

## License

[MIT](https://github.com/PomeloFoundation/Lolita/blob/master/LICENSE)
