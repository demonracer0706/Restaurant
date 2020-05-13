using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class OrderDetailModel
    {
        public string ID_THUCPHAM { get; set; }
        public string ID_HOADON { get; set; }
        public int SOLUONG { get; set; }
        public long TONGTIEN { get; set; }
        public string URL { get; set; }
        public string TEN_THUCPHAM { get; set; }
        public long DONGIA { get; set; }
    }
}