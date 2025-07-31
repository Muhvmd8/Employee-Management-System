global using Demo.Presentation.Models;
using Demo.Presentation.Utilities;

namespace Demo.Presentation.Controllers;
public class AccountController(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : Controller
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    #region  Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {

        // 1.Server Side Validation 
        if (!ModelState.IsValid) return View(model);


        // Manual Mapping 
        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };


        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
            return RedirectToAction("Login");

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        return View(model);
    }
    #endregion

    #region Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is not null)
        {
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await _signInManager.PasswordSignInAsync
                    (user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid Email Or Password ");
        return View(model);
    }
    #endregion

    public async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    #region Reset Password
    // 
    [HttpGet]
    public IActionResult ForgetPassword() => View();

    public async Task<IActionResult> SendResetPasswordLink(ForgetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // User Exist 
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Email Not Found");
            return View(model);
        }

        // Create the Reset Password Link 
        // 1.Host/Controller/ResetPassword
        // 2. Email ?? 
        // 3. Reset Password Token
        // 4. Create Email Object 
        // 5. Send Email

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var url = Url.Action(nameof(ResetPassword), "Account", new { token, model.Email }, Request.Scheme);

        var email = new Email
        {
            Body = url!,
            Recipient = model.Email,
            Subject = "Password Reset Link "
        };

        MailSettings.SendEmail(email);
        return RedirectToAction(nameof(CheckYourInBox));
    }

    [HttpGet]
    public IActionResult CheckYourInBox() => View();



    [HttpGet]
    public IActionResult ResetPassword(string email, string token)
    {
        TempData["Email"] = email;
        TempData["token"] = token;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        model.Email = (TempData["Email"] as string)!;
        model.Token = (TempData["token"] as string)!;
        if (!ModelState.IsValid) return View(model);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "user Not Found");
            return View(model);
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded) return RedirectToAction(nameof(Login));

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        return View(model);
    }
    #endregion
}

/* Security 
 * 
 * 1.Authentication 
 * Who Are You ?
 * 
 * 2. Authorization
 * what are you allowed to do ?
 * Role Based 
 * Claims 
 * Policy
 * 
 * User => [UserName , Email , ......]  XXXX
 *  Use Microsoft.AspNetCore.Identity.EntityFrameworkCore
 */