using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Repository;
using System.Data;

namespace Shopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Route("Admin/User")]
	//[Authorize(Roles ="Admin")]

	public class UserController : Controller
	{
		private readonly UserManager<AppUserModel> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		//[Route("Index")]
		public IActionResult Index()
		{
			var users = _userManager.Users.ToList();

			var usersWithRoles = users
				.Select(c => new
				{
					User = c,
					Roles = _userManager.GetRolesAsync(c).Result
				})
				.Where(c => c.Roles.Any())
				.Select(c => new UserRole()
				{   Id=c.User.Id,
					Username = c.User.UserName,
					Email = c.User.Email,
					Role = string.Join(",", c.Roles)
				})
				.ToList();
			return View(usersWithRoles);
		}
		[HttpGet]
		//[Route("Create")]
		public IActionResult Create()
		{
			ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
			return View();
		}
		[HttpPost]
		//[Route("Create")]
		public async Task<IActionResult> Create(UserRole model)
		{
			ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
			if (ModelState.IsValid)
			{

				AppUserModel newUser = new AppUserModel { UserName = model.Username, Email = model.Email };
				IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

				//Then we create a user 


				//Add default User to Role Admin    
				if (result.Succeeded)
				{
					TempData["success"] = "Sign up successfully";

					var assign = await _userManager.AddToRoleAsync(newUser, model.Role);
					return Redirect("Index");
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(model);
		}
	
		public async Task<IActionResult> Delete(string id)
		{
			Console.WriteLine("khoa");
			var account = await _userManager.Users.FirstOrDefaultAsync(p => p.Id == id);
			if (account == null) { Console.WriteLine("khoa"); }

			var roles = await _userManager.GetRolesAsync(account);

			string rolename = roles.FirstOrDefault();
			await _userManager.RemoveFromRoleAsync(account, rolename);
			await _userManager.DeleteAsync(account);

			TempData["success"] = "Delete Successfully";
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.Users.FirstAsync(x => x.Id == id);

			var roles = await _userManager.GetRolesAsync(user);

			var userRole = new UserRole
			{   Id=user.Id,
				Username = user.UserName,
				Email = user.Email,
				Role = string.Join(",", roles)
			};
			ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
			return View(userRole);
	}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UserRole user)
		{
			ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");

			var account = await _userManager.Users.FirstOrDefaultAsync(p => p.Id == user.Id);
			
			var roles = await _userManager.GetRolesAsync(account);
			
				string rolename = roles.FirstOrDefault();
				await _userManager.RemoveFromRoleAsync(account, rolename);
			IdentityResult result=await _userManager.AddToRoleAsync(account, user.Role);
		if(result.Succeeded)
			{
				TempData["success"] = "Update user successfully";

				account.UserName = user.Username;
				account.Email = user.Email;
				await _userManager.UpdateAsync(account);
				return RedirectToAction("Index");
			}
			
			
			else
			{
				TempData["error"] = "Some fields are missing";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);

					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);

			}


		}

		
	}



	}

