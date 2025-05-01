using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

        [HttpPost]
        public async Task<IResult> Add(int id, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Results.Unauthorized();

            if (await _context.FindAsync<CartItem>(user.Id, id) is CartItem existing) {
                existing.Quantity += quantity;
            }
            else
            {
                var item = await _context.StoreItems.FindAsync(id);

                if (item == null)
                {
                    return Results.NotFound();
                }

                var cartItem = new CartItem { CustomerId = user.Id, ItemId = id, Quantity = quantity };
                user.CartItems.Add(cartItem);
                _context.Update(user);
            }

            await _context.SaveChangesAsync();
            int cartItemCount = await _context.Entry(user).Collection(u => u.CartItems).Query().CountAsync();
            return Results.Ok(new { cartItemCount });
        }

        [HttpPost]
        public async Task<IResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Results.Unauthorized();

            var item = await _context.FindAsync<CartItem>(user.Id, id);
            if (item is null)
            {
                return Results.NotFound();
            }
            _context.Remove(item);
            await _context.SaveChangesAsync();
            return Results.NoContent();
        }

        [HttpPost]
        public async Task<IResult> ChangeQuantity(int id, int delta)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Results.Unauthorized();

            var item = await _context.FindAsync<CartItem>(user.Id, id);
            if (item is null)
            {
                return Results.NotFound();
            }
            if (item.Quantity == 1 && delta < 0)
            {
                return Results.BadRequest("Quantity cannot be below 1");
            }

            item.Quantity += delta;
            await _context.SaveChangesAsync();
            return Results.Accepted(value: item.Quantity);
        }
    }
}
