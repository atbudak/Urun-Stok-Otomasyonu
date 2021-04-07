using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokOtomasyonu.Models.EntityFramework;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Validation;
using System.Globalization;

namespace UrunStokOtomasyonu.Controllers
{
    [Authorize(Roles = "A")]
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
                if (kalanstok < 0)
                {
                    if (k.ACTION == "1")
                    {
                        db.TBLSATISHAREKET.Add(k);
                        db.SaveChanges();
                        //TempData["s0"] = "Başarıyla eklendi";
                        return RedirectToAction("SatisListesi");
                    }
                    else
                    {
                        TempData["s0"] = "Stok sizin talep ettiğiniz miktarı karşılamadığı için kaydedilemedi.İstek Satış aşamasını seçerek adminden talep edebilirsiniz.";
                        return RedirectToAction("SatisEkle");
                    }
                }
                else
                {
                    db.TBLSATISHAREKET.Add(k);
                    db.SaveChanges();
                    //TempData["s0"] = "Başarıyla eklendi";
                    return RedirectToAction("SatisListesi");
                }
            }
            else
            {
                if (k.ACTION == "1")
                {
                    db.TBLSATISHAREKET.Add(k);
                    db.SaveChanges();
                    //TempData["s0"] = "Başarıyla eklendi";
                    return RedirectToAction("SatisListesi");
                }
                else
                {
                    TempData["s0"] = "Stok bulunmadığı durumda İstek seçeneğini seçmelisiniz.";
                    return RedirectToAction("SatisEkle");
                }
            }

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

            var urn = db.TBLURUN.Where(x => x.ID == t.TBLURUN.ID).FirstOrDefault();
            var uye = db.TBLUYE.Where(x => x.ID == t.TBLUYE.ID).FirstOrDefault();



            double? stok = urn.STOK;
            double? urunmiktari = t.URUNMIKTARI;
            double? kalanstok = stok - urunmiktari;

            try
            {
                if (stok > 0)
                {
                    if (kalanstok < 0)
                    {
                        if (t.ACTION == "1")
                        {
                            sts.URUNMIKTARI = t.URUNMIKTARI;
                            //TempData["mesaj"] = "Başarıyla güncellendi.";
                        }
                        else
                        {
                            TempData["mesaj"] = "Stok sizin talep ettiğiniz miktarı karşılamadığı için kaydedilemedi.İstek Satış aşamasını seçerek adminden talep edebilirsiniz.";
                            return RedirectToAction("SatisGetir/"+ t.ID);
                        }
                    }
                    else
                    {
                        if (t.ACTION == "6" && sts.ACTION == "1")
                        {
                            urn.STOK = kalanstok;
                            sts.URUNMIKTARI = t.URUNMIKTARI;
                            //TempData["mesaj"] = "Başarıyla güncellendi.(Stok miktarı değişti)";
                        }
                        else
                        {
                            sts.URUNMIKTARI = t.URUNMIKTARI;
                        }
                        
                    }
                }
                else
                {
                    if (t.ACTION == "1")
                    {
                        sts.URUNMIKTARI = t.URUNMIKTARI;
                        //TempData["mesaj"] = "Başarıyla güncellendi.";
                    }
                    else
                    {
                        TempData["mesaj"] = "İşlem Başarısız.Yeterli ürün stoğu bulunmadığı için İstek Satış Talebini seçerek talepte bulunabilirsiniz.";
                        return RedirectToAction("SatisGetir/" + t.ID);
                    }
                }

                sts.TESLIMTARIHI = t.TESLIMTARIHI;
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
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {

                }
                throw;
            }
        }
    }
}
