using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class MenuItem
    {
        public string ID_THUCPHAM { get; set; }
        public string ID_LOAI { get; set; }
        public string LOAI { get; set; }
        public string TEN_THUCPHAM { get; set; }
        public long DONGIA { get; set; }
        public string HINH_ANH { get; set; }
    }
}