using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data;
using ShoppingApp.Models;

namespace ShoppingApp.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;

        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories
                            .OrderBy(c => c.Name)
                            .ToList();
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.Categories
                            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                            .OrderBy(c => c.Text)
                            .AsNoTracking()
                            .ToList();
        }
        public void CreateCategory(Category model)
        {
            Category category = new()
            {
                Name = model.Name,
            };
            _context.Add(category);
            _context.SaveChanges();
           
        }
        public Category? GetById(int id)
        {
            var category = _context.Categories.Find(id);
            return category;
        }

        public Category? Edit(Category model)
        {
            var Category = _context.Categories.Find(model.Id);
            if (Category == null)
                return null;

            Category.Name = model.Name;
            _context.SaveChanges();
            return Category;
        }

        public bool Delete(int id)
        {
            var isDeleted = false;
            var category = _context.Categories.Find(id);

            if (category is null)
                return isDeleted;

            _context.Remove(category);

            var effectedRows = _context.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;

            }
            return isDeleted;
        }

    }
}
