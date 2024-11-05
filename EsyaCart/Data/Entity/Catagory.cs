using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Data.Entity
{
    public class Catagory
    {
        [Key]
        public int Catagory_Id { get; set; }
        [Required]
        public string CatagoryName {get;set;}
        [Required]
        public string CategoryDescription {get;set;}
    }
}