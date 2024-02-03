using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using ShoppingApp.Models;
using ShoppingApp.ViewModels;
using System.Security.Claims;

namespace ShoppingApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
       
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginVM)
        {
           
            if (ModelState.IsValid)
            {
                //check db
                ApplicationUser userModel = await _userManager.FindByNameAsync(loginVM.UserName);
                if (userModel != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(userModel, loginVM.Password);

                    if (found)
                    {
                        //cookie
                        if (userModel.Active)
                        {
                            await signInManager.SignInAsync(userModel, loginVM.RememberMe);

                            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

                            if (string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase))
                                return RedirectToAction("Index", "Seller");

                            else if (string.Equals(role, "seller", StringComparison.OrdinalIgnoreCase))
                            {
                                if (userModel.Active)
                                    return RedirectToAction("Index", "Product");
                               
                            }

                            else //user
                                return RedirectToAction("HomePage", "Order");
                        }
                        else
                        {
                            ViewBag.Message = "Inactive";
                            return View();
                        }

                    }
                }
                ModelState.AddModelError("", "username or password wrong");

            }
            return View(loginVM);

        }


        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel newUserVM)
        {
            return await Register(newUserVM, "User");

        }

        //[Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult RegisterSeller()
        {
            return View();
        }
       
        
       // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterSeller(RegisterUserViewModel newUserVM)
        {
            return await Register(newUserVM, "Seller");
        }

       // [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterUserViewModel newUserVM)
        {
            return await Register(newUserVM, "Admin");
        }
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task<IActionResult> Register (RegisterUserViewModel newUserVM , string role)
        {
            if (ModelState.IsValid)
            {
                //Mapping from vm to model
                ApplicationUser userModel = new ApplicationUser();
                userModel.UserName = newUserVM.UserName;
                userModel.Address = newUserVM.Address;
                userModel.PasswordHash = newUserVM.Password;

                // save db
                IdentityResult result = await _userManager.CreateAsync(userModel, newUserVM.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userModel, role);
                    //create cookie 
                    await signInManager.SignInAsync(userModel, false);//id , name ,role in cookie
                    if (string.Equals(role, "seller", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Index", "Seller");
                    }
                    else if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Index", "Seller");
                    }
                    else 
                    {
                        return RedirectToAction("HomePage", "Order");
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("Password", item.Description);
                    }
                }
            }
            return View(newUserVM);
        }
       

        private async Task CreateRoles()
        {
            //initializing custom roles 
            
            string[] roleNames = { "Admin", "Seller", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
