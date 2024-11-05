using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EsyaCart.Data.Entity
{
    public class OrderItems
    {
        [Key]
        public int OrderItems_Id { get; set; }
        
        public int Order_Id { get; set; }

        public int Product_Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get; set; }
        
    }
}