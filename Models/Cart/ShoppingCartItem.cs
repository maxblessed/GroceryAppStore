using GroceryStoresApp.Models.StoresModel;

namespace GroceryStoresApp.Models.Cart
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Store store { get; set; }
        public int NoOfItems { get; set; }
        public string ShoppingCartId { get; set; }
    }
}
