using Microsoft.AspNetCore.Identity;

namespace ShoppingWebsiteMvc.Models
{
    public class CustomerIdentityUser : IdentityUser
    {
        public required List<CartItem> CartItems { get; set; } = [];
    }
}
