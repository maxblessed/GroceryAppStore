#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroceryStoresApp.Data;
using GroceryStoresApp.Models.Cart;
using GroceryStoresApp.Models.StoresModel;
using GroceryStoresApp.Models.CategoryModel;
using Microsoft.AspNetCore.Authorization;
using GroceryStoresApp.ViewModels;

namespace GroceryStoresApp.Controllers
{
    public class ShoppingCartItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStoreRepo _storeRepo;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartItemsController(ApplicationDbContext context, ShoppingCart shoppingCart, IStoreRepo bookrepo)
        {
            _context = context;
            _shoppingCart = shoppingCart;
            _storeRepo = bookrepo;
        }
        [AllowAnonymous]
        // GET: ShoppingCartItems
        public async Task<IActionResult> Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModelcs
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
           
        }

        // GET: ShoppingCartItems/Details/5
        public RedirectToActionResult AddToShoppingCart(int id)
        {
            var selectStore = _storeRepo.GetStoresById(id);

            if (selectStore != null)
            {
                _shoppingCart.AddToCart(selectStore, 1);
            }
            return RedirectToAction("Index");
        }

        // GET: ShoppingCartItems/Create
        public RedirectToActionResult RemoveFromShoppingCart(int id)
        {
            var selectedMovie = _storeRepo.GetStoresById(id);

            if (selectedMovie != null)
            {
                _shoppingCart.RemoveFromCart(selectedMovie);
            }
            return RedirectToAction("Index");
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShoppingCartItemId,NoOfItems,ShoppingCartId")] ShoppingCartItem shoppingCartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingCartItem);
        }

        

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShoppingCartItemId,NoOfItems,ShoppingCartId")] ShoppingCartItem shoppingCartItem)
        {
            if (id != shoppingCartItem.ShoppingCartItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartItemExists(shoppingCartItem.ShoppingCartItemId))
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
            return View(shoppingCartItem);
        }

        // GET: ShoppingCartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartItem = await _context.ShoppingCartItem
                .FirstOrDefaultAsync(m => m.ShoppingCartItemId == id);
            if (shoppingCartItem == null)
            {
                return NotFound();
            }

            return View(shoppingCartItem);
        }

        // POST: ShoppingCartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCartItem = await _context.ShoppingCartItem.FindAsync(id);
            _context.ShoppingCartItem.Remove(shoppingCartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartItemExists(int id)
        {
            return _context.ShoppingCartItem.Any(e => e.ShoppingCartItemId == id);
        }
    }
}
