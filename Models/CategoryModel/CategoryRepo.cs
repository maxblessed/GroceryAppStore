using GroceryStoresApp.Data;

namespace GroceryStoresApp.Models.CategoryModel
{
    public class CategoryRepo : ICategory
    {
         
        private readonly ApplicationDbContext _appDbContext;
        //get all fruit from database
        public CategoryRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Category> AllCategories()
        {


            return _appDbContext.Category.ToList();

        }
    }
}

