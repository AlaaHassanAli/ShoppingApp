using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;
using ShoppingApp.Services;
using ShoppingApp.ViewModels;

namespace ShoppingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
      

        public UserController( UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");
            if (users != null)
            {
                List<UserViewModel> usersvm = new List<UserViewModel>();
                foreach (var item in users)
                {
                    UserViewModel user = new UserViewModel()
                    {
                        Name = item.UserName,
                        Password = item.PasswordHash,
                        Id = item.Id,
                    };
                    usersvm.Add(user);
                }
                return View(usersvm);
            }
            else return Content("No user yet");
        }
      
        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            UserViewModel viewModel = new()
            {
                Id = user.Id,
                Name = user.UserName,
                Address = user.Address

            };
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserAsync(UserViewModel viewModel)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(viewModel.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.UserName = viewModel.Name;
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
