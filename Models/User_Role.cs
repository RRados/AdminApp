using System.ComponentModel.DataAnnotations;

namespace AdminApp.Models
{
    public class User_Role
    {
        public int IdRole { get; set; }
        public int IdUser { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Username")]
        [RegularExpression(@"[[a-zA-Z]+", ErrorMessage = "Unesite samo slova!")]
        public Role Role { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Username")]
        [RegularExpression(@"[[a-zA-Z]+", ErrorMessage = "Unesite samo slova!")]
        public User User { get; set; }
    }
}
