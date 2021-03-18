using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Data;
using ETicaret.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Controllers
{
    public class KullaniciIslemleri : Controller
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ETicaretContext _context;

        public KullaniciIslemleri(ETicaretContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Giris(Kullanici kullanici)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            kullanici.password = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(kullanici.password)));

            kullanici.username = kullanici.username.ToLower();
            var girisYapanKullanici = await _context.Kullanicilar
                .Where(x => x.username == kullanici.username && x.password == kullanici.password)
                .SingleOrDefaultAsync();

            if (girisYapanKullanici != null)
            {
                HttpContext.Session.SetString("username", girisYapanKullanici.username);
                HttpContext.Session.SetString("isAdmin", girisYapanKullanici.isAdmin);

                TempData["Mesaj"] =
                    string.Format("'{0}' Kullanıcısı başarıyla giriş yaptı.", girisYapanKullanici.username);
                return RedirectToAction("Index", "Home");
            }

            TempData["Mesaj"] = "Geçersiz kullanıcı adı veya şifre!";
            return View();
        }

        [HttpGet]
        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            TempData["Mesaj"] = "Geçerli oturum başarıyla sonlandırıldı.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Detay()
        {
            TempData["Mesaj"] = "Bu Sayfa Yapım Aşamasında!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kayit([Bind("Id,username,password,cPassword,isAdmin")]
            Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                SHA256 sha = new SHA256CryptoServiceProvider();
                kullanici.password =
                    Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(kullanici.password)));
                kullanici.cPassword =
                    Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(kullanici.cPassword)));
                kullanici.isAdmin = "user";
                kullanici.username = kullanici.username.ToLower();


                // char.ToUpper(kullanici.username[0]) + kullanici.username.Substring(1); //capitalized

                var kullaniciKayitliMi = await _context.Kullanicilar.Where(x => x.username == kullanici.username)
                    .SingleOrDefaultAsync();

                if (kullaniciKayitliMi == null)
                {
                    if (kullanici.password == kullanici.cPassword)
                    {
                        _context.Add(kullanici);
                        await _context.SaveChangesAsync();
                        TempData["Mesaj"] = kullanici.username + " aramıza hoşgeldin (:";

                        return RedirectToAction("Giris", "KullaniciIslemleri");
                    }
                    TempData["Mesaj"] = "Girdiğiniz şifreler eşleşmiyor lütfen tekrar deneyiniz!";
                }
                else
                {
                    TempData["Mesaj"] = "Kullanıcı adı alınmış.. Başka bir kullanıcı adı seçiniz!";
                }
            }

            return View(kullanici);
        }
    }
}