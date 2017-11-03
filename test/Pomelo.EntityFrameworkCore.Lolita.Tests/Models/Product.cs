using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomelo.EntityFrameworkCore.Lolita.Tests.Models
{
    [Table("Products", Schema = "someAttributeSchema")]
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(64)]
        [Required]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
