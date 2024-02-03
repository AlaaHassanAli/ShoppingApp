using ShoppingApp.Settings;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data;
using ShoppingApp.Models;
using ShoppingApp.ViewModels;
using System;

namespace ShoppingApp.Services
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public ProductServices(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{MyFileSettings.ImagesPath}";
        }
   
        public async Task Create(ProductFormViewModel model)
        {
            var CoverName = await SaveCover(model.Cover);
            if(model.ExpireDate is null)
            {
                model.ExpireDate = new DateOnly(2100, 1, 1);//
            }
            Product product = new()
            {
                SellerId = model.SellerId,
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Image = CoverName,
                Price = model.Price,
                Quantity = model.Quantity,
                Available = model.Available,
                ExpireDate = model.ExpireDate


            };
            _context.Add(product);
            _context.SaveChanges();
        }

        public Product? GetById(int id)
        { 
            var product = _context.Products
             .Include(p => p.Category)
             .AsNoTracking()
             .SingleOrDefault(p => p.Id == id);
            return product;
        }
        public bool Delete(int id)
        {
            var isDeleted = false;
            var product = _context.Products.Find(id);

            if (product is null)
                return isDeleted;

            _context.Remove(product);

            var effectedRows = _context.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;

                var path = Path.Combine(_imagesPath, product.Image);
                File.Delete(path);
            }
            return isDeleted;
        }

        public IEnumerable<Product> GetAll(string GetSellerID)//for seller
        {
            var products = _context.Products
             .Include(p => p.Category)
             .Where(p => p.SellerId.Equals(GetSellerID))
             .AsNoTracking()
             .ToList();
            return products;
        } 
        public IEnumerable<Product> GetAll()// for user
        {

            var products = _context.Products
             .Include(p => p.Category)
             .Where(p => (p.Available) && (p.Quantity > 0) && (p.ExpireDate.Value.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0))
             .AsNoTracking()
             .ToList();
            return products;
        } 

        public IEnumerable<Product> GetByCategory(int categoryId) // called by userController
        {
            var products = _context.Products
                       .Where(p => (p.CategoryId == categoryId) && (p.Available) && (p.Quantity>0) && (p.ExpireDate.Value.CompareTo(DateOnly.FromDateTime(DateTime.Now))>0))
                       .AsNoTracking()
                       .ToList();
            return products;
        }

        public async Task<Product?> Edit(EditProductFormViewModel model)
        {
            var product = _context.Products.Find(model.Id);
            if (product == null)
                return null;
            var oldCover = product.Image;

            product.Name = model.Name;
            product.Description = model.Description;
            product.CategoryId = model.CategoryId;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.Available = model.Available;
            product.ExpireDate = model.ExpireDate;
            

            var hasNewCover = model.Cover is not null;

            if (hasNewCover)
            {
                product.Image = await SaveCover(model.Cover!);
            }

            var effectedRows = _context.SaveChanges();
            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var path = Path.Combine(_imagesPath, oldCover);
                    File.Delete(path);
                }
                return product;
            }
            else // if it's not saved in db (sql not executed)-> delete new cover
            {
                var path = Path.Combine(_imagesPath, product.Image);
                File.Delete(path);
                return null;
            }
        }
        private async Task<string> SaveCover(IFormFile cover)
        {
            var CoverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagesPath, CoverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return CoverName;
        }
    }
}
