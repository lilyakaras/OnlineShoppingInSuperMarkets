using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingInSupermarkets.Models
{
    public class Order
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        [Display(Name = "Supermarket")]
        public int SuperMarketId { get; set; }
        public Customer Customer { get; set; }
        public SuperMarket SuperMarket { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
