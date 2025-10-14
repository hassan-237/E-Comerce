using Eproject.Models;
using Eproject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eproject.Controllers
{
    public class AdminController : Controller
    {
        public readonly AppDbContext _dbcontext;
        public AdminController(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateCategories()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategories(Category category)
        {
            await _dbcontext.Categories.AddAsync(category);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction("ShowCategories");
        }

        public async Task<IActionResult> ShowCategories()
        {
            List<Category> categories = await _dbcontext.Categories.ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> UpdateCat(int id)
        {
            var cat = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.CatID == id);
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Category = cat;
            categoryVM.ErrorMessage = "";
            return View(categoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCat(Category category)
        {
            try
            {
                var cat = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.CatID == category.CatID);
                if(cat == null)
                {
                    throw new Exception("Category Not Found");
                }
                cat.Cat_Name = category.Cat_Name;
                _dbcontext.Categories.Update(cat);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("ShowCategories");
            }
            catch (Exception ex)
            {
                CategoryVM categoryVM = new CategoryVM();
                categoryVM.Category = category;
                categoryVM.ErrorMessage = ex.Message;
                return View(categoryVM);
            }
        }

        public async Task<IActionResult> DeleteCat(int id)
        {
            var cat = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.CatID == id);
            _dbcontext.Categories.Remove(cat);
            await _dbcontext.SaveChangesAsync();
            TempData["Message"] = "Category deleted Succesfully";
            return RedirectToAction("ShowCategories");
        }
        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> ShowProducts()
        {
            List<Product> products = await _dbcontext.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _dbcontext.Categories.ToListAsync();
            ProductFormVM productFormVM = new ProductFormVM
            {
                Categories = new SelectList(categories, "CatID", "Cat_Name")
            };
            return View(productFormVM);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductFormVM productFormVM)
        {
            var uploards = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images");
            //create directory
            Directory.CreateDirectory(uploards);

            var filename = productFormVM.ImageFile.FileName;
            var fileupload = Path.Combine(uploards, filename);

            using (var stream = new FileStream(fileupload, FileMode.Create))
            {
                await productFormVM.ImageFile.CopyToAsync(stream);
            }
            productFormVM.Product.ImageName = filename;

            await _dbcontext.Products.AddAsync(productFormVM.Product);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction("ShowProducts");
        }

        public async Task<IActionResult> UpdateProduct(int id)
        {
            var Product = await _dbcontext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ID == id);
            var Category = await _dbcontext.Categories.ToListAsync();
            ProductFormVM productFormVM = new ProductFormVM()
            {
                Categories = new SelectList(Category, "CatID", "Cat_Name")
            };
            productFormVM.Product = Product;
            productFormVM.ErrorMessage = "";
            return View(productFormVM);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductFormVM productFormVM)
        {
            try
            {
                var getProduct = await _dbcontext.Products.FirstOrDefaultAsync(p => p.ID == productFormVM.Product.ID);
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images");
                Directory.CreateDirectory(uploads);

                if (!string.IsNullOrEmpty(getProduct?.ImageName))
                {
                    var oldImgPath = Path.Combine(uploads, getProduct.ImageName);
                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }
                //create directory

                var filename = productFormVM.ImageFile.FileName;
                var fileupload = Path.Combine(uploads, filename);
                
                using (var stream = new FileStream(uploads, FileMode.Create))
                {
                    await productFormVM.ImageFile.CopyToAsync(stream);
                }

                getProduct.ImageName = filename;

                getProduct.Name = productFormVM.Product.Name;
                getProduct.price = productFormVM.Product.price;
                getProduct.description = productFormVM.Product.description;
                getProduct.qty = productFormVM.Product.qty;
                getProduct.CatID = productFormVM.Product.CatID;
                _dbcontext.Products.Update(getProduct);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("ShowProducts");
            }
            catch(Exception ex)
            {
                throw new Exception("Product not Found");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbcontext.Products.FirstOrDefaultAsync(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            _dbcontext.Products.Remove(product);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction("ShowProducts");
        }

    }
}
