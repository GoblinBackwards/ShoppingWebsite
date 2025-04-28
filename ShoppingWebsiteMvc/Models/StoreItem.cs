using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsiteMvc.Models
{
    public class StoreItem
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        [DisplayName("Price")]
        public required decimal PriceGBP { get; set; }
        public required List<CartItem> CartItems { get; set; } = [];
    }
}
