using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LolitaConsoleAppSample.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }

        public bool IsPinned { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
