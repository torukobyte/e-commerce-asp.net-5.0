using System.Linq;
using System.Threading.Tasks;
using ETicaret.Data;
using ETicaret.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Controllers
{
    public class KategoriIslemleri : Controller
    {
        private readonly ETicaretContext _context;

        public KategoriIslemleri(ETicaretContext context)
        {
            _context = context;
        }


        // GET: KategoriIslemleri
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kategoriler.ToListAsync());
        }

        // GET: KategoriIslemleri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var kategori = await _context.Kategoriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategori == null) return NotFound();

            return View(kategori);
        }

        // GET: KategoriIslemleri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KategoriIslemleri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Adi,Aciklama")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kategori);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(kategori);
        }

        // GET: KategoriIslemleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null) return NotFound();

            return View(kategori);
        }

        // POST: KategoriIslemleri/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adi,Aciklama")] Kategori kategori)
        {
            if (id != kategori.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategori);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriExists(kategori.Id)) return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(kategori);
        }

        // GET: KategoriIslemleri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var kategori = await _context.Kategoriler
                .Include(x => x.Urunleri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategori == null) return NotFound();

            return View(kategori);
        }

        // POST: KategoriIslemleri/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, IFormCollection elemanlar)
        {
            if (HttpContext.Session.GetString("isAdmin") == "admin")
            {
                var kategori = await _context.Kategoriler
                    .Include(x => x.Urunleri)
                    .Include(x => x.KategoriUrunler)
                    .FirstOrDefaultAsync(m => m.Id == id);

                var urunlerSilinsin = elemanlar["deleteItems"] == "on";

                if (urunlerSilinsin) _context.RemoveRange(kategori.Urunleri);

                _context.RemoveRange(kategori.KategoriUrunler);

                _context.Kategoriler.Remove(kategori);
                await _context.SaveChangesAsync();

                var urunMesaj = urunlerSilinsin ? "ve içerisindeki " + kategori.Urunleri.Count + " adet ürün " : "";

                TempData["Mesaj"] = $"{kategori.Adi} kategorisi {urunMesaj} başarıyla silindi!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Mesaj"] = "Erişim reddedildi!";
            return RedirectToAction("Index", "Home");
        }

        private bool KategoriExists(int id)
        {
            return _context.Kategoriler.Any(e => e.Id == id);
        }
    }
}