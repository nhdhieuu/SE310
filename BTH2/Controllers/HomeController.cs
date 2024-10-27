using Azure;
using BTH2.Models;
using BTH2.Models.Authentication;
using BTH2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;
using X.PagedList;

namespace BTH2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        QlbanVaLiContext db = new QlbanVaLiContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authentication]
        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page ==null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x=>x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham,pageNumber,pageSize);

            return View(lst);
        }
            public IActionResult SanPhamTheoLoai(String maloai,int? page)
            {
                int pageSize = 8;
                int pageNumber = page == null || page < 0 ? 1 : page.Value;
                var lstsanpham = db.TDanhMucSps.AsNoTracking().Where(x => x.MaLoai == maloai).OrderBy(x => x.TenSp);
                PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
    /*            var lst = db.TDanhMucSps.Where(x => x.MaLoai == maloai).OrderBy(x => x.TenSp).ToList();
    */            
                ViewBag.maloai = maloai;
                return View(lst);
            }

        public IActionResult ChiTietSanPham(String maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSp = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            ViewBag.anhSp = anhSp;
            return View(sanPham);
        }

        public IActionResult ProductDetail(String maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSp = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            var homeProductDetailViewModel = new HomeProductDetailViewModel { anhSps=anhSp, danhMucSp=sanPham};
            return View(homeProductDetailViewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}