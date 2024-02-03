using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShoppingApp.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            var pwd = "Admin@123";
            var passwordHasher = new PasswordHasher<IdentityUser>();

            // Seed Roles
            var adminRole = new IdentityRole("Admin");
            adminRole.NormalizedName = adminRole.Name!.ToUpper();

            var sellerRole = new IdentityRole("Seller");
            sellerRole.NormalizedName = sellerRole.Name!.ToUpper();

            var userRole = new IdentityRole("User");
            userRole.NormalizedName = userRole.Name!.ToUpper();

            List<IdentityRole> roles = new List<IdentityRole>() {
            adminRole,
            sellerRole,
            userRole
            };

            builder.Entity<IdentityRole>().HasData(roles);


            // Seed Users
            //var adminUser = new IdentityUser
            //{
            //    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
            //    UserName = "AdminName",
            //    Email = "aa@aa.aa",
            //    EmailConfirmed = true,
            //};
            //adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
            //adminUser.NormalizedEmail = adminUser.Email.ToUpper();
            //adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, pwd);

           

            //List<IdentityUser> users = new List<IdentityUser>() {
            //    adminUser                
            //};

            //builder.Entity<IdentityUser>().HasData(users);

            //// Seed UserRoles
            //List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            //userRoles.Add(new IdentityUserRole<string>
            //{
            //    UserId = users[0].Id,
            //    RoleId = roles.First(q => q.Name == "Admin").Id
            //});

           


            //builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }
    }

}