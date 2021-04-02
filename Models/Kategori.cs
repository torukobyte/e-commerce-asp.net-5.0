using System;
using System.Collections.Generic;

namespace ETicaret.Models
{
    public class Kategori
    {
        public Guid Id { get; set; }
        public string Adi { get; set; }
        public string Aciklama { get; set; }
        
        public List<Urun> Urunleri { get; set; } = new List<Urun>();
        public List<KategoriUrun> KategoriUrunler { get; set; }
    }
}