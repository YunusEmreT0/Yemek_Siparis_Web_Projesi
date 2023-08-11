using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YemekSiparisProje.Models;

namespace YemekSiparisProje.Controllers
{
    [Authorize]
    public class GirisController : Controller
    {
        YemekProjeEntities1 model = new YemekProjeEntities1();

        // GET: Giris
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<il> i = model.il.ToList();
            ViewBag.il = i;
            List<ilce> ilce = model.ilce.ToList();
            ViewBag.ilce = ilce;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(musteri t)
        {
            List<il> i = model.il.ToList();
            ViewBag.il = i;
            List<ilce> ilce = model.ilce.ToList();
            ViewBag.ilce = ilce;

            musteri m = model.musteri.FirstOrDefault(x => x.eposta == t.eposta && x.sifre == t.sifre && x.silindi == false);
            if (m == null)
            {
                personel p = model.personel.FirstOrDefault(x => x.eposta == t.eposta && x.sifre == t.sifre && x.silindi == false);
                if (p == null)
                {
                    adminler a = model.adminler.FirstOrDefault(x => x.eposta == t.eposta && x.sifre == t.sifre);
                    if(a == null)
                    {
                        ViewBag.hata = "Kullanıcı Adı veya Şifre Hatalı!";
                        return View();
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(a.eposta + "A", false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(p.eposta + "P", false);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                FormsAuthentication.SetAuthCookie(m.eposta + "M", false);
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UyeOl(musteri m)
        {
            model.musteri.AddOrUpdate(m);
            model.SaveChanges();
            FormsAuthentication.SetAuthCookie(m.eposta + "M", false);
            return RedirectToAction("Index", "Home");
        }
    }
}