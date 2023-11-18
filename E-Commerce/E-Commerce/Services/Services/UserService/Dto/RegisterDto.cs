using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService.Dto
{
    public class RegisterDto
    { 
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{"":;'?/>,.<,])(?!.*\s).{6,10}$",
         ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non-alphanumeric, and be at least 6 characters.")]
        public string Password { get; set; }
    }
}
