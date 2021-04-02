using System;

namespace ETicaret.Models
{
    public class KategoriUrun
    {
        public Guid KategoriId { get; set; }
        public Guid UrunId { get; set; }
        public Kategori Kategori { get; set; }
        public Urun Urun { get; set; }

        // public DateTime? EklenmeTarihi { get; set; } = DateTime.Now;

    }
}