using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Data.Entity
{
    public class Products
    {
        [Key]
        public int Product_Id { get; set; }

        [Required]
        public int Category_Id { get; set; }
        [Required]
        public int Vendor_Id {get; set;}

        [Required]
        public string ProductName { get; set; }
     
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; } = 0;
        
        [Required,StringLength(200)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
          
        [Required]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public DateTime? updatedAt { get; set; }
    }
}