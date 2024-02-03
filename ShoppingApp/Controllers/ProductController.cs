using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Services;
using ShoppingApp.ViewModels;
using System.Security.Claims;

namespace ShoppingApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IProductServices _productServices;

        public ProductController(ICategoriesService categoriesService, IProductServices productServices)
        {
            _categoriesService = categoriesService;
            _productServices = productServices;
        }

        public IActionResult Index()
        {
            var products = _productServices.GetAll(GetSellerID());
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductFormViewModel ViewModel = new()
            {
                //projection (list -> SelectListItem)
                Categories = _categoriesService.GetSelectList(),

            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();

                return View(model);
            }
            // save product in DB
            // save cover in server
            model.SellerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            await _productServices.Create(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productServices.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            EditProductFormViewModel viewModel = new()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Categories = _categoriesService.GetSelectList(),
                CurrentCover = product.Image,
                SellerId = product.SellerId,
                Price = product.Price,
                Quantity = product.Quantity,
                Available = product.Available,
                ExpireDate = product.ExpireDate,
               
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();

                return View(model);
            }
            // update product in DB
            // update cover in server
            var product = await _productServices.Edit(model);
            if (product is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var product = _productServices.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _productServices.Delete(id);
            return isDeleted ? RedirectToAction(nameof(Index)) : BadRequest();
        }
        private string GetSellerID()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return userId;
        }
    }
}
