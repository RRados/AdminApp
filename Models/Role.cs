using System;
using System.ComponentModel.DataAnnotations;

namespace AdminApp.Models
{
    public partial class Role
    {
        public int IdRole { get; set; }
        
        [RegularExpression(@"[[a-zA-Z]+", ErrorMessage = "Unesite samo slova!")]
        [Required(ErrorMessage = "Ime role je obavezno!")]
        [Display(Name = "Role name")]
        public string RoleName { get; set; }


        [Display(Name = "Created")]
        public DateTime? DateCreated { get; set; }


        [Display(Name = "Dismised")]
        public DateTime? DateDismised { get; set; }
    }
}
