using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UrunStokOtomasyonu.Models.EntityFramework;

namespace UrunStokOtomasyonu.Controllers
{
    public class LoginController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();
        // GET: Login
        [AllowAnonymous]
        public ActionResult AdminLogin()
        {
            TBLADMIN t = new TBLADMIN();
            return View(t);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(TBLADMIN t)
        {
            var kullanici = db.TBLADMIN.FirstOrDefault(x => x.KULLANICIADI == t.KULLANICIADI && x.SIFRE == t.SIFRE);
            if (kullanici != null)
            {
                FormsAuthentication.SetAuthCookie(kullanici.KULLANICIADI, false);
            }
            else
            {
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        public ActionResult AdminLogout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("AdminLogin", "Login");
        }
        [AllowAnonymous]
        public ActionResult UyeLogin()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UyeLogin(TBLUYE t)
        {
            var kullanici = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == t.KULLANICIADI && x.SIFRE == t.SIFRE);
            if (kullanici != null)
            {
                FormsAuthentication.SetAuthCookie(kullanici.KULLANICIADI, false);
            }
            else
            {
                return View();
            }
            return RedirectToAction("SatisListesi", "UyePanel");
        }
        public ActionResult UyeLogout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("UyeLogin", "Login");
        }
    }
}