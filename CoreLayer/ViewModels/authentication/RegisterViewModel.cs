using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.ViewModels.authentication
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
        [Compare("Password", ErrorMessage = "پسورد یکسان نیست")]
        public string RePassword { get; set; } = default!;
    }
}
