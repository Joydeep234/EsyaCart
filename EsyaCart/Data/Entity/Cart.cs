using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Data.Entity
{
    public class Cart
    {
        [Key]
        public int Cart_Id { get; set; }

        public int Customer_Id { get; set; }            // Customer_Id = Account.Account_Id

        public int Product_Id { get; set; }

        public int Quantity { get; set; }

    }
}