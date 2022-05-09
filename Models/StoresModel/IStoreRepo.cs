namespace GroceryStoresApp.Models.StoresModel
{
    public interface IStoreRepo
    {
        //IBook Interface signature for querying database
        IEnumerable<Store> AllStores();
        Store GetStoresById(int? Id);
        Task<int> Save(Store store);
        Task<int> Update(Store store);
        Task<int> Delete(Store store);
        bool Any(int id);
    }
}
