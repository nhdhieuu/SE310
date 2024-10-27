using BTH2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTH2.Controllers
{
    public class AccessController : Controller
    {
        QlbanVaLiContext db= new QlbanVaLiContext();
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return View();
            else return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public IActionResult Login (TUser user)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                var u = db.TUsers.Where(x => x.Username.Equals(user.Username)&&x.Password.Equals(user.Password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("Username", u.Username);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Access");
        }
    }
}
