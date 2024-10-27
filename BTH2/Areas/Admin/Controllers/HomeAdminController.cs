﻿using Azure;
using BTH2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BTH2.Areas.Admin.Controllers;
[Area("admin")]
[Route("admin")]
[Route("admin/homeadmin")]
public class HomeAdminController : Controller
{
    QlbanVaLiContext db = new QlbanVaLiContext();
    [Route("")]
    [Route("index")]
    public IActionResult Index()
    {

        return View();
    }
    [Route("danhmucsanpham")]
    public IActionResult DanhMucSanPham(int? page) {
        /*var lst = db.TDanhMucSps.ToList();
        return View(lst);*/

        int pageSize = 8;
        int pageNumber = page == null || page < 0 ? 1 : page.Value;
        var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
        PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);

        return View(lst);
    }

    [Route("ThemSanPhamMoi")]
    [HttpGet]
    public IActionResult ThemSanPhamMoi()
    {
        ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
        ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
        ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
        ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
        ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
        return View();
    }

    [Route("ThemSanPhamMoi")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ThemSanPhamMoi(TDanhMucSp sanPham)
    {
        if (ModelState.IsValid)
        {
            db.TDanhMucSps.Add(sanPham);
            db.SaveChanges();
            return RedirectToAction("DanhMucSanPham");
        }
        return View(sanPham);
    }
    [Route("SuaSanPhamMoi")]
    [HttpGet]
    public IActionResult SuaSanPhamMoi(String maSanPham)
    {
        ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
        ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
        ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
        ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
        ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
        var sanPham = db.TDanhMucSps.Find(maSanPham);
        return View(sanPham);
    }

    [Route("SuaSanPhamMoi")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SuaSanPhamMoi(TDanhMucSp sanPham)
    {
        if (ModelState.IsValid)
        {
            db.Entry(sanPham).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhMucSanPham","HomeAdmin");
        }
        return View(sanPham);
    }

    [Route("XoaSanPham")]
    [HttpGet]
    public IActionResult XoaSanPham(String maSanPham)
    {
        TempData["Message"] = "";
        var chiTietSanPham = db.TChiTietSanPhams.Where(x=>x.MaSp==maSanPham).ToList();
        if (chiTietSanPham.Count() > 0)
        {
            TempData["Message"] = "Không được xóa sản phẩm này";
            return RedirectToAction("DanhMucSanPham", "HomeAdmin");
        }
        var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSanPham);
        if (anhSanPham.Any())
            db.RemoveRange(anhSanPham);
        db.Remove(db.TDanhMucSps.Find(maSanPham));
        db.SaveChanges();
        TempData["Message"] = "Sản phẩm đã được xóa";
        return RedirectToAction("DanhMucSanPham", "HomeAdmin");
    }


}