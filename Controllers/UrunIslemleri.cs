using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ETicaret.Data;
using ETicaret.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Controllers
{
    public class UrunIslemleri : Controller
    {
        private readonly ETicaretContext _context;
        private readonly string _dosyaYolu;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UrunIslemleri(ETicaretContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

            _dosyaYolu = Path.Combine(_hostEnvironment.WebRootPath, "resimler");
            if (!Directory.Exists(_dosyaYolu)) Directory.CreateDirectory(_dosyaYolu);
        }

        // GET: UrunIslemleri
        public async Task<IActionResult> Index()
        {
            // ViewBag.KategoriAdi = "Tüm Kategoriler";
            return View(await _context.Urunler.Include(x => x.Resimler).ToListAsync());
        }

        public async Task<IActionResult> KategorininUrunleri(Guid? id)
        {
            var kategori = await _context.Kategoriler
                .Include(x => x.KategoriUrunler).ThenInclude(x => x.Urun).ThenInclude(x => x.Resimler)
                .SingleOrDefaultAsync(x => x.Id == id);

            var kategorininUrunleri = kategori.KategoriUrunler.Select(x => x.Urun);

            // ViewBag.KategoriAdi = kategori.Adi;
            // ViewBag.KategoriId = kategori.Id;
            ViewBag.Kategori = kategori;

            return View("index", kategorininUrunleri);
        }

        public async Task<IActionResult> KategorileriniAyarla(Guid? id)
        {
            if (id == null) return NotFound();

            var urun = await _context.Urunler
                .Include(x => x.Kategorileri)
                .SingleOrDefaultAsync(m => m.Id == id);


            if (urun == null) return NotFound();

            return View(urun);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KategorileriniAyarla(Guid? id, IFormCollection elemanlar)
        {
            var urun = await _context.Urunler
                .Include(x => x.Kategorileri)
                .SingleOrDefaultAsync(m => m.Id == id);


            urun.Kategorileri.Clear();
            foreach (var i in elemanlar["SeciliCheckBoxlar"])
                urun.Kategorileri.Add(await _context.Kategoriler.FindAsync(Guid.Parse(i)));

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{urun.Ad} Ürününün kategorisi başarıyla güncellendi!";

            return View(urun);
        }

        // GET: UrunIslemleri/Details

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var urun = await _context.Urunler.Include(x => x.Resimler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null) return NotFound();

            return View(urun);
        }

        // GET: UrunIslemleri/Create
        [HttpGet]
        public IActionResult Create(int? id)
        {
            return View();
        }

        // POST: UrunIslemleri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? id, [Bind("Ad,Aciklama,Fiyat,Dosya")] Urun urun)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in urun.Dosya)
                    {
                        string guid = Guid.NewGuid().ToString();
                        var split = item.FileName.Split(".");
                        var fileName = split[0] + guid + "." + split[1]; //split[0] noktadan öncesi guid . ve noktadan sonrası
                        
                        var tamDosyaAdi = Path.Combine(_dosyaYolu, fileName);
                        await using (var dosyaAkisi = new FileStream(tamDosyaAdi, FileMode.Create))
                        {
                            await item.CopyToAsync(dosyaAkisi);
                        }

                        urun.Resimler.Add(new Resim {DosyaAdi = fileName});
                    }
                }
                catch (NullReferenceException)
                {
                }

                if (id != null) urun.KategoriUrunler.Add(new KategoriUrun {KategoriId = (Guid) id});
                // if (id != null) urun.Kategorileri.Add(await _context.Kategoriler.FindAsync(id = id));
                _context.Add(urun);
                await _context.SaveChangesAsync();
                urun.Ad = char.ToUpper(urun.Ad[0]) + urun.Ad.Remove(0, 1);
                if (id != null) return RedirectToAction(nameof(KategorininUrunleri), new {id});
                try
                {
                    TempData["mesaj"] = urun.Ad + " ürünü Başarıyla Eklendi!";
                }
                catch
                {
                    TempData["mesaj"] = urun.Ad + " ürünü Oluşturulurken bir HATA oluştu!";
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(urun);
        }

        // GET: UrunIslemleri/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var urun = await _context.Urunler.Include(x => x.Resimler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null) return NotFound();

            return View(urun);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Ad,Aciklama,Fiyat,Dosya")] Urun urun)
        {
            if (id != urun.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var dosyaYolu = Path.Combine(_hostEnvironment.WebRootPath, "resimler");
                if (!Directory.Exists(dosyaYolu)) Directory.CreateDirectory(dosyaYolu);

                try
                {
                    foreach (var item in urun.Dosya)
                    {
                        string guid = Guid.NewGuid().ToString();
                        var split = item.FileName.Split(".");
                        var fileName = split[0] + guid + "." + split[1];
                        
                        var tamDosyaAdi = Path.Combine(dosyaYolu, fileName);

                        await using (var dosyaAkisi = new FileStream(tamDosyaAdi, FileMode.Create))
                        {
                            await item.CopyToAsync(dosyaAkisi);
                        }

                        urun.Resimler.Add(new Resim {DosyaAdi = fileName});
                    }
                }
                catch (NullReferenceException)
                {
                }

                try
                {
                    _context.Update(urun);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunExists(urun.Id)) return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }


            return View(urun);
        }

        // GET: UrunIslemleri/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            //resmin görünebilmesini sağlıyor.
            await _context.Urunler.Include(x => x.Resimler)
                .FirstOrDefaultAsync(m => m.Id == id);

            var urun = await _context.Urunler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null) return NotFound();

            return View(urun);
        }

        // POST: UrunIslemleri/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var urun = _context.Urunler.Include(x => x.Resimler).SingleOrDefault(x => x.Id == id);
            _context.Urunler.Remove(urun!);

            urun.Ad = char.ToUpper(urun.Ad[0]) + urun.Ad.Remove(0, 1);

            try
            {
                await _context.SaveChangesAsync();
                foreach (var item in urun.Resimler) System.IO.File.Delete(Path.Combine(_dosyaYolu, item.DosyaAdi));

                TempData["mesaj"] = urun.Ad + " ürünü Başarıyla Silindi!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["mesaj"] = urun.Ad + " ürünü Silinemedi!";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ResimSil(Guid id)
        {
            var resim = await _context.Resimler.FindAsync(id);
            _context.Resimler.Remove(resim);
            await _context.SaveChangesAsync();
            System.IO.File.Delete(Path.Combine(_dosyaYolu, resim.DosyaAdi));

            return RedirectToAction(nameof(Edit), new {id = resim.UrunuId});
        }

        private bool UrunExists(Guid id)
        {
            return _context.Urunler.Any(e => e.Id == id);
        }
    }
}