using GroceryStoresApp.Data;
using GroceryStoresApp.Models.StoresModel;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoresApp.Models.Cart
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _appDbContext;

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        private ShoppingCart(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Store store, int NoOfItems)
        {
            var shoppingCartItem =
                    _appDbContext.ShoppingCartItem.SingleOrDefault(
                        s => s.store.id == store.id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    store = store,
                    NoOfItems = 1
                };

                _appDbContext.ShoppingCartItem.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.NoOfItems++;
            }
            _appDbContext.SaveChanges();
        }

        public int RemoveFromCart(Store store)
        {
            var shoppingCartItem =
                    _appDbContext.ShoppingCartItem.SingleOrDefault(
                        s => s.store.id == store.id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.NoOfItems > 1)
                {
                    shoppingCartItem.NoOfItems--;
                    localAmount = shoppingCartItem.NoOfItems;
                }
                else
                {
                    _appDbContext.ShoppingCartItem.Remove(shoppingCartItem);
                }
            }

            _appDbContext.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       _appDbContext.ShoppingCartItem.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.store)
                           .ToList());
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext
                .ShoppingCartItem
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItem.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingCartItem.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.store.price * c.NoOfItems).Sum();
            return total;
        }
    }
}
