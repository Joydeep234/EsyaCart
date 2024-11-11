using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace EsyaCart.Models.Vendor
{
    public class EditProductModel
    {
        public string ProductName { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

    }
}
