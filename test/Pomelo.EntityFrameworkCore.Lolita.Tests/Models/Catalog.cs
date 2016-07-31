using System.ComponentModel.DataAnnotations;

namespace Pomelo.EntityFrameworkCore.Lolita.Tests.Models
{
    public class Catalog
    {
        [MaxLength(32)]
        public string Id { get; set; }

        [MaxLength(64)]
        public string Display { get; set; }
    }
}
