using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Models.User
{
    public class CartModelClass
    {
        public int Customer_Id { get; set; }            // Customer_Id = Account.Account_Id

        public int Product_Id { get; set; }

        public int Quantity { get; set; }
    }
}