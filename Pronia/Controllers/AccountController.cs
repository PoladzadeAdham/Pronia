using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.ViewModels.UserViewModel;
using System.Threading.Tasks;

namespace Pronia.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var isExistUserName = await userManager.FindByNameAsync(vm.UserName);

            if (isExistUserName is { })
            {
                ModelState.AddModelError("Username", "This username is already exist");
                return View(vm);
            }

            var isExistEmail = await userManager.FindByEmailAsync(vm.EmailAddress);
            if (isExistEmail is { })
            {
                ModelState.AddModelError(nameof(vm.EmailAddress), "This email is already exist");
            }

            AppUser appUser = new AppUser()
            {
                FullName = vm.FirstName + " " + vm.LastName,
                Email = vm.EmailAddress,
                UserName = vm.UserName,
            };

            var result = await userManager.CreateAsync(appUser, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(vm);
            }

            await signInManager.SignInAsync(appUser, false);

            return RedirectToAction("Index", "Home");

        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid) return View(vm);


            var user = await userManager.FindByEmailAsync(vm.EmailAddress);

            if (user is null)
            {
                ModelState.AddModelError("EmailAddress", "Username or password is wrong");
                return View(vm);

            }

            var loginResult = await userManager.CheckPasswordAsync(user, vm.Password);

            if (!loginResult)
            {
                ModelState.AddModelError("EmailAddress", "Username or password is wrong");
                return View(vm);
            }


            await signInManager.SignInAsync(user, vm.IsRemember);

            return Ok($"{user.FullName} welcome");

        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }


        public async Task<IActionResult> CreateRoles()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "User",
            });
            
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Admin",
            });
            
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Moderator",
            });

            return Ok("Roles created");
        }

    }
}
