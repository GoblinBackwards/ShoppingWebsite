using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ShoppingWebsiteMvc.Data;
using ShoppingWebsiteMvc.Models;

namespace ShoppingWebsiteMvc.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<CustomerIdentityUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<CustomerIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Add(int itemId, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (await _context.FindAsync<CartItem>(user.Id, itemId) is CartItem existing) {
                existing.Quantity += quantity;
                _context.Update(existing);
            }
            else
            {
                var item = await _context.StoreItems.FindAsync(itemId);

                if (item == null)
                {
                    return NotFound();
                }

                var cartItem = new CartItem { CustomerId = user.Id, ItemId = itemId, Quantity = quantity };
                user.CartItems.Add(cartItem);
                _context.Update(user);
            }

            _context.SaveChanges();
            string? url = Request.Headers.Referer;
            return url is not null ? Redirect(url) : RedirectToAction("index", "home");
        }
    }
}
