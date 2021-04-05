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
            if (kullanici != null && kullanici.DURUM == true)
            {
                FormsAuthentication.SetAuthCookie(kullanici.KULLANICIADI, false);
                Session["Kullanici"] = kullanici.KULLANICIADI.ToString();
                Session["Ad"] = kullanici.AD.ToString();
                Session["Soyad"] = kullanici.SOYAD.ToString();
                if (kullanici.Role == "A")
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Anasayfa", "UyePanel");
            }
            else
            {
                return View();
            }
        }
        public ActionResult UyeLogout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("UyeLogin", "Login");
        }
    }
}