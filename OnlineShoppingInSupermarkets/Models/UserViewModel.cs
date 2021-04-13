using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingInSupermarkets.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [Display(Name = "Discount Type")]
        public BuyerType Type { get; set; }
    }
}
