using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class ForgetPasswordViewModel
    {

        [Required]
        [EmailAddress(ErrorMessage = "Invaild Email")]

        public string Email { get; set; }
    }
}
