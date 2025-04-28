using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsiteMvc.Models
{
    [PrimaryKey(nameof(CustomerId), nameof(ItemId))]
    public class CartItem
    {
        public required string CustomerId { get; set; }
        public required int ItemId { get; set; }
        public CustomerIdentityUser Customer { get; set; } = null!;

        public StoreItem Item { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
