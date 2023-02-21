using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminApp.Models
{
    public partial class User
    {
        public User()
        {
            LogIns = new HashSet<LogIn>();
        }

        public int IdUser { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Username")]
        [RegularExpression(@"[[a-zA-Z]+", ErrorMessage = "Unesite samo slova!")]        
        public string UserName { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "First name")]
        [RegularExpression(@"[[a-zA-Z]+", ErrorMessage = "Unesite samo slova!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage="Obavezno polje")]
        [Display(Name = "Last name")]
        [RegularExpression(@"[[a-zA-Z]+", ErrorMessage = "Unesite samo slova!")]
        public string LastName { get; set; }

        [Required(ErrorMessage="Obavezno polje")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage="Obavezno polje")]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "Slova i brojevi!")]
        public string Address { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }


        public virtual ICollection<LogIn> LogIns { get; set; }
    }
}
