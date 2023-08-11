using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YemekSiparisProje.Models;

namespace YemekSiparisProje.Controllers
{
    [Authorize(Roles = "P")]
    public class RestoranController : Controller
    {
        YemekProjeEntities1 model = new YemekProjeEntities1();

        // GET: Restoran
        public ActionResult Urunler()
        {
            string a = User.Identity.Name;
            a = a.Remove(a.Length - 1);
            personel p = model.personel.FirstOrDefault(x => x.eposta == a);
            List<urunGetir2> u = model.urunGetir2.Where(x => x.restoranID == p.restoranID).ToList();
            ViewBag.u = u;
            ViewBag.restoranID = p.restoranID;
            return View();
        }

        public ActionResult Kampanya(int id)
        {
            return View(id);
        }

        public ActionResult KampanyaEkle(kampanya k)
        {
            model.kampanya.AddOrUpdate(k);
            model.SaveChanges();
            return RedirectToAction("Urunler", "Restoran");
        }

        [HttpPost]
        public void KampanyaSil(int id)
        {
            kampanya k = model.kampanya.FirstOrDefault(x => x.kampanyaID == id);
            if(k != null)
            {
                k.aktif = false;
                model.kampanya.AddOrUpdate(k);
                model.SaveChanges();
            }
        }

        
        public ActionResult UrunEkle()
        {
            string a = User.Identity.Name;
            a = a.Remove(a.Length - 1);
            personel p = model.personel.FirstOrDefault(x=> x.eposta == a);
            ViewBag.restoranID = p.restoranID;
            ViewBag.urun = null;
            ViewBag.ktg = model.kategoriler.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UrunEkle(int u)
        {
            urunler urun = model.urunler.FirstOrDefault(x => x.urunID == u);
            ViewBag.restoranID = urun.restoranID;
            ViewBag.urun = urun;
            ViewBag.ktg = model.kategoriler.ToList();
            return View();
        }

        public ActionResult UrunGuncelle(urunler u)
        {
            model.urunler.AddOrUpdate(u);
            model.SaveChanges();
            return RedirectToAction("Urunler", "Restoran");

        }

        public void UrunSil(int id)
        {
            urunler u = model.urunler.FirstOrDefault(x => x.urunID == id);
            if (u != null)
            {
                u.silindi = true;
                model.urunler.AddOrUpdate(u);
                model.SaveChanges();
            }
        }

        public ActionResult Siparisler()
        {
            string a = User.Identity.Name;
            a = a.Remove(a.Length - 1);
            personel p = model.personel.FirstOrDefault(x => x.eposta == a);
            List<siparisGetir> s = model.siparisGetir.Where(x => x.restoranID == p.restoranID && x.tamamlandi == false).ToList();
            return View(s);
        }

        [HttpPost]
        public void SiparisTamamla(int id)
        {
            siparisler s = model.siparisler.FirstOrDefault(x => x.siparisID == id);
            s.tamamlandi = true;
            model.siparisler.AddOrUpdate(s);
            model.SaveChanges();
        }
    }
}