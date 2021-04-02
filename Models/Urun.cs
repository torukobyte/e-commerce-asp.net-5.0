using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ETicaret.Models
{
    public class Urun
    {
        public Urun()
        {
            Resimler = new List<Resim>();
        }

        [Key] public Guid Id { get; set; }

        [StringLength(20, ErrorMessage = "{0} Alanı 10 karakterden uzun olamaz!")]
        [Required(ErrorMessage = "{0} Alanı Boş Bırakılamaz!")]
        public string Ad { get; set; }

        [Display(Name = "Açıklama")] public string Aciklama { get; set; }

        [Required(ErrorMessage = "{0} Alanı Boş Bırakılamaz!")]
        [DisplayFormat(ApplyFormatInEditMode = false,
            DataFormatString =
                "{0:c}")] //edit butonuna bastığımızda tl simgesi gelmemesi için format modeunu false yapıyoruz
        public decimal Fiyat { get; set; }

        [NotMapped] public IFormFile[] Dosya { get; set; } //resmin asıl yer kaplayan dosyasını tutacağız

        public List<Resim> Resimler { get; set; }

        public List<KategoriUrun> KategoriUrunler { get; set; }

        public List<Kategori> Kategorileri { get; set; } =
            new(); //birden fazla kategori bağlayabileceğimiz anlamına gelir
    }
}