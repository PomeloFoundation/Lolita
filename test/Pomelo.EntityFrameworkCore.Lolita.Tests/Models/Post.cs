using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pomelo.EntityFrameworkCore.Lolita.Tests.Models
{
    public class Post
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime Time { get; set; }

        [MaxLength(32)]
        [ForeignKey("Catalog")]
        public string CatalogId { get; set; }

        public virtual Catalog Catalog { get; set; }

        public bool IsPinned { get; set; }

        public bool IsHighlighted { get; set; }

        public int PV { get; set; } = 0;
    }
}
