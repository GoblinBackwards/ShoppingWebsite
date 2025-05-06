using System.ComponentModel;

namespace ShoppingWebsiteMvc.Models.ViewModels
{
    public class CreateStoreItemViewModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        [DisplayName("Price (GBP):")]
        public decimal PriceGBP { get; set; }
        public IFormFile? Image { get; set; }
    }
}