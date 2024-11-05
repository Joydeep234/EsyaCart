using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Data.Entity
{
    public class Reviews
    {
        [Key]
        public int Review_Id { get; set; }

        public float Ratings { get; set; }

        [Required, StringLength(200)]
        public string Description { get; set; }
        [Required]
        public int Customer_Id { get; set; }  // Customer_Id == Account.Account_Id

        public int Product_Id { get; set; }
    }
}