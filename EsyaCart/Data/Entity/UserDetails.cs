using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Data.Entity
{
    public class UserDetails
    {
        [Key]
        public int UserDetails_Id { get; set; }

        [Required, StringLength (50)]
        public string UserName { get; set; }  

        [Required,StringLength(100)]
        public string Area{get;set;}

        [Required,StringLength(100)]
        public string Landmark{get;set;}

        [Required, MaxLength(6)]
        public int Pincode {get;set;}

        public int Accounts_Id {get;set;}

    }
}