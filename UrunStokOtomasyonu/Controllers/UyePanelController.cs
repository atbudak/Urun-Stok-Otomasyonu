using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using UrunStokOtomasyonu.Models.EntityFramework;

namespace UrunStokOtomasyonu.Controllers
{
    [Authorize(Roles ="A,U")]
    public class UyePanelController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();

        public ActionResult Anasayfa()
        {
            var uye = (string)Session["Kullanici"];
            var deger = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == uye);

            var uye1 = db.EnCokSatanUye().FirstOrDefault();
            ViewBag.d2 = uye1;
            var duyuru = db.TBLDUYURU.Count();
            ViewBag.duyuru = duyuru;

            var d1 = db.TBLURUN.Where(x => x.ID == 1).Select(x => x.STOK).FirstOrDefault();
            ViewBag.d1 = d1;
            var d3 = db.TBLURUN.Where(x => x.ID == 3).Select(x => x.STOK).FirstOrDefault();
            ViewBag.d3 = d3;
            var d4 = db.TBLURUN.Where(x => x.ID == 2).Select(x => x.STOK).FirstOrDefault();
            ViewBag.d4 = d4;

            var uyeid = db.TBLUYE.Where(x => x.KULLANICIADI == deger.KULLANICIADI).Select(y => y.ID).FirstOrDefault();

            var d5 = db.TBLSATISHAREKET.Where(x => x.UYE == uyeid && x.ACTION == "1").Count();
            ViewBag.d5 = d5;
            var d6 = db.TBLSATISHAREKET.Where(x => x.UYE == uyeid && x.DURUM == false).Count();
            ViewBag.d6 = d6;

            var d7 = db.TBLSATISHAREKET.Where(x => x.UYE == uyeid && x.ACTION == "1" && x.URUN == 1).Sum(x => x.URUNMIKTARI);
            ViewBag.d7 = d7;
            var d8 = db.TBLSATISHAREKET.Where(x => x.UYE == uyeid && x.ACTION == "1" && x.URUN == 3).Sum(x => x.URUNMIKTARI);
            ViewBag.d8 = d8;
            var d9 = db.TBLSATISHAREKET.Where(x => x.UYE == uyeid && x.ACTION == "1" && x.URUN == 2).Sum(x => x.URUNMIKTARI);
            ViewBag.d9 = d9;

            var d10 = db.TBLSATISHAREKET.Where(x => x.UYE == uyeid && x.DURUM == true).Count();
            ViewBag.d10 = d10;

            var urn2 = db.EnCokSatilanUrun().FirstOrDefault();
            ViewBag.d11 = urn2;


