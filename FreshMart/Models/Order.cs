using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models
{
    [Table("Orders")]
    public class Order
    {
        public int Id { get; set; }

        //public int ProductOrderId { get; set; }

        //public IList<Product> Product { get; set; }

        //public string SellerId { get; set; }

        public int BuyerId { get; set; }

        public int TransactionId { get; set; }

        public int ShippingId { get; set; }

        public bool IsOrderCompleted { get; set; }

        public DateTime OrderDate { get; set; }

        public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
