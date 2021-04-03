using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ETicaret.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ETicaret.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ETicaretContext(serviceProvider.GetRequiredService<
                DbContextOptions<ETicaretContext>>());
            context.Database.Migrate();

            //eğer boşsa
            if (context.Urunler.Any()) return; // DB has been seeded

            var urunler = new[]
            {
                new Urun
                {
                    Ad = "Ayakkabı",
                    Aciklama = "Rahat, şık ve ucuz...",
                    Fiyat = 99.99M, //urunler[0]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "10268547481650.jpg"},
                        new() {DosyaAdi = "10268547579954.jpg"},
                        new() {DosyaAdi = "10268547678258.jpg"},
                        new() {DosyaAdi = "10268547743794.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Kol saati",
                    Aciklama = "Rahat, şık ve pahalı...",
                    Fiyat = 199.99M, //urunler[1]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "kolsaati.jfif"}
                    }
                },
                new Urun
                {
                    Ad = "Drone",
                    Aciklama = "Uçar, kaçar...",
                    Fiyat = 399.90M, //urunler[2]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "10072484479026.jpg"},
                        new() {DosyaAdi = "10072484511794.jpg"},
                        new() {DosyaAdi = "10072484577330.jpg"},
                        new() {DosyaAdi = "10072484610098.jpg"},
                        new() {DosyaAdi = "10395470102578.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Oyuncu Faresi",
                    Aciklama = "Yüksek hassasiyet..",
                    Fiyat = 299.90M, //urunler[3]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "9730068119602.jpg"},
                        new() {DosyaAdi = "9730068152370.jpg"},
                        new() {DosyaAdi = "9730068185138.jpg"},
                        new() {DosyaAdi = "9730068217906.jpg"},
                        new() {DosyaAdi = "9730068250674.jpg"},
                        new() {DosyaAdi = "9730068283442.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Kemer",
                    Aciklama = "Sade bir kemer..",
                    Fiyat = 599.90M, //urunler[4]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "kemer.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Mouse",
                    Aciklama = "Her yüzeyde çalışır...",
                    Fiyat = 89.90M, //urunler[5]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "mouse.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Ateş Ölçer",
                    Aciklama = "Tam ölçer, hızlı ölçer",
                    Fiyat = 189.90M, //urunler[6]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "9973508210738.jpg"},
                        new() {DosyaAdi = "9973508276274.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Davul",
                    Aciklama = "Az yer kaplar....",
                    Fiyat = 829.90M, //urunler[7]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "10049148190770.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Parfüm",
                    Aciklama = "Kalıcıdır...",
                    Fiyat = 329.90M, //urunler[8]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "10125668220978.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Yemek Masası",
                    Aciklama = "Balkonunuza çok yakışacak...",
                    Fiyat = 899.90M, //urunler[9]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "10165918564402.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Bisiklet",
                    Aciklama = "Hafif ve dayanıklı...",
                    Fiyat = 999.90M, //urunler[10]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "10183905771570.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Makyaj Kalemi",
                    Aciklama = "Boyar, çizer...",
                    Fiyat = 49.90M, //urunler[11]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "10399240159282.jpg"}
                    }
                },
                new Urun
                {
                    Ad = "Drone 2",
                    Aciklama = "Otomatik iniş-kalkış...",
                    Fiyat = 899.90M, //urunler[12]
                    Resimler = new List<Resim>
                    {
                        new() {DosyaAdi = "drone1.png"}
                    }
                }
            };

            context.Urunler.AddRange(urunler);

            var kategoriler = new[]
            {
                new Kategori {Adi = "Elektronik"}, //kategoriler[0]
                new Kategori {Adi = "Moda"}, //kategoriler[1]
                new Kategori {Adi = "Ev/Yaşam/Kırtasiye/Ofis"}, //kategoriler[2]
                new Kategori {Adi = "Oto/Bahçe/Yapı Market"}, //kategoriler[3]
                new Kategori {Adi = "Anne/Bebek/Oyuncak"}, //kategoriler[4]
                new Kategori {Adi = "Spor/Outdoor"}, //kategoriler[5]
                new Kategori {Adi = "Kozmetik/Kişisel Bakım"}, //kategoriler[6]
                new Kategori {Adi = "Süpermarket"}, //kategoriler[7]
                new Kategori {Adi = "Kitap/Müzik/Film/Hobi"} //kategoriler[8]
            };

            context.Kategoriler.AddRange(kategoriler);

            var kategorilerVeUrunler = new[]
            {
                new KategoriUrun {Urun = urunler[0], Kategori = kategoriler[1]}, //Ayakkabı-->Moda
                new KategoriUrun {Urun = urunler[1], Kategori = kategoriler[1]}, //Kol Saati-->Moda
                new KategoriUrun {Urun = urunler[2], Kategori = kategoriler[0]}, //Drone -->Elektronik
                new KategoriUrun {Urun = urunler[2], Kategori = kategoriler[8]}, //Drone -->Hobi
                new KategoriUrun {Urun = urunler[0], Kategori = kategoriler[5]},
                new KategoriUrun {Urun = urunler[3], Kategori = kategoriler[0]},
                new KategoriUrun {Urun = urunler[4], Kategori = kategoriler[1]},
                new KategoriUrun {Urun = urunler[5], Kategori = kategoriler[0]},
                new KategoriUrun {Urun = urunler[6], Kategori = kategoriler[0]},
                new KategoriUrun {Urun = urunler[6], Kategori = kategoriler[4]},
                new KategoriUrun {Urun = urunler[7], Kategori = kategoriler[0]},
                new KategoriUrun {Urun = urunler[7], Kategori = kategoriler[8]},
                new KategoriUrun {Urun = urunler[8], Kategori = kategoriler[6]},
                new KategoriUrun {Urun = urunler[8], Kategori = kategoriler[1]},
                new KategoriUrun {Urun = urunler[9], Kategori = kategoriler[2]},
                new KategoriUrun {Urun = urunler[9], Kategori = kategoriler[3]},
                new KategoriUrun {Urun = urunler[10], Kategori = kategoriler[5]},
                new KategoriUrun {Urun = urunler[11], Kategori = kategoriler[6]},
                new KategoriUrun {Urun = urunler[12], Kategori = kategoriler[0]},
                new KategoriUrun {Urun = urunler[12], Kategori = kategoriler[8]}
            };
            context.KategorilerVeUrunler.AddRange(kategorilerVeUrunler);

            SHA256 sha = new SHA256CryptoServiceProvider();
            var salt = "17.3.999+2/5-qxw28012.992";
            var pwEncrypt =
                Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes("123")));
            var saltEncrypt = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(salt)));

            var password =
                Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(pwEncrypt + saltEncrypt)));
            
            
            context.Kullanicilar.AddRange(
                new Kullanici
                {
                    username = "burak",
                    password = password,
                    cPassword = password,
                    isAdmin = "admin"
                }
            );

            context.SaveChanges();

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Çekirdek veriler başarıyla yazıldı!");
        }
    }
}