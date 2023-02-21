using System.ComponentModel.DataAnnotations;

namespace AdminApp.Models
{
    public class Register
    {

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
    }
}
