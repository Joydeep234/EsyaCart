using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace EsyaCart.Models.Vendor
{
    public class AddProductModel
    {
        [Required]
        public int Category_Id { get; set; }

        [Required(ErrorMessage = "ProductName is Required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Quantity is Required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        [StringLength(maximumLength: 200)]
        public string Descirption { get; set; }

        [Required, DataType(DataType.Upload)]
        public IFormFile ImageUrl { get; set; }

    }
}
