using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Models.User
{
    public class UserSignUpModelClass
    {
        [Required]
        [EmailAddress(ErrorMessage ="Needs a proper email")]
        public string? EmailID { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConformPassword { get; set; }
        [Required] 
        [Phone(ErrorMessage ="Needs a proper Phone")]
        [StringLength(10,MinimumLength =10, ErrorMessage ="Phone number should contain 10 digits")]
        public string? ContactNumber {  get; set; }
    }
}