using System;
using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class Resim
    {
        public Guid id { get; set; }

        public string dosyaAdi { get; set; }

        public int UrunuId { get; set; }

        //[Required] //burayı eğer Required yapmazsak migration dan restirct olarak oluşuyor şuan ise CASCADE!
        public Urun Urunu { get; set; }
    }
}