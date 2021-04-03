using System;
using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class Resim
    {
        public Guid Id { get; set; }

        public string DosyaAdi { get; set; }

        public Guid UrunuId { get; set; }

        //[Required] //burayı eğer Required yapmazsak migration dan restirct olarak oluşuyor şuan ise CASCADE!
        public Urun Urunu { get; set; }
    }
}