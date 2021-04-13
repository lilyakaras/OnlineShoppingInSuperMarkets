using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingInSupermarkets.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        [Display(Name = "Discount Type")]
        public BuyerType Type { get; set; }

        [Display(Name = "Role")]
        public int? RoleId { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}
