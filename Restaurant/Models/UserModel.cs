using System.Collections.Generic;
using Restaurant.Models.EF;

namespace Restaurant.Models
{
    public class UserModel
    {
        public List<Table> TableList { get; set; }
        public List<Table> TableActiveList { get; set; }
        public List<OrderDetailModel> OrderList { get; set; }
        public int TableId { get; set; }
        public Invoice Invoice { get; set; }
        public List<Product> MenuList { get; set; }
    }
}