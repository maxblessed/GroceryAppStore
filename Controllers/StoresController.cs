#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroceryStoresApp.Data;
using GroceryStoresApp.Models.StoresModel;
using GroceryStoresApp.ViewModels;
using GroceryStoresApp.Models.CategoryModel;
using Microsoft.AspNetCore.Authorization;

namespace GroceryStoresApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IStoreRepo _storeRepo;
        private readonly ICategory _categories;
        private readonly string _file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        public StoresController(ApplicationDbContext context, ICategory category, IStoreRepo bookrepo)
        {
            _context = context;
            _categories = category;
            _storeRepo = bookrepo;
        }


      
       

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(_storeRepo.AllStores());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = _storeRepo.GetStoresById(id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            StoreCustViewModel bookandCustViewModel = new StoreCustViewModel();
            bookandCustViewModel.categories = _categories.AllCategories();

            return View(bookandCustViewModel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreCustViewModel store)
        {
            if (store.stores != null)
            {
                var file = store.stores.img;
                string path = Path.Combine(_file, DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                store.stores.imgname = path;
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                await _storeRepo.Update(store.stores);
                return RedirectToAction(nameof(Index));
            }
            StoreCustViewModel bookandCustViewModel = new StoreCustViewModel();
            bookandCustViewModel.categories = _categories.AllCategories();
            return View(bookandCustViewModel);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = _storeRepo.GetStoresById(id); ;
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }


        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,author,content,category,price,img")] Store store)
        {
            if (id != store.id)
            {
                return NotFound();
            }

            if (store != null)
            {
                try
                {
                    var file = store.img;
                    string path = Path.Combine(_file, DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    store.imgname = path;
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }
                    await _storeRepo.Update(store);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!storeExists(store.id))
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
            return View(store);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = _storeRepo.GetStoresById(id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }
        // POST: Books/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await _storeRepo.Delete(_storeRepo.GetStoresById(id));
            return RedirectToAction(nameof(Index));
        }

        private bool storeExists(int id)
        {
            return _storeRepo.Any(id);
        }
    }
}
