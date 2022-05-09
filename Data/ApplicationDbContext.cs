using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GroceryStoresApp.Models.StoresModel;
using GroceryStoresApp.Models.CategoryModel;
using GroceryStoresApp.Models.Cart;
using GroceryStoresApp.Models.UserModel;

namespace GroceryStoresApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GroceryStoresApp.Models.StoresModel.Store> Store { get; set; }
        public DbSet<ApplicationUser> Applicationusers { get; set; }
        public DbSet<GroceryStoresApp.Models.CategoryModel.Category> Category { get; set; }
        public DbSet<GroceryStoresApp.Models.Cart.ShoppingCartItem> ShoppingCartItem { get; set; }
    }
}