            return View(deger);
        }
        public ActionResult Profil(TBLUYE t)
        {
            var uye = (string)Session["Kullanici"];
            var deger = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == uye);
            return View(deger);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilGuncelle(TBLUYE t)
        {
            if (!ModelState.IsValid)
            {
                return View("Profil");
            }
            var kullanici = (string)Session["Kullanici"];
            var uye = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == kullanici);
            uye.KULLANICIADI = t.KULLANICIADI;
            uye.AD = t.AD;
            uye.SOYAD = t.SOYAD;
            uye.SIFRE = t.SIFRE;
            db.SaveChanges();
            return RedirectToAction("UyeLogout", "Login");
        }

        public ActionResult Duyurular(string search, int page = 1)
        {
            var ara = from x in db.TBLDUYURU select x;
            if (!string.IsNullOrEmpty(search))
            {
                ara = ara.Where(x => x.DURUM == true && x.KONU.ToUpper().Contains(search.ToUpper()));
            }
            return View(ara.Where(x => x.DURUM == true).OrderByDescending(x => x.TARIH).ToList().ToPagedList(page, 10));
        }

        public ActionResult DuyuruDetay(int id)
        {
            var detay = db.TBLDUYURU.Find(id);
            return View("DuyuruDetay", detay);
        }
        public ActionResult SatisListesi(string search, int page = 1)
        {
            var uye = (string)Session["Kullanici"];
            var deger = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == uye);

            var urun = from x in db.TBLSATISHAREKET select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.UYE == deger.ID && z.TBLURUN.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.MUSTERI.ToUpper().Contains(search.ToUpper()) && z.DURUM == true);
            }
            return View(urun.Where(x => x.UYE == deger.ID && x.DURUM == true).ToList().OrderByDescending(x => x.SIPARISTARIHI).ToPagedList(page, 10));
        }

        public ActionResult TamamlananSatislar(string search, int page = 1)
        {
            var uye = (string)Session["Kullanici"];
            var deger = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == uye);

            var urun = from x in db.TBLSATISHAREKET select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.UYE == deger.ID && z.TBLURUN.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true || z.MUSTERI.ToUpper().Contains(search.ToUpper()) && z.DURUM == true);
            }
            return View(urun.Where(x => x.UYE == deger.ID && x.DURUM == false).ToList().OrderByDescending(x => x.SIPARISTARIHI).ToPagedList(page, 10));
        }

        public ActionResult IstenenUrunler(string search, int page = 1)
        {
            var uye = (string)Session["Kullanici"];
            var deger = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == uye);

            var urun = from x in db.TBLSATISHAREKET select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.UYE == deger.ID && z.TBLURUN.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true && z.ACTION == "1" || z.MUSTERI.ToUpper().Contains(search.ToUpper()) && z.DURUM == true && z.ACTION == "1");
            }
            return View(urun.Where(x => x.UYE == deger.ID && x.DURUM == true && x.ACTION == "1").ToList().OrderByDescending(x => x.SIPARISTARIHI).ToPagedList(page, 10));
        }
        public ActionResult GelenUrunler(string search, int page = 1)
        {
            var uye = (string)Session["Kullanici"];
            var deger = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == uye);

            var urun = from x in db.TBLSATISHAREKET select x;
            if (!string.IsNullOrEmpty(search))
            {
                urun = urun.Where(z => z.UYE == deger.ID && z.TBLURUN.AD.ToUpper().Contains(search.ToUpper()) && z.DURUM == true && z.ACTION == "6" || z.MUSTERI.ToUpper().Contains(search.ToUpper()) && z.DURUM == true && z.ACTION == "6");
            }
            return View(urun.Where(x => x.UYE == deger.ID && x.DURUM == true && x.ACTION == "6").ToList().OrderByDescending(x => x.SIPARISTARIHI).ToPagedList(page, 10));
        }

        [HttpGet]
        public ActionResult SatisEkle()
        {
            List<SelectListItem> u1 = (from x in db.TBLURUN.Where(x => x.DURUM == true).ToList()
                                       select new SelectListItem { Text = x.AD, Value = x.ID.ToString() }).ToList();
            ViewBag.urn1 = u1;

            TBLSATISHAREKET k = new TBLSATISHAREKET();
            return View(k);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SatisEkle(TBLSATISHAREKET k)
        {

            var kullanici = (string)Session["Kullanici"];
            var girenuye = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == kullanici);

            var urn = db.TBLURUN.Where(x => x.ID == k.TBLURUN.ID).FirstOrDefault();

            if (k.ACTION == "4" || k.ACTION == "5") { k.DURUM = false; } else { k.DURUM = true; }
            k.TBLURUN = urn;
            k.UYE = girenuye.ID;
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
                        return RedirectToAction("SatisListesi");
                    }
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

            var sts = db.TBLSATISHAREKET.Find(id);
            return View("SatisGetir", sts);
        }

        [ValidateAntiForgeryToken]
        public ActionResult SatisGuncelle(TBLSATISHAREKET t)
        {
            var kullanici = (string)Session["Kullanici"];
            var girenuye = db.TBLUYE.FirstOrDefault(x => x.KULLANICIADI == kullanici);
            var sts = db.TBLSATISHAREKET.Find(t.ID);

            var urn = db.TBLURUN.Where(x => x.ID == t.TBLURUN.ID).FirstOrDefault();



            double? stok = urn.STOK;
            double? urunmiktari = t.URUNMIKTARI;
            double? kalanstok = stok - urunmiktari;

            if (stok > 0)
            {
                if (kalanstok < 0)
                {
                    if (t.ACTION == "1")
                    {
                        sts.URUNMIKTARI = t.URUNMIKTARI;
                        ViewBag.err1 = "Stok bulunmamaktadır.";
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    sts.URUNMIKTARI = t.URUNMIKTARI;
                }
            }
            else
            {
                if (t.ACTION == "1")
                {
                    sts.URUNMIKTARI = t.URUNMIKTARI;
                }
                else
                {
                    return View();
                }
            }

            sts.TESLIMTARIHI = t.TESLIMTARIHI;
            sts.MUSTERI = t.MUSTERI;
            sts.MUSTERIDETAY = t.MUSTERIDETAY;

            if (t.ACTION == "4" || t.ACTION == "5")
            { sts.DURUM = false; }
            sts.ACTION = t.ACTION;


            sts.URUN = urn.ID;
            sts.UYE = girenuye.ID;

            double? sayi1 = urn.FIYAT;
            double? sayi2 = t.URUNMIKTARI;
            sts.ISLEMTUTARI = sayi1 * sayi2;

            db.SaveChanges();
            return RedirectToAction("SatisListesi");
        }


    }
}