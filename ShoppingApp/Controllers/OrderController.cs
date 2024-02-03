using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using ShoppingApp.Models;
using ShoppingApp.Services;
using ShoppingApp.ViewModels;
using System.Security.Claims;

namespace ShoppingApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IProductServices _productServices;
        private readonly IOrderServices _orderServices;
        private string searchString;

        public OrderController( ICategoriesService categoriesService, IProductServices productServices,IOrderServices orderServices)
        {
            _categoriesService = categoriesService;
            _productServices = productServices;
            _orderServices = orderServices;
        }

        public IActionResult HomePage()
        {
            HomePageViewModel viewModel = new HomePageViewModel();
            viewModel.Categories = _categoriesService.GetCategories();
            viewModel.Products = _productServices.GetAll();
             
            return View(viewModel);
            
        }

        public IActionResult CategoryFilter(int id)
        {
            var Products = _productServices.GetByCategory(id);
            return PartialView("_ProductFilterPartialView", Products);
        }
        public IActionResult AllProducts()
        {
            var Products = _productServices.GetAll();
            return PartialView("_ProductFilterPartialView", Products);
        }

        public IActionResult ProductDetails(int id)
        {
            Product Model = _productServices.GetById(id);
            ProductCartViewModel viewModel = new()
            {
                Id = id,
                Name = Model.Name,
                Description = Model.Description,
                CategoryId = Model.CategoryId,
                Categories = _categoriesService.GetSelectList(),
                CurrentCover = Model.Image,
                SellerId = Model.SellerId,
                Price = Model.Price,
                Quantity = Model.Quantity,
                Available = Model.Available,
                ExpireDate = Model.ExpireDate,
            };

            return View(viewModel);
        }
     
        [HttpPost]
        public IActionResult AddToOrder(ProductOrder item,int id)
        {
            if (item == null)
            {
                RedirectToAction(nameof(HomePage));
            }
            item.ProductId=id;
            //search for current order = true && useid == this user
            Order order = _orderServices.GetCurrentOrder(GetUserID());
            if (order is null)
            {
                order = _orderServices.CreateOrder(GetUserID());

            }
               
                _orderServices.AddItemToOrder(item, order);

            
            return RedirectToAction(nameof(HomePage));//redirect to cart
        }
        public IActionResult DeleteItemFromCart(int ProductId)
        {
            
            //search for current order = true && useid == this user
            Order order = _orderServices.GetCurrentOrder(GetUserID());
            if (order is null)
            {
                ModelState.AddModelError("", "NO Items IN Order To Delete It");

            }
            else
            {
                _orderServices.DeleteItem(ProductId, order);
            }

            return RedirectToAction(nameof(HomePage));//redirect to cart

        }
        public IActionResult Cart()
        {
          var model=  _orderServices.GetCurrentOrderItems(GetUserID());
            if (model is null)
            {
                return View(null);
            }
            List<ProductCartViewModel> cart = new List<ProductCartViewModel>();
            foreach (var item in model) {
                ProductCartViewModel viewModel = new()
                {
                    Id = item.ProductId,
                    Name = item.Product.Name,
                    Description = item.Product.Description,
                    CategoryId = item.Product.CategoryId,
                    CurrentCover = item.Product.Image,
                    SellerId = item.Product.SellerId,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                };
                cart.Add(viewModel);
            }
            return View(cart);
        }
       
        
        public IActionResult CheckOut()
        {
            var order = _orderServices.GetCurrentOrder(GetUserID());
            if (order is not null)
            {
              var Success=  _orderServices.CheckOut(order);
                if (!Success)
                    ModelState.AddModelError("","Error While Checking Out");
            }
            return RedirectToAction(nameof(HomePage));
        }
        public IActionResult UserHistory()
        {
           var userHistory = _orderServices.GetHistoryPerUser(GetUserID());
            if (userHistory is null)
            {
                return View(null);
            }
           
            return View(userHistory);
        }

        private string GetUserID()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return userId;
        }
    }
}
