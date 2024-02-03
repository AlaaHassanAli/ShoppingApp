using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models;
using ShoppingApp.Services;

namespace ShoppingApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoryController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;

        }
        public IActionResult Index()
        {
            return View(_categoriesService.GetCategories());
        }
        

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoriesService.CreateCategory(category);

                return RedirectToAction(nameof(Index));

            }
            return View(category);

        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = _categoriesService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            Category model = new()
            {
                Id = id,
                Name = category.Name,

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category model)
        {
            var category = _categoriesService.Edit(model);
            if (category is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            var isDeleted = _categoriesService.Delete(id);
            //return isDeleted ? RedirectToAction("GetCategories") : BadRequest();
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
