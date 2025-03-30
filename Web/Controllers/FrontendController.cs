using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Web.Models;

namespace Web.Controllers
{
    public class FrontendController : Controller
    {

        // GET: Frontend
        
        public ActionResult Index(DateTime? ngayden, DateTime? ngaydi)
        {
            var phong = db.PHONGs.Include(p => p.KHACHSAN).Take(4).ToList();
            return View(phong);
             // Trả về các phòng có sẵn trong khoảng thời gian
            
        }
        public ActionResult HienthiKS()
        {
            using (DatPhongContext db = new DatPhongContext())
            {
                var khachsan = db.KHACHSANs.ToList();
                return View(khachsan);
            }
            
        }
        public ActionResult Details(int id)
        {
            using (DatPhongContext db = new DatPhongContext())
            {
                // Tìm phòng theo id
                var phong = db.PHONGs.Include("KHACHSAN").FirstOrDefault(p => p.id_phong == id);

                // Kiểm tra nếu không tìm thấy phòng
                if (phong == null)
                {
                    return HttpNotFound();
                }

                return View(phong);
            }
        }
        public ActionResult DetailKS(int id)
        {
            using (DatPhongContext db = new DatPhongContext())
            {
                // Lấy thông tin khách sạn theo ID
                var khachsan = db.KHACHSANs
                                 .Where(k => k.id_khachsan == id)
                                 .FirstOrDefault();

                // Nếu khách sạn không tồn tại, trả về trang lỗi 404
                if (khachsan == null)
                {
                    return HttpNotFound();
                }

                // Lấy danh sách phòng của khách sạn theo ID khách sạn
                var danhSachPhong = db.PHONGs
                                      .Where(p => p.id_khachsan == id)
                                      .ToList();

                // Tạo đối tượng ViewModel để truyền dữ liệu tới View
                var model = new KhachSanDetailViewModel
                {
                    KhachSan = khachsan,
                    DanhSachPhong = danhSachPhong
                };

                // Trả về View với dữ liệu khách sạn và danh sách phòng
                return View(model);
            }
        }

        public ActionResult DSDetailKS(int id)
        {
            using (DatPhongContext db = new DatPhongContext())
            {
                // Lấy thông tin khách sạn theo ID
                var khachsan = db.KHACHSANs
                                 .Where(k => k.id_khachsan == id)
                                 .FirstOrDefault();

                // Nếu khách sạn không tồn tại, trả về trang lỗi 404
                if (khachsan == null)
                {
                    return HttpNotFound();
                }

                // Lấy danh sách phòng của khách sạn theo ID khách sạn
                var danhSachPhong = db.PHONGs
                                      .Where(p => p.id_khachsan == id)
                                      .ToList();

                // Tạo đối tượng ViewModel để truyền dữ liệu tới View
                var model = new KhachSanDetailViewModel
                {
                    KhachSan = khachsan,
                    DanhSachPhong = danhSachPhong
                };

                // Trả về View với dữ liệu khách sạn và danh sách phòng
                return View(model);
            }
        }
        private DatPhongContext db = new DatPhongContext();

        // Các phương thức trước đó ...

        // GET: Đăng ký
        public ActionResult Register()
        {
            return View();
        }

        // POST: Đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên tài khoản đã tồn tại chưa
                var existingUser = db.KHACHHANGs.FirstOrDefault(u => u.taikhoan == khachhang.taikhoan);
                if (existingUser != null)
                {
                    ModelState.AddModelError("taikhoan", "Tài khoản đã tồn tại.");
                    return View(khachhang);
                }

                // Thêm khách hàng vào cơ sở dữ liệu
                db.KHACHHANGs.Add(khachhang);
                db.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(khachhang);
        }

        // GET: Đăng nhập
        public ActionResult Login()
        {
            return View();
        }

