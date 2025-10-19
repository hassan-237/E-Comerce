using Eproject.ViewModels;
using Eproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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

                // Fetch the saved user
                var newRegUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                // Create claims for cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, newRegUser.Username ?? ""),
                    new Claim(ClaimTypes.Email, newRegUser.Email ?? ""),
                    new Claim(ClaimTypes.Role, newRegUser.Role.ToString() ?? "")
                };

                var Identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(Identity);

                await HttpContext.SignInAsync("MyCookieAuth",principal);
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                RegisterVM vm = new RegisterVM
                {
                    ErrorMessage = ex.Message
                };
                return View(vm);
            }
            
        }

        public IActionResult Login()
        {
            if(User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            try
            {
                if(User.Identity?.IsAuthenticated ?? false)
                {
                    return RedirectToAction("Index", "Home");
                }

                var finduser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginVM.Email && u.Password == loginVM.Password);

                if (User == null)
                {
                    throw new Exception("User Does`nt exists!");
                }

                // Create claims for cookie 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, finduser.Username ?? ""),
                    new Claim(ClaimTypes.Email, finduser.Email ?? ""),
                    new Claim(ClaimTypes.Role, finduser.Role.ToString() ?? "")

                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principle = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principle);

                if(finduser.Role == Enums.UserRole.Admin)
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "Home");

            }

            catch(Exception ex)
            {
                LoginVM login = new LoginVM
                {
                    ErrorMessage = ex.Message
                };
                return View(login);
            }
        }

        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login"); 
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
