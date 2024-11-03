using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanDT.Models;
using WebBanDT.Models.ViewModel;

namespace WebBanDT.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		public AccountController(SignInManager<AppUserModel> sign, UserManager<AppUserModel> userManager) 
		{
			_signInManager = sign;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}
		[HttpPost]
		

		public async Task<IActionResult> Login(LoginViewModel login
			)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false,false);
				if (result.Succeeded)
				{
					return Redirect(login.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", " UserName hoặc Password sai");
			}
			return View(login);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email}; 
				IdentityResult result = await _userManager.CreateAsync(newUser);
				if (result.Succeeded)
				{
					TempData["success"] = "Tạo user thành công";
					return Redirect("/account/login");
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(user);
		}
	}
}