        // POST: Đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string taikhoan, string matkhau)
        {
            // Kiểm tra tài khoản và mật khẩu
            var user = db.KHACHHANGs.FirstOrDefault(u => u.taikhoan == taikhoan && u.matkhau == matkhau);
            if (user != null)
            {
                // Đăng nhập thành công, lưu thông tin người dùng vào session
                Session["User"] = user;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
            return View();
        }

        // Đăng xuất
        public ActionResult Logout()
        {
            // Xóa thông tin người dùng trong session
            Session["User"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult UserInfo()
        {
            // Kiểm tra nếu người dùng đã đăng nhập
            var user = Session["User"] as KHACHHANG;

            if (user == null)
            {
                // Nếu không có người dùng, chuyển hướng về trang đăng nhập
                return RedirectToAction("Login", "Frontend");
            }

            // Trả về View với thông tin khách hàng
            return View(user);
        }
        public ActionResult EditUserInfo()
        {
            var user = Session["User"] as KHACHHANG;

            if (user == null)
            {
                return RedirectToAction("Login", "Frontend");
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult EditUserInfo(KHACHHANG model)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật thông tin khách hàng trong cơ sở dữ liệu
                using (DatPhongContext db = new DatPhongContext())
                {
                    var user = db.KHACHHANGs.FirstOrDefault(k => k.id_khachhang == model.id_khachhang);
                    if (user != null)
                    {
                        user.hoten = model.hoten;
                        user.ngaysinh = model.ngaysinh;
                        user.sodienthoai = model.sodienthoai;
                        user.email = model.email;
                        user.gioitinh = model.gioitinh;
                        db.SaveChanges();
                    }
                }

                // Lưu lại thông tin mới vào session
                Session["User"] = model;

                // Quay lại trang thông tin cá nhân
                return RedirectToAction("UserInfo", "Frontend");
            }

            return View(model);
        }


        public ActionResult BookRoom(int id, DateTime? ngayden = null, DateTime? ngaydi = null)
        {
            using (DatPhongContext db = new DatPhongContext())
            {
                var phong = db.PHONGs.Include("KHACHSAN").FirstOrDefault(p => p.id_phong == id);
                if (phong == null)
                {
                    return HttpNotFound();
                }
             

                // Khởi tạo đối tượng đơn đặt phòng cho form
                var model = new DONDATPHONG
                {
                    id_phong = phong.id_phong,
                    id_khachsan = phong.id_khachsan,
                    ngayden = ngayden ?? DateTime.Now,
                    ngaydi = ngaydi ?? DateTime.Now.AddDays(1),
                    songuoi = 1,
                    giatong = phong.gia // Tính toán giá phòng
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookRoom(DONDATPHONG donDatPhong)
        {
            var user = Session["User"] as KHACHHANG;
            if (user == null)
            {
                // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
                return RedirectToAction("Login", "Frontend");
            }

            if (ModelState.IsValid)
            {
                if (donDatPhong.ngayden.HasValue && donDatPhong.ngayden.Value < DateTime.Now.Date)
                {
                    ModelState.AddModelError("", "Không đặt được các ngày đã qua.");
                    return View(); // Trả về trang đặt phòng với thông báo lỗi
                }
                // Kiểm tra nếu ngày đi lớn hơn ngày đến
                if (donDatPhong.ngaydi <= donDatPhong.ngayden)
                {
                    ModelState.AddModelError("", "Ngày đi phải lớn hơn ngày đến.");
                    return View(donDatPhong); // Trả về form với thông báo lỗi
                }

                // Kiểm tra các đơn đã xác nhận trong bảng DONDATPHONG
                var existingBooking = db.DONDATPHONGs
                    .Where(d => d.id_phong == donDatPhong.id_phong && d.trangthai == "Đã xác nhận")
                    .ToList();

                foreach (var booking in existingBooking)
                {
                    // Kiểm tra nếu ngày của đơn đặt phòng trùng với khoảng thời gian người dùng muốn đặt
                    if ((donDatPhong.ngaydi >= booking.ngayden && donDatPhong.ngaydi <= booking.ngaydi) ||
                        (donDatPhong.ngayden >= booking.ngayden && donDatPhong.ngayden <= booking.ngaydi))
                    {
                        ModelState.AddModelError("", "Phòng đã đầy trong khoảng thời gian này.");
                        return View(donDatPhong); // Trả về form đặt phòng với thông báo lỗi
                    }
                }

                // Gán trạng thái đơn đặt phòng là "Chờ xác nhận"
                donDatPhong.trangthai = "Chờ xác nhận";

                // Lấy thông tin phòng từ database theo id_phong đã gửi lên
                var phong = db.PHONGs.FirstOrDefault(p => p.id_phong == donDatPhong.id_phong);
                if (phong != null)
                {
                    donDatPhong.id_phong = phong.id_phong;
                    donDatPhong.id_khachsan = phong.id_khachsan;  // Lấy id khách sạn từ phòng

                    // Kiểm tra xem cả ngày đi và ngày đến đều có giá trị
                    if (donDatPhong.ngaydi.HasValue && donDatPhong.ngayden.HasValue)
                    {
                        var ngayDen = donDatPhong.ngayden.Value;
                        var ngayDi = donDatPhong.ngaydi.Value;

                        // Tính số ngày đặt phòng (số ngày chênh lệch giữa ngày đi và ngày đến)
                        var soNgay = (ngayDi - ngayDen).Days;  // Số ngày chênh lệch

                        if (soNgay > 0) // Đảm bảo số ngày đặt là dương
                        {
                            donDatPhong.giatong = soNgay * phong.gia;  // Tính giá tổng
                        }
                        else
                        {
                            ModelState.AddModelError("ngaydi", "Ngày đi phải lớn hơn ngày đến.");
                            return View(donDatPhong);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ngaydi", "Vui lòng chọn ngày đi và ngày đến.");
                        return View(donDatPhong);
                    }
                }

                // Gán thông tin khách hàng từ session
                donDatPhong.id_khachhang = user.id_khachhang;

                // Thêm đơn đặt phòng vào db
                db.DONDATPHONGs.Add(donDatPhong);
                db.SaveChanges();

                // Chuyển hướng đến trang xác nhận đặt phòng
                return RedirectToAction("BookingConfirmation");
            }

            // Nếu có lỗi, trả lại form để người dùng chỉnh sửa
            return View(donDatPhong);
        }


        public ActionResult BookingConfirmation()
        {
            var user = Session["User"] as KHACHHANG;

            if (user == null)
            {
                return RedirectToAction("Login", "Frontend");
            }

            // Truy vấn các đơn đặt phòng của khách hàng với trạng thái "Đã xác nhận"
            var confirmedBookings = db.DONDATPHONGs
                                      .Where(d => d.id_khachhang == user.id_khachhang && d.trangthai == "Đã xác nhận")
                                      .Include(d => d.PHONG).Include(d => d.KHACHSAN).ToList();

            // Cập nhật trạng thái phòng nếu có đơn đặt phòng đã xác nhận
            foreach (var booking in confirmedBookings)
            {
                var room = db.PHONGs.FirstOrDefault(p => p.id_phong == booking.id_phong);

                if (room != null)
                {   
                    if (booking.ngayden.HasValue && booking.ngaydi.HasValue)
                    {
                        var ngayDen = booking.ngayden.Value;
                        var ngayDi = booking.ngaydi.Value;

                        var roomsInPeriod = db.PHONGs.Where(p => p.id_phong == room.id_phong &&
                                                                 p.trangthai != "đầy" &&
                                                                 (ngayDen <= DateTime.Now && ngayDi >= DateTime.Now)).ToList();

                        foreach (var roomInPeriod in roomsInPeriod)
                        {
                            roomInPeriod.trangthai = "đầy";
                        }

                        db.SaveChanges();
                    }
                }
            }

            var model = db.DONDATPHONGs.Where(d => d.id_khachhang == user.id_khachhang).ToList();
            
            foreach (var booking in model)
            {
                if (booking.ngayden.HasValue)
                {
                    booking.ngayden = booking.ngayden.Value.Date; 
                }
                if (booking.ngaydi.HasValue)
                {
                    booking.ngaydi = booking.ngaydi.Value.Date;
                }
            }
            return View(model);
        }

        public ActionResult CancelBooking(int id)
        {
            var booking = db.DONDATPHONGs.FirstOrDefault(d => d.id_don == id);

            if (booking != null && booking.trangthai != "Đã hủy đơn")
            {
                // Cập nhật trạng thái của đơn đặt phòng thành "Đã hủy đơn"
                booking.trangthai = "Đã hủy đơn";
                // Cập nhật trạng thái phòng tương ứng thành "Trống"
                var room = db.PHONGs.FirstOrDefault(p => p.id_phong == booking.id_phong);
                if (room != null)
                {
                    room.trangthai = "Trống";  // Đặt trạng thái phòng là "Trống"
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public ActionResult SearchAvailableRooms(DateTime? ngayden, DateTime? ngaydi)
        {
            using (DatPhongContext db = new DatPhongContext())
            {
                if (ngayden == null || ngaydi == null)
                {
                    // Nếu không chọn ngày, trả về danh sách phòng không có tìm kiếm
                    return View("HienThiPhong", db.PHONGs.ToList()); // Trả về tất cả các phòng
                }

                if (ngaydi <= ngayden)
                {
                    ModelState.AddModelError("ngaydi", "Ngày đi phải lớn hơn ngày đến.");
                    return View("HienThiPhong", db.PHONGs.ToList()); // Trả về tất cả các phòng nếu ngày không hợp lệ
                }

                // Tìm các phòng trống trong khoảng thời gian người dùng chọn
                var availableRooms = db.PHONGs.Where(p =>
                    !db.DONDATPHONGs
                        .Any(dd => dd.id_phong == p.id_phong &&
                                   dd.trangthai == "Đã xác nhận" &&
                                   ((dd.ngayden >= ngayden && dd.ngayden <= ngaydi) ||
                                    (dd.ngaydi >= ngayden && dd.ngaydi <= ngaydi))))
                .ToList();

                return View("HienThiPhong", availableRooms); // Trả về các phòng có trống trong khoảng thời gian
            }
        }

        public ActionResult HienthiPhong(DateTime? ngayden, DateTime? ngaydi)
        {
            using (DatPhongContext db = new DatPhongContext())
            {
                // Nếu không có thông tin ngày đến và ngày đi, hiển thị tất cả các phòng
                if (ngayden == null || ngaydi == null)
                {
                    var phong = db.PHONGs.Include(p => p.KHACHSAN).ToList();
                    return View(phong);
                }

                // Kiểm tra ngày đến phải trước ngày đi
                if (ngaydi <= ngayden)
                {
                    ModelState.AddModelError("ngaydi", "Ngày đi phải lớn hơn ngày đến.");
                    var phong = db.PHONGs.Include(p => p.KHACHSAN).ToList();
                    return View(phong);
                }

                // Lọc các phòng trống trong khoảng thời gian
                var availableRooms = db.PHONGs.Include(p => p.KHACHSAN).Where(p =>
                    !db.DONDATPHONGs
                        .Any(dd => dd.id_phong == p.id_phong &&
                                   dd.trangthai == "Đã xác nhận" &&
                                   ((dd.ngayden >= ngayden && dd.ngayden <= ngaydi) ||
                                    (dd.ngaydi >= ngayden && dd.ngaydi <= ngaydi)))
                ).ToList();
                // Truyền ngày đến và ngày đi vào ViewBag hoặc ViewData

                ViewBag.NgayDen = ngayden.HasValue ? ngayden.Value.ToString("dd/MM/yyyy") : "Chưa có ngày đến";
                ViewBag.NgayDi = ngaydi.HasValue ? ngaydi.Value.ToString("dd/MM/yyyy") : "Chưa có ngày đi";
                return View(availableRooms); // Trả về các phòng có sẵn trong khoảng thời gian
            }
        }

    }
}