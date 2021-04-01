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
    public class UrunController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();
        // GET: Urun
        public ActionResult UrunListesi(string search, int page = 1)
        {
            var urun = from x in db.TBLURUN select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true);
            }
            return View(urun.Where(x => x.DURUM == true).ToList().ToPagedList(page, 10));
        }
        public ActionResult PasifUrunListesi(string search, int page = 1)
        {
            var urun = from x in db.TBLURUN select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == false);
            }
            return View(urun.Where(x => x.DURUM == false).ToList().ToPagedList(page, 10));
        }

        public ActionResult AktifEt(int id)
        {
            var urn = db.TBLURUN.Find(id);
            urn.DURUM = true;
            db.SaveChanges();
            return RedirectToAction("PasifUrunListesi");
        }

        public ActionResult UrunSil(int id)
        {
            var sil = db.TBLURUN.Find(id);
            sil.DURUM = false;
            db.SaveChanges();
            return RedirectToAction("UrunListesi");
        }
        [HttpGet]
        public ActionResult UrunEkle()
        {
            TBLURUN t = new TBLURUN();
            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrunEkle(TBLURUN t)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                db.TBLURUN.Add(t);
                t.DURUM = true;
                db.SaveChanges();
                return RedirectToAction("UrunListesi");
            }
            catch
            {
                return RedirectToAction("UrunEkle");
            }
        }

        public ActionResult UrunGetir(int id)
        {

            var urn = db.TBLURUN.Find(id);
            return View(urn);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UrunGuncelle(TBLURUN k)
        {
            if (!ModelState.IsValid)
            {
                return View("UrunGetir");
            }

            var kt = db.TBLURUN.Find(k.ID);
            kt.AD = k.AD;
            kt.FIYAT = k.FIYAT;
            kt.STOK = k.STOK;
            kt.DETAY = k.DETAY;
            kt.DURUM = true;
            db.SaveChanges();
            return RedirectToAction("UrunListesi");

        }

    }
}