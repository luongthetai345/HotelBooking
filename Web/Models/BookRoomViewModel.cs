using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class BookRoomViewModel
    {
        public DONDATPHONG DonDatPhong { get; set; }
        public PHONG Phong { get; set; }
        public KHACHSAN KhachSan { get; set; }
        public KHACHHANG KhachHang { get; set; }
    }
}