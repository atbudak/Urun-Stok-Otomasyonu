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
    [Authorize(Roles ="A")]
    public class DuyuruController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();
        // GET: Duyuru
        public ActionResult DuyuruListesi(string search, int page = 1)
        {
            var ara = from x in db.TBLDUYURU select x;
            if (!string.IsNullOrEmpty(search))
            {
                ara = ara.Where(x => x.DURUM == true && x.KONU.ToUpper().Contains(search.ToUpper()));
            }
            return View(ara.Where(x => x.DURUM == true).ToList().ToPagedList(page, 10));
        }

        [HttpGet]
        public ActionResult YeniDuyuru()
        {
            TBLDUYURU dy = new TBLDUYURU();
            return View(dy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YeniDuyuru(TBLDUYURU d)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            db.TBLDUYURU.Add(d);
            d.DURUM = true;
            db.SaveChanges();
            return RedirectToAction("DuyuruListesi");
        }

        public ActionResult DuyuruSil(int id)
        {
            var duyuru = db.TBLDUYURU.Find(id);
            duyuru.DURUM = false;
            db.SaveChanges();
            return RedirectToAction("DuyuruListesi");
        }
        public ActionResult DuyuruDetay(int id)
        {
            var detay = db.TBLDUYURU.Find(id);
            return View("DuyuruDetay", detay);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DuyuruGuncelle(TBLDUYURU d)
        {
            if (!ModelState.IsValid)
            {
                return View("DuyuruDetay");
            }
            var duyuru = db.TBLDUYURU.Find(d.ID);
            duyuru.KONU = d.KONU;
            duyuru.TARIH = d.TARIH;
            duyuru.ICERIK = d.ICERIK;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}