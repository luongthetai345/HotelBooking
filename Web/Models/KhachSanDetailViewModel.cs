using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class KhachSanDetailViewModel
    {
        public KHACHSAN KhachSan { get; set; }
        public List<PHONG> DanhSachPhong { get; set; }
    }
}