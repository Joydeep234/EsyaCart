using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Data.Entity
{
    public class VendorDetails
    {
        [Key]
        public int VendorID {get;set;}

        [Required,StringLength(50)]
        public string VendorName { get; set; }

        [Required]
        public bool IsApproved {get;set;}

        [Required,StringLength(100)]
        public string Area{get;set;}

        [Required,StringLength(100)]
        public string Landmark{get;set;}
       
        [Required,MaxLength(6)]
        public int Pincode {get;set;}

        public int Accounts_Id {get;set;}
    }
}