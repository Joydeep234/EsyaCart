using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EsyaCart.Data.Entity
{

    public class Orders
    {
        [Key]
        public int Order_id { get; set; }

        public DateTime OrderedDate { get; set; }

        public double TotalPrice { get; set; }

        public int Customer_Id { get; set; } // Customer_Id = Account.Account_Id;

        public bool isDelivered{ get; set; } = true;

    }
}