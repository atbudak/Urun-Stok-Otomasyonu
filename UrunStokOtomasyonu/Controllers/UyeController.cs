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
    public class UyeController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();
        // GET: Uye
        public ActionResult UyeListesi(string search, int page = 1)
        {
            var urun = from x in db.TBLUYE select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true);
            }
            return View(urun.Where(x => x.DURUM == true && x.Role== "U").ToList().ToPagedList(page, 10));
        }

        public ActionResult PasifUyeListesi(string search, int page = 1)
        {
            var urun = from x in db.TBLUYE select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == false);
            }
            return View(urun.Where(x => x.DURUM == false && x.Role == "U").ToList().ToPagedList(page, 10));
        }

        public ActionResult AktifEt(int id)
        {
            var uye = db.TBLUYE.Find(id);
            uye.DURUM = true;
            db.SaveChanges();
            return RedirectToAction("PasifUyeListesi");
        }

        public ActionResult UyeSil(int id)
        {
            var sil = db.TBLUYE.Find(id);
            sil.DURUM = false;
            db.SaveChanges();
            return RedirectToAction("UyeListesi");
        }
        [HttpGet]
        public ActionResult UyeEkle()
        {
            TBLUYE t = new TBLUYE();
            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UyeEkle(TBLUYE t)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                db.TBLUYE.Add(t);
                t.DURUM = true;
                t.Role = "U";
                db.SaveChanges();
                return RedirectToAction("UyeListesi");
            }
            catch
            {
                return RedirectToAction("UyeEkle");
            }
        }

        public ActionResult UyeGetir(int id)
        {
            var uye = db.TBLUYE.Find(id);
            return View(uye);
        }

    }
}
