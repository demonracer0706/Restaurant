using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class UserDetail
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}