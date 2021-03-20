using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace ETicaret.Models
{
    public class Kullanici
    {
        [Key] public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı:")]
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz!")]
        public string username { get; set; }

        [Display(Name = "Şifre:")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Şifre Onay:")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Şireler Uyuşmuyor!")]
        [NotMapped]
        public string cPassword { get; set; }

        public string isAdmin { get; set; } = "guest"; //guest -> if isAdmin not specified
    }
}