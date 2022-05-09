using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryStoresApp.Models.StoresModel
{
    public class Store
    {
        public int id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public string category { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public string imgname { get; set; }
        [NotMapped]
        public IFormFile img { get; set; }
    }
}
