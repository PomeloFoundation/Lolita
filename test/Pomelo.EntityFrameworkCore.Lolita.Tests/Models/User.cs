using System;

namespace Pomelo.EntityFrameworkCore.Lolita.Tests.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public DateTime RememberMeExpire { get; set; } = DateTime.Now.AddDays(7);
    }
}
