using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Models.User
{
    public class ReviewModelClass
    {
      
        [Range(0.0, 5.0, ErrorMessage = "Ratings must be between 0 and 5")]
        public float Ratings { get; set; }

        public string Description { get; set; }
        [Required]
        public int Customer_Id { get; set; }  // Customer_Id == Account.Account_Id

        public int Product_Id { get; set; }
    }
}