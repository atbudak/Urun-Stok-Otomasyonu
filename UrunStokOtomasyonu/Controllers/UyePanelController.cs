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
    
    public class UyePanelController : Controller
    {
        DBUrunStokEntities db = new DBUrunStokEntities();

        public ActionResult Profil()
        {
            return View();
        }
    }
}