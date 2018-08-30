using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models
{
    [Table("Districts")]
    public class District
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Division { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
