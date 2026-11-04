namespace CalendarApp.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using CalendarApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CalendarApp.Application.ViewModels;

public class AccountController : Controller
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly RoleManager<IdentityRole> _roleManager;

  // DI
  public AccountController(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      RoleManager<IdentityRole> roleManager)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _roleManager = roleManager;
  }

  // GET: /Account/Register
  [HttpGet]
  [AllowAnonymous]
  public IActionResult Register()
  {
    if (User.Identity?.IsAuthenticated == true)
    {
      return RedirectToAction("Index", "Home");
    }

    return View();
  }

  // POST: /Account/Register
  [HttpPost]
  [AllowAnonymous]
  // poistka proti cross-site request forgery
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Register(RegisterViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return View(model);
    }

    var user = new ApplicationUser
    {
      UserName = model.Email,
      Email = model.Email,
      FirstName = model.FirstName,
      LastName = model.LastName,
      CreatedAt = DateTime.UtcNow
    };

    var result = await _userManager.CreateAsync(user, model.Password);

    if (result.Succeeded)
    {
      if (!await _roleManager.RoleExistsAsync("User"))
      {
        await _roleManager.CreateAsync(new IdentityRole("User"));
      }

      await _userManager.AddToRoleAsync(user, "User");

      await _signInManager.SignInAsync(user, isPersistent: false);

      return RedirectToAction("Index", "Home");
    }

    foreach (var error in result.Errors)
    {
      ModelState.AddModelError(string.Empty, error.Description);
    }

    return View(model);
  }
  // GET: /Account/Login
  [HttpGet]
  [AllowAnonymous]
  public IActionResult Login(string? returnUrl = null)
  {
    if (User.Identity?.IsAuthenticated == true)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewData["ReturnUrl"] = returnUrl;
    return View();
  }

  // POST: /Account/Login
  [HttpPost]
  [AllowAnonymous]
  // poistka proti cross-site request forgery
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
  {
    ViewData["ReturnUrl"] = returnUrl;

    if (!ModelState.IsValid)
    {
      return View(model);
    }

    var result = await _signInManager.PasswordSignInAsync(
        model.Email,
        model.Password,
        model.RememberMe,
        lockoutOnFailure: false);

    if (result.Succeeded)
    {
      if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }

      return RedirectToAction("Index", "Home");
    }

    ModelState.AddModelError(string.Empty, "Neplatný e-mail alebo heslo.");
    return View(model);
  }

  // POST: /Account/Logout
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Logout()
  {
    await _signInManager.SignOutAsync();
    return RedirectToAction("Index", "Home");
  }

  // GET: /Account/AccessDenied
  [HttpGet]
  public IActionResult AccessDenied()
  {
    return View();
  }
}