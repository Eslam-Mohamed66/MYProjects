using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invaild Email")]
       
        public string Email { get; set; }
        [Required]
        //[MaxLength(6,ErrorMessage = "Minimum Length is 6 characters")]
        [StringLength(6, MinimumLength = 6)]
        public string Password{ get; set; }
       
        [Compare(nameof(Password), ErrorMessage = "Password Mismatch")]

        //[StringLength(6, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsAgree { get; set; } 
    }
}
