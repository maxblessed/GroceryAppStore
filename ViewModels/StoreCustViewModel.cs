using GroceryStoresApp.Models.CategoryModel;
using GroceryStoresApp.Models.StoresModel;

namespace GroceryStoresApp.ViewModels
{
    public class StoreCustViewModel
    {
        public IEnumerable<Category> categories { get; set; }
        public Store stores { get; set; }
    }
}
