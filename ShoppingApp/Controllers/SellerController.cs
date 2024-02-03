using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingApp.Models;
using ShoppingApp.Services;
using ShoppingApp.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;

namespace ShoppingApp.Controllers
{
    public class SellerController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellerController(ICategoriesService categoriesService,UserManager<ApplicationUser> userManager)
        {
            _categoriesService = categoriesService;
            this._userManager = userManager;

        }

        //get all sellers
        public async Task<IActionResult> Index()
        {
            var sellers= await _userManager.GetUsersInRoleAsync("Seller");
            if (sellers != null)
            { List <SellerViewModel> sellersVM = new List<SellerViewModel>();
                foreach(var item in sellers)
                {
                    SellerViewModel seller = new SellerViewModel()
                    {
                        Name = item.UserName,
                        Password = item.PasswordHash,
                        Id = item.Id,
                        active = item.Active,
                    };
                    sellersVM.Add(seller);
                }
                return View(sellersVM);
            }
            else return Content("No seller yet");
        }
        [HttpGet]
        public async Task<IActionResult> EditSeller(string Id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            SellerViewModel viewModel = new()
            {
                Id = user.Id,
                Name = user.UserName,
                active= user.Active,
                Address = user.Address

            };
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSellerAsync(SellerViewModel viewModel)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(viewModel.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.UserName = viewModel.Name;
            user.Active = viewModel.active;
            user.Address = viewModel.Address;
            var IdentityResult = await _userManager.UpdateAsync(user);

            if (!IdentityResult.Succeeded)
            {
                foreach (var item in IdentityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return RedirectToAction(nameof(Index));

        }
    
        

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string Name)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(Name);
            if (user == null)
            {
                return NotFound();
            }
            ResetPasswordSellerViewModel viewModel = new()
            {
                SellerName = user.UserName,
                
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordSellerViewModel viewModel)
        {          
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(viewModel.SellerName);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, viewModel.NewPassword);
                if (passwordChangeResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var item in passwordChangeResult.Errors)
                    {
                        ModelState.AddModelError("NewPassword", item.Description);
                    }
                }

            }
            return View(viewModel);

        }

    



    }
}

