using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyTicket.Data;
using MyTicket.Models;
using MyTicket.Static;
using MyTicket.ViewModel;

namespace MyTicket.Controllers
{
    public class IdentityController : Controller
    {

        private readonly AppDbContext db;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public IdentityController(AppDbContext db, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await userManager.FindByEmailAsync(registerViewModel.Email);

            if (user != null)
            {
                TempData["Error"] = "The email address is already exists";
                return View(registerViewModel);
            }

            var newUser = new AppUser
            {
                FullName = registerViewModel.FullName,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email
            };

            var result = await userManager.CreateAsync(newUser, registerViewModel.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, UserRoles.User);
                return View("Login");
            }
            else
                TempData["Error"] = "The Password must contains at least one upper case and one digit and one symbol";

            return View(registerViewModel);
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await userManager.FindByEmailAsync(loginViewModel.Email);

            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (passwordCheck)
                {
                    var identityResult = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Movie");
                    }
                }

                TempData["Error"] = "Email or password is wrong!!";

                return View(loginViewModel);
            }

            TempData["Error"] = "Email or password is wrong!!";

            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movie");
        }

        public async Task<IActionResult> Users()
        {
            var users = await db.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await db.Users.SingleOrDefaultAsync(u => u.Id == userManager.GetUserId(User));

            var userProfile = new ProfileViewModel()
            {
                Email = user.Email,
                FullName = user.FullName,
            };

            return View(userProfile);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel profileViewModel)
        {

            var user = await userManager.GetUserAsync(User);
            user.Email = profileViewModel.Email;
            user.FullName = profileViewModel.FullName;

            if (!profileViewModel.Password.IsNullOrEmpty())
            {
                var passswordValidator = new PasswordValidator<AppUser>();
                var validationResult = await passswordValidator.ValidateAsync(userManager, user, profileViewModel.Password);

                if (validationResult.Succeeded)
                {
                    TempData["PasswordSucceeded"] = "Password updated successfully";
                    user.PasswordHash = userManager.PasswordHasher.HashPassword(user, profileViewModel.Password);
                }
                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["FullNameSucceeded"] = "Full Name updated successfully";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("Profile", profileViewModel);
        }
    }
}
