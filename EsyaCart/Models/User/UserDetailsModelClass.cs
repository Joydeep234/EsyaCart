using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Models.User
{
    public class UserDetailsModelClass
    {

        [Required] 
        [StringLength(50, ErrorMessage = "Username need to fill")]
        public string UserName { get; set; }  

        [Required] 
        [StringLength(50, ErrorMessage = "Area need to fill")]
        public string Area{get;set;}

        [Required] 
        [StringLength(50, ErrorMessage = "Landmark need to fill")]
        public string Landmark{get;set;}

        [Required]
        public int Pincode {get;set;}
        [Required]
        public int Accounts_Id {get;set;}
    }
}