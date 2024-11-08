using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Models.User
{
    public class LoginModelClass
    {
        [Required]
        [EmailAddress(ErrorMessage ="Needs a proper email")]
        public string Email {  get; set; }
        [Required]
        public string Password { get; set; }
    }
}