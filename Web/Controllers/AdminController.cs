using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.UI.WebControls.WebParts;
using Web.Models;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DatPhongContext db = new DatPhongContext();
       
        public ActionResult DanhSachKS()
        {
            var khachsan = db.KHACHSANs.ToList();
            return View(khachsan);
        }
        public ActionResult DeleteKS(int id)
        {
            // Tìm khách sạn cần xóa theo ID
            var khachsan = db.KHACHSANs.Find(id);

            // Nếu không tìm thấy khách sạn, trả về lỗi 404
            if (khachsan == null)
            {
                return HttpNotFound();
            }

            // Xóa khách sạn
            db.KHACHSANs.Remove(khachsan);
            db.SaveChanges();

            // Chuyển hướng về danh sách khách sạn
            return RedirectToAction("DanhSachKS");
        }

        // Hành động hiển thị form thêm khách sạn
        public ActionResult AddKS()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddKS(KHACHSAN newKhachSan, HttpPostedFileBase hinhanh)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu có tệp được tải lên
                if (hinhanh != null && hinhanh.ContentLength > 0)
                {
                    // Tạo tên tệp duy nhất cho hình ảnh (có thể sử dụng GUID hoặc tên tệp gốc)
                    var fileName = Path.GetFileName(hinhanh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);

                    // Lưu tệp vào thư mục
                    hinhanh.SaveAs(path);

                    // Gán đường dẫn tệp vào thuộc tính hinhanh của đối tượng KHACHSAN
                    newKhachSan.hinhanh =  fileName;
                }

                // Thêm khách sạn vào cơ sở dữ liệu
                db.KHACHSANs.Add(newKhachSan);
                db.SaveChanges();

                return RedirectToAction("DanhSachKS");
            }

            return View(newKhachSan);
        }

        // GET: Admin/Edit/{id}
        public ActionResult EditKS(int id)
        {
            var khachSan = db.KHACHSANs.Find(id);

            if (khachSan == null)
            {
                return HttpNotFound();
            }

            return View(khachSan);
        }

        // POST: Admin/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKS(KHACHSAN khachSan, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload if new image is provided
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    // Remove the old image if necessary
                    if (!string.IsNullOrEmpty(khachSan.hinhanh))
                    {
                        string oldImagePath = Path.Combine(Server.MapPath("~/Content/Image"), khachSan.hinhanh);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image
                    string fileName = Path.GetFileName(HinhAnh.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
                    HinhAnh.SaveAs(path);
                    khachSan.hinhanh = fileName;
                }

                // Update the student in the database
                db.Entry(khachSan).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DanhSachKS");
            }

            return View(khachSan);
        }

        public ActionResult SearchKS(string keyword)
        {
            var results = db.KHACHSANs
                            .Where(ks => ks.tenkhachsan.Contains(keyword))
                            .ToList();
            return View("DanhSachKS", results);
        }

        public ActionResult DanhSachPhong(int page = 1, int pageSize = 4)
        {
            // Lấy tổng số phòng để tính toán số trang
            var totalRecords = db.PHONGs.Count();

            // Lấy danh sách phòng cho trang hiện tại
            var rooms = db.PHONGs.Include(p => p.KHACHSAN)
                                  .OrderBy(p => p.id_phong) // Sắp xếp theo id hoặc tiêu chí bạn chọn
                                  .Skip((page - 1) * pageSize) // Bỏ qua số phòng đã hiển thị ở các trang trước
                                  .Take(pageSize) // Lấy số phòng tối đa trong trang hiện tại
                                  .ToList();

            // Tính toán số trang
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Tạo đối tượng ViewModel để truyền dữ liệu
            var model = new Tuple<List<PHONG>, int, int>(rooms, page, totalPages);

            // Trả về view cùng với dữ liệu
            return View(model);
        }

        public ActionResult AddPhong()
        {
            var khachsanList = db.KHACHSANs.ToList();

            // Chuyển danh sách khách sạn vào ViewBag
            ViewBag.id_khachsan = new SelectList(khachsanList, "id_khachsan", "tenkhachsan");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPhong(PHONG newPhong, HttpPostedFileBase hinhanh)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu có tệp hình ảnh được tải lên
                if (hinhanh != null && hinhanh.ContentLength > 0)
                {
                    // Tạo tên tệp duy nhất cho hình ảnh
                    var fileName = Path.GetFileName(hinhanh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);

                    // Lưu tệp vào thư mục
                    hinhanh.SaveAs(path);

                    // Gán đường dẫn tệp vào thuộc tính hinhanh của phòng
                    newPhong.hinhanh = fileName;
                }

                // Thêm phòng vào cơ sở dữ liệu
                db.PHONGs.Add(newPhong);
                db.SaveChanges();

                // Chuyển hướng về danh sách phòng
                return RedirectToAction("DanhSachPhong");
            }

            // Nếu model không hợp lệ, hiển thị lại form để người dùng nhập lại
            ViewBag.KhachSanList = new SelectList(db.KHACHSANs, "id_khachsan", "tenkhachsan");

            return View(newPhong);
        }

        public ActionResult DeletePhong(int id)
        {
            // Tìm phòng cần xóa theo ID
            var phong = db.PHONGs.Find(id);

            // Nếu không tìm thấy phòng, trả về lỗi 404
            if (phong == null)
            {
                return HttpNotFound();
            }

            // Xóa phòng
            db.PHONGs.Remove(phong);
            db.SaveChanges();

            // Chuyển hướng về danh sách phòng
            return RedirectToAction("DanhSachPhong");
        }
        // GET: Admin/EditPhong/{id}
        public ActionResult EditPhong(int id)
        {
            // Tìm phòng theo id
            var phong = db.PHONGs.Find(id);

            // Nếu không tìm thấy phòng, trả về lỗi 404
            if (phong == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách khách sạn để hiển thị trong dropdown (SelectList)
            ViewBag.KhachSanList = new SelectList(db.KHACHSANs, "id_khachsan", "tenkhachsan", phong.id_khachsan);

            // Trả về view với phòng cần sửa
            return View(phong);
        }
        // POST: Admin/EditPhong/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhong(PHONG phong, HttpPostedFileBase hinhanh)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload hình ảnh mới nếu có
                if (hinhanh != null && hinhanh.ContentLength > 0)
                {
                    // Xóa hình ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(phong.hinhanh))
                    {
                        string oldImagePath = Path.Combine(Server.MapPath("~/Content/Image"), phong.hinhanh);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu hình ảnh mới
                    string fileName = Path.GetFileName(hinhanh.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
                    hinhanh.SaveAs(path);
                    phong.hinhanh = fileName;
                }

                // Cập nhật phòng vào cơ sở dữ liệu
                db.Entry(phong).State = EntityState.Modified;
                db.SaveChanges();

                // Chuyển hướng về danh sách phòng sau khi sửa thành công
                return RedirectToAction("DanhSachPhong");
            }

            // Nếu model không hợp lệ, trả về lại form sửa phòng
            ViewBag.KhachSanList = new SelectList(db.KHACHSANs, "id_khachsan", "tenkhachsan", phong.id_khachsan);
            return View(phong);
        }

        public ActionResult SearchPhong(string keyword, int page = 1, int pageSize = 4)
        {
            var roomsQuery = db.PHONGs.Include(p => p.KHACHSAN).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                // Tìm kiếm phòng theo tên, trạng thái, hoặc các tiêu chí khác
                roomsQuery = roomsQuery.Where(p => p.sophong.Contains(keyword) ||
                                                   p.loaiphong.Contains(keyword) ||
                                                   p.trangthai.Contains(keyword));
            }

            // Tính toán tổng số phòng
            var totalRecords = roomsQuery.Count();

            // Lấy danh sách phòng cho trang hiện tại
            var rooms = roomsQuery.OrderBy(p => p.id_phong)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            // Tính toán số trang
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var model = new Tuple<List<PHONG>, int, int>(rooms, page, totalPages);

            return View("DanhSachPhong", model);
        }

        public ActionResult DanhSachKH()
        {
            var khachhang = db.KHACHHANGs.ToList();
            return View(khachhang);
        }
        public ActionResult AddKH()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddKH(KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                // Thêm khách hàng vào cơ sở dữ liệu
                db.KHACHHANGs.Add(khachhang);
                db.SaveChanges();

                // Chuyển hướng về danh sách khách hàng sau khi thêm thành công
                return RedirectToAction("DanhSachKH");
            }

            // Nếu dữ liệu không hợp lệ, quay lại form để người dùng chỉnh sửa
            return View(khachhang);
        }
        public ActionResult DeleteKH(int id)
        {
            var khachhang = db.KHACHHANGs.Find(id);

            // Nếu không tìm thấy khách hàng, trả về lỗi 404
            if (khachhang == null)
            {
                return HttpNotFound();
            }

            // Xóa khách sạn
            db.KHACHHANGs.Remove(khachhang);
            db.SaveChanges();

            // Chuyển hướng về danh sách khách hàng
            return RedirectToAction("DanhSachKH");
        }
        // GET: Admin/EditKH/{id}
        public ActionResult EditKH(int id)
        {
            var khachHang = db.KHACHHANGs.Find(id);

            if (khachHang == null)
            {
                return HttpNotFound();
            }

            return View(khachHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKH(KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachhang).State = EntityState.Modified;
                db.SaveChanges();

                // Chuyển hướng về danh sách khách hàng sau khi sửa thành công
                return RedirectToAction("DanhSachKH");
            }

            // Nếu dữ liệu không hợp lệ, quay lại form để người dùng chỉnh sửa
            return View(khachhang);
        }
        public ActionResult SearchKH(string keyword)
        {
            var results = db.KHACHHANGs
                            .Where(kh => kh.hoten.Contains(keyword))
                            .ToList();
            return View("DanhSachKH", results);
        }
        // GET: Admin/Login
        public ActionResult Login()
        {
            
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string taikhoan, string matkhau)
        {
            // Kiểm tra tài khoản và mật khẩu
            var admin = db.loginadmins.FirstOrDefault(a => a.taikhoan == taikhoan && a.matkhau == matkhau);

            if (admin != null)
            {
                // Nếu tìm thấy tài khoản, lưu ID vào Session và chuyển hướng
                Session["AdminId"] = admin.id;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                // Nếu không tìm thấy tài khoản hoặc mật khẩu sai
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu không đúng!";
                return View();
            }
        }

        // Kiểm tra xem người dùng đã đăng nhập chưa trong các phương thức khác
        public ActionResult SomeAction()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            // Thực hiện các hành động khác nếu đã đăng nhập
            return View();
        }
        // Kiểm tra đăng nhập trước khi vào bất kỳ trang nào trong AdminController
       
        // Logout action
        public ActionResult Logout()
        {
            // Xóa session khi đăng xuất
            Session["AdminId"] = null;
            return RedirectToAction("Login", "Admin");
        }

        // Trang quản lý sau khi đăng nhập
        public ActionResult Index()
        {
            if(Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            // Lấy tên tài khoản từ Session và truyền vào ViewBag
            string adminUsername = Session["AdminUsername"] as string;
            ViewBag.WelcomeMessage = $"Chào mừng đến với trang Admin, {adminUsername}";
            
            return View();


        }
       public ActionResult DonDatPhong()
        {
            var don = db.DONDATPHONGs.ToList();
            return View(don);
        }
        // Action để cập nhật trạng thái của đơn đặt phòng
        [HttpPost]
        public ActionResult CapNhatTrangThai(int id)
        {
            var donDatPhong = db.DONDATPHONGs.FirstOrDefault(d => d.id_don == id);
            if (donDatPhong != null && donDatPhong.trangthai == "Chờ xác nhận")
            {
                donDatPhong.trangthai = "Đã xác nhận";
                db.SaveChanges();  // Lưu thay đổi vào database
            }

            return RedirectToAction("DonDatPhong"); // Quay lại trang danh sách sau khi cập nhật
        }
        public ActionResult DeleteDon(int id)
        {
            // Tìm đơn cần xóa theo ID
            var don = db.DONDATPHONGs.Find(id);

            // Nếu không tìm thấy đơn, trả về lỗi 404
            if (don == null)
            {
                return HttpNotFound();
            }

            // Xóa phòng
            db.DONDATPHONGs.Remove(don);
            db.SaveChanges();

            // Chuyển hướng về danh sách phòng
            return RedirectToAction("DonDatPhong");
        }
        public ActionResult ThongKe()
        {
            // Lọc các đơn đã xác nhận
            var donDatPhongDaXacNhan = db.DONDATPHONGs
                                          .Where(d => d.trangthai == "Đã xác nhận")
                                          .ToList();

            // Tính toán số lượng đơn đã xác nhận
            int soLuongDonDaXacNhan = donDatPhongDaXacNhan.Count();

            // Tính tổng giá trị của các đơn đã xác nhận (tính tổng giatong)
            decimal tongTienDaXacNhan = donDatPhongDaXacNhan.Sum(d => d.giatong ?? 0); // Sử dụng ?? 0 để tránh null

            // Tạo ViewModel hoặc anonymous object để truyền dữ liệu vào View
            var model = new ThongKecs
            {
                SoLuongDon = soLuongDonDaXacNhan,
                TongTien = tongTienDaXacNhan
            };

            return View(model);
        }

    }
}
