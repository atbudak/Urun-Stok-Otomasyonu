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
    public class SatisController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();
        // GET: Satis
        public ActionResult SatisListesi(string search, int page = 1)
        {
            var urun = from x in db.TBLSATISHAREKET select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.TBLURUN.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.SOYAD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.MUSTERI.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true && z.TBLUYE.SOYAD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true);
            }
            return View(urun.Where(x => x.DURUM == true).ToList().OrderByDescending(x => x.SIPARISTARIHI).ToPagedList(page, 10));
        }
        public ActionResult ButunIslemler(string search, int page = 1)
        {
            var urun = from x in db.TBLSATISHAREKET select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.TBLURUN.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.SOYAD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.MUSTERI.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true && z.TBLUYE.SOYAD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true);
            }
            return View(urun.Where(x => x.DURUM == false).ToList().OrderByDescending(x => x.SIPARISTARIHI).ToPagedList(page, 10));
        }
        public ActionResult BitenUrunListesi(string search, int page = 1)
        {
            var urun = from x in db.TBLSATISHAREKET select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.TBLURUN.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.SOYAD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.MUSTERI.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.TBLUYE.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true && z.TBLUYE.SOYAD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true);
            }
            return View(urun.Where(x => x.DURUM == true && x.ACTION == "1").ToList().OrderByDescending(x => x.SIPARISTARIHI).ToPagedList(page, 10));
        }
        [HttpGet]
        public ActionResult SatisEkle()
        {
            List<SelectListItem> u1 = (from x in db.TBLURUN.Where(x => x.DURUM == true).ToList()
                                         select new SelectListItem { Text = x.AD, Value = x.ID.ToString() }).ToList();
            ViewBag.urn1 = u1;

            List<SelectListItem> u2 = (from x in db.TBLUYE.Where(x => x.DURUM == true).ToList()
                                        select new SelectListItem { Text = x.AD + " " + x.SOYAD, Value = x.ID.ToString() }).ToList();
            ViewBag.uye1 = u2;
            TBLSATISHAREKET k = new TBLSATISHAREKET();
            return View(k);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SatisEkle(TBLSATISHAREKET k)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            var urn = db.TBLURUN.Where(x => x.ID == k.TBLURUN.ID).FirstOrDefault();
            var uye = db.TBLUYE.Where(x => x.ID == k.TBLUYE.ID).FirstOrDefault();
            if (k.ACTION == "4" || k.ACTION == "5") { k.DURUM = false; } else { k.DURUM = true; }
            k.TBLURUN = urn;
            k.TBLUYE = uye;
            double? sayi1 = urn.FIYAT;
            double? sayi2 = k.URUNMIKTARI;
            k.ISLEMTUTARI = sayi1 * sayi2;
                       
            double? stok = urn.STOK;
            double? urunmiktari = k.URUNMIKTARI;
            double? kalanstok = stok - urunmiktari;
            if (stok > 0)
            {
                if (kalanstok < 0 || kalanstok == 0)
                {
                    if (k.ACTION == "1")
                    {
                        db.TBLSATISHAREKET.Add(k);
                        db.SaveChanges();
                        return RedirectToAction("SatisListesi");
                    }
                    ViewBag.err1 = "Stok bulunmadığı durumda Sipariş Tükendi seçeneğini seçmelisiniz.";
                    return RedirectToAction("SatisEkle");
                }
                else if (kalanstok > 0)
                {
                    db.TBLSATISHAREKET.Add(k);
                    db.SaveChanges();
                    return RedirectToAction("SatisListesi");
                }
            }
            else
            {
                if (k.ACTION == "1")
                {
                    db.TBLSATISHAREKET.Add(k);
                    db.SaveChanges();
                    return RedirectToAction("SatisListesi");
                }
                ViewBag.err1 = "Stok bulunmadığı durumda Sipariş Tükendi seçeneğini seçmelisiniz.";
                return RedirectToAction("SatisEkle");
            }

            db.TBLSATISHAREKET.Add(k);
            db.SaveChanges();
            return RedirectToAction("SatisListesi");
        }

        public ActionResult SatisGetir(int id)
        {

            List<SelectListItem> urun = (from x in db.TBLURUN.Where(x => x.DURUM == true).ToList()
                                         select new SelectListItem { Text = x.AD, Value = x.ID.ToString() }).ToList();
            ViewBag.urn1 = urun;

            List<SelectListItem> uye = (from x in db.TBLUYE.Where(x => x.DURUM == true).ToList()
                                        select new SelectListItem { Text = x.AD + " " + x.SOYAD, Value = x.ID.ToString() }).ToList();
            ViewBag.uye1 = uye;
            var sts = db.TBLSATISHAREKET.Find(id);
            return View("SatisGetir", sts);
        }
        [ValidateAntiForgeryToken]
        public ActionResult SatisGuncelle(TBLSATISHAREKET t)
        {   
            var sts = db.TBLSATISHAREKET.Find(t.ID);
            //if (!ModelState.IsValid)
            //{
            //    return View("SatisGetir");
            //}
            var urn = db.TBLURUN.Where(x => x.ID == t.TBLURUN.ID).FirstOrDefault();
            var uye = db.TBLUYE.Where(x => x.ID == t.TBLUYE.ID).FirstOrDefault();

           

            double? stok = urn.STOK;
            double? urunmiktari = t.URUNMIKTARI;
            double? kalanstok = stok - urunmiktari;

            if (t.ACTION == "6")
            {
                if (sts.ACTION == "1")
                {
                    if (kalanstok == 0 || kalanstok < 0)
                    {

                        ViewBag.err1 = "Stok bulunmamaktadır.";
                        return View();
                    }
                    urn.STOK = kalanstok;
                }
                t.ACTION = sts.ACTION;
            }

            sts.TESLIMTARIHI = t.TESLIMTARIHI;
            sts.URUNMIKTARI = t.URUNMIKTARI;
            sts.MUSTERI = t.MUSTERI;
            sts.MUSTERIDETAY = t.MUSTERIDETAY;

            if (t.ACTION == "4" || t.ACTION == "5")
            { sts.DURUM = false; }
            sts.ACTION = t.ACTION;


            sts.URUN = urn.ID;
            sts.UYE = uye.ID;

            double? sayi1 = urn.FIYAT;
            double? sayi2 = t.URUNMIKTARI;
            sts.ISLEMTUTARI = sayi1 * sayi2;

            db.SaveChanges();
            return RedirectToAction("SatisListesi");
        }
    }
}
