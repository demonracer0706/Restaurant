using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class RevenueByMenuItem
    {
        public int ID { get; set; }
        public string ID_THUCPHAM { get; set; }
        public string LOAI { get; set; }
        public string TEN_THUCPHAM { get; set; }
        public int SOLUONG { get; set; }
        public long DONGIA { get; set; }
        public long DOANHTHU { get; set; }
    }
}