using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace EsyaCart.Models.Vendor
{
    public class EditProductModel
    {
        [Required(ErrorMessage = "This feild can't be empty")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public double Price { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Thid feild can't be empty")]
        public string Description { get; set; }

    }
}
