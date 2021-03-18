namespace ETicaret.Models
{
    public class KategoriUrun
    {
        public int KategoriId { get; set; }
        public int UrunId { get; set; }
        public Kategori Kategori { get; set; }
        public Urun Urun { get; set; }

        // public DateTime? EklenmeTarihi { get; set; } = DateTime.Now;

    }
}