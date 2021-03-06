﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FreshMart.Areas.Admin.Models;

namespace FreshMart.Models
{
    [Table("Districts")]
    public class District
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Division { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<SellerRequest> SellerRequest { get; set; }

    }
}