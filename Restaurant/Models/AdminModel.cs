using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Restaurant.Models.EF;

namespace Restaurant.Models
{
    public class AdminModel
    {
        public List<long> RevenueList { get; set; }
        public List<RevenueByMenuItem> RevenueByMenuItemList { get; set; }
        public List<Account> UserList { get; set; }
        public List<OrderTable> BookTableList { get; set; }
        public List<Table> TableList { get; set; }
        public List<Zone> ZoneList { get; set; }
        public List<Role> RoleList { get; set; }
        public List<MenuItem> MenuList { get; set; }
        public List<ProductType> MenuTypeList { get; set; }
    }
}