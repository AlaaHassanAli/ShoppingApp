using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingApp.Models;

namespace ShoppingApp.Services
{
    public interface ICategoriesService
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<SelectListItem> GetSelectList();
        void CreateCategory(Category model);
        Category? GetById(int id);
        Category? Edit(Category model);
        bool Delete(int id);




    }
}
