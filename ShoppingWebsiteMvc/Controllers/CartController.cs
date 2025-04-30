using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsiteMvc.Data;
using ShoppingWebsiteMvc.Models;
using ShoppingWebsiteMvc.Models.ViewModels;

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

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var itemsInCart = await _context.Entry(user)
                .Collection(u => u.CartItems)
                .Query()
                .ToArrayAsync();

            foreach (var item in itemsInCart)
            {
                await _context.Entry(item)
                    .Reference(i => i.Item)
                    .LoadAsync();
            }

            CartViewModel viewModel = new() { Items = itemsInCart };

            return View(viewModel);
        }

        public async Task<IActionResult> Add(int itemId, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

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

            await _context.SaveChangesAsync();
            string? url = Request.Headers.Referer;
            return url is not null ? Redirect(url) : RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var item = await _context.FindAsync<CartItem>(user.Id, id);
            if (item is not null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangeQuantity(int id, int delta)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var item = await _context.FindAsync<CartItem>(user.Id, id);
            if (item is not null)
            {
                if (item.Quantity == 0 && delta < 0) return base.BadRequest("Quantity cannot be negative");
                item.Quantity += delta;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
