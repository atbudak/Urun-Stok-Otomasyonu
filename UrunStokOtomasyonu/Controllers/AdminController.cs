using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokOtomasyonu.Models.EntityFramework;
using PagedList;
using PagedList.Mvc;

namespace UrunStokOtomasyonu.Controllers
{
    [Authorize(Roles = "A")]
    public class AdminController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();
        public ActionResult AdminListesi(string search, int page = 1)
        {
            var urun = from x in db.TBLUYE select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true);
            }
            return View(urun.Where(x => x.DURUM == true && x.Role == "A").ToList().ToPagedList(page, 10));
        }

        public ActionResult PasifAdminListesi(string search, int page = 1)
        {
            var urun = from x in db.TBLUYE select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == false);
            }
            return View(urun.Where(x => x.DURUM == false && x.Role == "A").ToList().ToPagedList(page, 10));
        }

        public ActionResult AktifEt(int id)
        {
            var uye = db.TBLUYE.Find(id);
            uye.DURUM = true;
            db.SaveChanges();
            return RedirectToAction("PasifAdminListesi");
        }

        public ActionResult AdminSil(int id)
        {
            var admin = db.TBLUYE.Where(x => x.Role == "A").Count();
            if (admin > 1)
            {
                var sil = db.TBLUYE.Find(id);
                sil.DURUM = false;
                db.SaveChanges();
                return RedirectToAction("AdminListesi");
            }
            return RedirectToAction("AdminListesi");
        }

        [HttpGet]
        public ActionResult AdminEkle()
        {
            TBLUYE t = new TBLUYE();
            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEkle(TBLUYE t)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                db.TBLUYE.Add(t);
                t.DURUM = true;
                t.Role = "A";
                db.SaveChanges();
                return RedirectToAction("AdminListesi");
            }
            catch
            {
                return RedirectToAction("AdminEkle");
            }
        }
        public ActionResult AdminGetir(int id)
        {
            var admin = db.TBLUYE.Find(id);
            return View(admin);
        }
        [ValidateAntiForgeryToken]
        public ActionResult AdminGuncelle(TBLUYE t)
        {
            if (!ModelState.IsValid)
            {
                return View("AdminGetir");
            }
            var admin = db.TBLUYE.Find(t.ID);

            admin.AD = t.AD;
            admin.SOYAD = t.SOYAD;
            admin.KULLANICIADI = t.KULLANICIADI;
            admin.SIFRE = t.SIFRE;
            db.SaveChanges();
            return RedirectToAction("AdminListesi");
        }


    }
}