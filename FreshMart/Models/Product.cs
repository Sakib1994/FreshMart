using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models
{
    [Table("Products")]
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int SellerId { get; set; }
        public Seller Seller { get; set; }

        [Required]
        public float Price { get; set; }

        public string Description { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        public int ItemInStock { get; set; }

        [Required]
        public int DistrictId { get; set; }
        public District District { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        //IMAGE
        public string ImagePath { get; set; }

        public float OfferPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime OfferExpireDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }

        //public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
