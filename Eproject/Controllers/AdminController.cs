using Eproject.Models;
using Eproject.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            List<Product> products = await _dbcontext.Products.ToListAsync();
            return View(products);
        }



    }
}
