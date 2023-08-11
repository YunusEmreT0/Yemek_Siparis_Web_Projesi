using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YemekSiparisProje.Models;

namespace YemekSiparisProje.Controllers
{
    [Authorize(Roles = "A")]
    public class AdminController : Controller
    {
        YemekProjeEntities1 model = new YemekProjeEntities1();
        // GET: Admin
        public ActionResult Musteriler()
        {
            List<musteri> m = model.musteri.Where(x => x.silindi == false).ToList();
            return View(m);
        }

        [HttpPost]
        public void MusteriSil(int id)
        {
            musteri m = model.musteri.FirstOrDefault(x => x.musteriID == id);
            if (m != null)
            {
                m.silindi = true;
                model.musteri.AddOrUpdate(m);
                model.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult MusteriDuzenle(int mst)
        {
            musteri m = model.musteri.FirstOrDefault(x => x.musteriID == mst);
            ViewBag.m = m;
            ViewBag.i = model.il.ToList();
            ViewBag.c = model.ilce.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Guncelle(musteri mus)
        {
            model.musteri.AddOrUpdate(mus);
            model.SaveChanges();
            return RedirectToAction("Musteriler");
        }

        public ActionResult Sirketler()
        {
            List<sirket> s = model.sirket.Where(x => x.silindi == false).ToList();
            return View(s);
        }

        [HttpPost]
        public ActionResult SirketDuzenle(int srk)
        {
            sirket s = model.sirket.FirstOrDefault(x => x.sirketID == srk);
            ViewBag.s = s;
            return View();
        }

        [HttpPost]
        public ActionResult SGuncelle(sirket srt)
        {
            model.sirket.AddOrUpdate(srt);
            model.SaveChanges();
            return RedirectToAction("Sirketler");
        }

        [HttpPost]
        public void SirketSil(int id)
        {
            sirket s = model.sirket.FirstOrDefault(x => x.sirketID == id);
            if (s != null)
            {
                s.silindi = true;
                model.sirket.AddOrUpdate(s);
                model.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult Restoranlar(int id)
        {
            List<restoranGetir> r = model.restoranGetir.Where(x => x.silindi == false && x.sirketID == id).ToList();
            ViewBag.sirketID = id;
            return View(r);
        }

        public ActionResult SirketDuzenle()
        {
            return View();
        }

        [HttpPost]
        public void RestoranSil(int id)
        {
            restoran r = model.restoran.FirstOrDefault(x => x.restoranID == id);
            if(r != null)
            {
                r.silindi = true;
                model.restoran.AddOrUpdate(r);
                model.SaveChanges();
            }
        }

        public ActionResult RestoranDuzenle(int id)
        {
            ViewBag.sirket = id;
            ViewBag.il = model.il.ToList();
            ViewBag.ilce = model.ilce.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult RestoranDuzenle(string srk)
        {
            int id = Convert.ToInt32(srk);
            ViewBag.s = model.restoran.FirstOrDefault(x => x.restoranID == id);
            ViewBag.il = model.il.ToList();
            ViewBag.ilce = model.ilce.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult RGuncelle(restoran res)
        {
            model.restoran.AddOrUpdate(res);
            model.SaveChanges();
            return RedirectToAction("Restoranlar/" + res.sirketID);
        }

        [HttpGet]
        public ActionResult Personeller(int id)
        {
            List<personel> r = model.personel.Where(x => x.silindi == false && x.restoranID == id).ToList();
            ViewBag.rest = model.restoran.FirstOrDefault(x => x.silindi == false && x.restoranID == id);
            return View(r);
        }

        [HttpPost]
        public void PersonelSil(int id)
        {
            personel p = model.personel.FirstOrDefault(x => x.personelID == id);
            if(p != null)
            {
                p.silindi = true;
                model.personel.AddOrUpdate(p);
                model.SaveChanges();
            }
        }

        public ActionResult PersonelDuzenle(int id)
        {
            ViewBag.rest = id;
            return View();
        }

        [HttpPost]
        public ActionResult PersonelDuzenle(string srk)
        {
            int id = Convert.ToInt32(srk);
            ViewBag.s = model.personel.FirstOrDefault(x => x.personelID == id);
            return View();
        }

        [HttpPost]
        public ActionResult PGuncelle(personel res)
        {
            model.personel.AddOrUpdate(res);
            model.SaveChanges();
            return RedirectToAction("Personeller/" + res.restoranID);
        }
    }
}