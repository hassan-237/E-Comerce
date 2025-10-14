using Eproject.ViewModels;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eproject.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context; 
        public AuthController(AppDbContext Dbcontext)
        {
            _context = Dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            if(User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            RegisterVM registerVM = new RegisterVM();
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            try
            {
                // Check if already logged in
                if (User.Identity?.IsAuthenticated ?? false)
                {
                    return RedirectToAction("Index", "Home");
                }
                // Check if user exists
                var finduser = await _context.Users.AnyAsync(u => u.Email == registerVM.Email);
                if (finduser) { throw new Exception("User Already Exists"); }

                //Create User

                var user = new User
                {
                    Email = registerVM.Email,
                    Username = registerVM.Username,
                    Password = registerVM.Password,
                    Role = registerVM.Role
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

            }
            catch { }
            return View();
        }


    }
}
