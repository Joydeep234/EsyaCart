using System.ComponentModel.DataAnnotations;

namespace EsyaCart.Models.Admin
{
    public class AdminLogin
    {
        [Required(ErrorMessage ="Please Enter Email") ]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please Enter Password")]
        
        public string Password { get; set; }
    }
}
