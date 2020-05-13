using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Restaurant.Models.EF;

namespace Restaurant.Models
{
    public class HomeModel
    {
        public List<Product> FoodList { get; set; }
        public List<Product> DrinkList { get; set; }
        public List<Table> TableList { get; set; }
        public int TableId { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please enter Your Full Name")]
        [StringLength(40, MinimumLength = 1)]
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}