using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingInSupermarkets.Models
{
    public enum Discount
    {
        O, R, V
    }
    public class Customer
    {
        public int ID { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string Address { get; set; }
        public Discount? Discount { get; set; }
        public ICollection<Order> Orders { get; set; }
        [Display(Name = "User Id")]
        public int UserId { get; set; }
    }
}
