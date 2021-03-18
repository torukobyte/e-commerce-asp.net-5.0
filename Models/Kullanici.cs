using System.ComponentModel.DataAnnotations;
// ReSharper disable InconsistentNaming

namespace ETicaret.Models
{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz!")]
        public string username { get; set; }
        
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string isAdmin { get; set; }
        
    }
}