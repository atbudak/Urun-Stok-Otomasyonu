using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokOtomasyonu.Models.EntityFramework;

namespace UrunStokOtomasyonu.Controllers
{
    [Authorize(Roles = "A")]
    public class HomeController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();
        public ActionResult Index()
        {
            var urn = db.TBLURUN.Count();
            ViewBag.d1 = urn;
            var uye = db.TBLUYE.Where(x=>x.Role == "U").Count();
            ViewBag.d2 = uye;
            var duyuru = db.TBLDUYURU.Count();
            ViewBag.d3 = duyuru;
            var siparis1 = db.TBLSATISHAREKET.Count();
            ViewBag.d4 = siparis1;
            var siparis2 = db.TBLSATISHAREKET.Where(x => x.ACTION == "1").Count();
            ViewBag.d5 = siparis2;
            var siparis3 = db.TBLSATISHAREKET.Where(x => x.DURUM == false && x.ACTION=="4").Count();
            ViewBag.d6 = siparis3;

            var siparis4 = db.TukenenFistikKilosu().FirstOrDefault();
            ViewBag.d7 = siparis4;
            var siparis5 = db.TukenenBademKilosu().FirstOrDefault();
            ViewBag.d8 = siparis5;
            var siparis6 = db.TukenenBiberKilosu().FirstOrDefault();
            ViewBag.d9 = siparis6;

            var siparis7 = db.TBLSATISHAREKET.Where(x => x.DURUM == true).Count();
            ViewBag.d10 = siparis7;

            var urn2 = db.EnCokSatilanUrun().FirstOrDefault();
            ViewBag.d11 = urn2;
            var uye2 = db.EnCokSatanUye().FirstOrDefault();
            ViewBag.d12 = uye2;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Page403()
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Page404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
     
    }
}