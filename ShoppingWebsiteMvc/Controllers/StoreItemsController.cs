using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsiteMvc.Data;
using ShoppingWebsiteMvc.Models;
using ShoppingWebsiteMvc.Models.ViewModels;

namespace ShoppingWebsiteMvc.Controllers
{
    [Authorize(Policy = "HasRoleAdmin")]
    public class StoreItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public StoreItemsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: StoreItems
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.StoreItems.ToListAsync());
        }

        // GET: StoreItems/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.StoreItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storeItem == null)
            {
                return NotFound();
            }

            return View(storeItem);
        }

        // GET: StoreItems/Create
        public IActionResult Create() {
        
            return View();
        }

        // POST: StoreItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PriceGBP,Image")] CreateStoreItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                StoreItem item = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    PriceGBP = model.PriceGBP,
                    CartItems = []
                };

                if (model.Image != null)
                {
                    string fileName = model.Image.FileName;
                    string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(fileName));
                    string relativeFilePath = Path.Combine("item_images", newFileName);
                    string fullFilePath = Path.Combine(_env.WebRootPath, relativeFilePath);
                    await using FileStream stream = new(fullFilePath, FileMode.Create);
                    await model.Image.CopyToAsync(stream);
                    item.ImageFileName = relativeFilePath;
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: StoreItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.StoreItems.FindAsync(id);
            if (storeItem == null)
            {
                return NotFound();
            }
            return View(storeItem);
        }

        // POST: StoreItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PriceGBP")] StoreItem storeItem)
        {
            if (id != storeItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storeItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreItemExists(storeItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(storeItem);
        }

        // GET: StoreItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.StoreItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storeItem == null)
            {
                return NotFound();
            }

            return View(storeItem);
        }

        // POST: StoreItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storeItem = await _context.StoreItems.FindAsync(id);
            if (storeItem != null)
            {
                _context.StoreItems.Remove(storeItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreItemExists(int id)
        {
            return _context.StoreItems.Any(e => e.Id == id);
        }
    }
}
