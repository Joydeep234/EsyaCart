using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Data.Entity
{
    public class Account
    {
        [Key]
        public int Account_Id {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;

        [Required,StringLength(50)]
        public string Email { get; set; }

        [Required,StringLength(10)]
        public string Contact_No { get; set; }

        [Required,StringLength(10)]
        public string Password {get;set;}

        [Required,StringLength(10)]
        public string Account_type { get; set; }

        public bool IsActive { get; set; } = true;

    }
}