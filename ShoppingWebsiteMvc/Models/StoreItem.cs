using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsiteMvc.Models
{
    public class StoreItem
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal PriceGBP { get; set; }
    }
}
