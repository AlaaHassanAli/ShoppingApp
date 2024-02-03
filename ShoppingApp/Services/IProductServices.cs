using ShoppingApp.Models;
using ShoppingApp.ViewModels;

namespace ShoppingApp.Services
{
    public interface IProductServices
    {
        IEnumerable<Product> GetAll(string GetSellerID);
        IEnumerable<Product> GetAll();
        public Product? GetById(int id);

        IEnumerable<Product> GetByCategory(int categoryId);
        Task Create(ProductFormViewModel product);
        Task<Product?> Edit(EditProductFormViewModel product);
        bool Delete(int id);

    }
}
