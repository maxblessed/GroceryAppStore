using GroceryStoresApp.Data;

namespace GroceryStoresApp.Models.StoresModel
{
    public class StoreRepo : IStoreRepo
    {
        //database object use to query database
        private readonly ApplicationDbContext _appDbContext;

        public StoreRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }




        //get all books from database
        public IEnumerable<Store> AllStores()
        {


            return _appDbContext.Store.ToList();

        }


        //get a single book from database
        public Store GetStoresById(int? Id)
        {
            return _appDbContext.Store.FirstOrDefault(m => m.id == Id);
        }

        //save a book in database
        public Task<int> Save(Store store)
        {
            _appDbContext.Store.Add(store);
            return _appDbContext.SaveChangesAsync();

        }

        //update a book in database
        public Task<int> Update(Store store)
        {
            _appDbContext.Store.Update(store);
            return _appDbContext.SaveChangesAsync();
        }

        //delete a book in database
        public Task<int> Delete(Store store)
        {
            _appDbContext.Store.Remove(store);
            return _appDbContext.SaveChangesAsync();
        }
        //check if a book exist in database
        public bool Any(int id)
        {
            return _appDbContext.Store.Any(e => e.id == id);
        }
    }
}
