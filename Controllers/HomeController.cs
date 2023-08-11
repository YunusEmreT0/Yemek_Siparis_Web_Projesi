using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YemekSiparisProje.Models;

namespace YemekSiparisProje.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        static int sayac = 0;
        YemekProjeEntities1 model = new YemekProjeEntities1();
        public ActionResult Index()
        {
            List<restoranGetir> r = model.restoranGetir.ToList();
            List<il> i = model.il.ToList();
            ViewBag.il = i;
            return View(r);
        }

        public ActionResult Restoran(int id)
        {
            restoran r = model.restoran.FirstOrDefault(x => x.restoranID == id);
            List<urunGetir> u = model.urunGetir.Where(x => x.restoranID == id).OrderBy(x => x.kategoriAdi).ToList();
            ViewBag.urun = u;
            return View(r);
        }

        public ActionResult Sehir(int id)
        {
            il i = model.il.FirstOrDefault(x => x.ilID == id);
            List<restoranGetir> r = model.restoranGetir.Where(x => x.ilID == id).ToList();
            ViewBag.restoran = r;
            return View(i);
        }

        public ActionResult Profil()
        {
            List<il> i = model.il.ToList();
            ViewBag.il = i;
            List<ilce> ilce = model.ilce.ToList();
            ViewBag.ilce = ilce;

            string a = User.Identity.Name;
            char rol = a.Last();
            a = a.Remove(a.Length - 1);

            if (rol == 'M')
            {
                musteri m = model.musteri.FirstOrDefault(x => x.eposta == a);
                ViewBag.m = m;
                return View();
            }
            else if (rol == 'P')
            {
                personel m = model.personel.FirstOrDefault(x => x.eposta == a);
                ViewBag.m = m;
                return View();
            }
            else
            {
                adminler m = model.adminler.FirstOrDefault(x => x.eposta == a);
                ViewBag.m = m;
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "M")]
        public ActionResult GuncelleM(musteri m)
        {
            model.musteri.AddOrUpdate(m);
            model.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult GuncelleA(adminler m)
        {
            model.adminler.AddOrUpdate(m);
            model.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "P")]
        public ActionResult GuncelleP(personel m)
        {
            model.personel.AddOrUpdate(m);
            model.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "M")]
        public ActionResult SatinAl(int id)
        {
            string a = User.Identity.Name;
            a = a.Remove(a.Length - 1);

            musteri m = model.musteri.FirstOrDefault(x => x.eposta == a);
            siparisler s = new siparisler();
            s.musteriID = m.musteriID;
            s.tarih = DateTime.Now;
            s.urunId = id;
            s.tamamlandi = false;
            model.siparisler.AddOrUpdate(s);
            model.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}