using BTH2.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BTH2.ViewComponents
{
    public class LoaiSpMenuViewComponent : ViewComponent
    {
        private readonly ILoaiSpRepository _loaiSp;
        public LoaiSpMenuViewComponent ( ILoaiSpRepository _loaiSpRepository)
        {
            _loaiSp = _loaiSpRepository;
        }
        public IViewComponentResult Invoke() {
            var loaisp = _loaiSp.GetAllLoaiSp().OrderBy(x => x.MaLoai);
            return View(loaisp);
        }
    }
}
