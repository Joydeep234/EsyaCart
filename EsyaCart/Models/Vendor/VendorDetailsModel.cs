using System.ComponentModel.DataAnnotations;

namespace EsyaCart.Models.Vendor
{
    public class VendorDetailsModel
    {
        [Required(ErrorMessage = "Vendor Name is required")]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Area field is required")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Landmark is required")]
        public string Landmark { get; set; }

        [Required(ErrorMessage = "Pincode is required")]
        public int Pincode { get; set; }

		public bool isApproved { get; set; }

	}
}
