using KursovaDBFinal.Loggers;
using KursovaDBFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KursovaDBFinal.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public AccountController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    // GET
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    // POST
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid) 
            return View();
        var identityUser = new IdentityUser
        {
            UserName = registerViewModel.Username,
            Email = registerViewModel.Email
        };

        var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerViewModel);
        }
        
        var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "Customer");

        if (!roleIdentityResult.Succeeded)
        {
            foreach (var error in roleIdentityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerViewModel);
        }

        await Logger.Log(User?.Identity?.Name ?? "User", "Registered", "Users", DateTime.Now);
        return RedirectToAction("Login");
    }
    
    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username,
            loginViewModel.Password, false, false);

        if (!signInResult.Succeeded)
        {
            switch (signInResult.IsNotAllowed)
            {
                case true:
                    ModelState.AddModelError(string.Empty, "The user account is not allowed to login.");
                    break;
                case false when signInResult.IsLockedOut:
                    ModelState.AddModelError(string.Empty, "The user account is locked out.");
                    break;
                default:
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    break;
            }

            return View();
        }

        if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
        {
            return Redirect(loginViewModel.ReturnUrl);
        }

        await Logger.Log(User?.Identity?.Name ?? "User", "Logged in", "Users", DateTime.Now);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await Logger.Log(User?.Identity?.Name ?? "User", "Logged out", "Users", DateTime.Now);
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}