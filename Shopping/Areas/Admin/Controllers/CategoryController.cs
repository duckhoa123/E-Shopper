using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext dataContext)
		{
			_dataContext = dataContext;
			
		}

		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}
		[HttpGet]
		public IActionResult Create()
		{
			

			return View();



		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryModel category)
		{
			
			if (ModelState.IsValid)
			{
				category.Slug = category.Name.Replace(" ", "-");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Category already existed");
					return View(category);
				}

				

				_dataContext.Add(category);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Add category successfully";
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
		public async Task<IActionResult> Delete(int Id)
		{
			CategoryModel category = await _dataContext.Categories.FindAsync(Id);

		
			_dataContext.Remove(category);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Delete Successfully";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int Id)
		{
			CategoryModel category = await _dataContext.Categories.FindAsync(Id);
		
			return View(category);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CategoryModel category)
		{
			
			
			if (ModelState.IsValid)
			{
				category.Slug = category.Name.Replace(" ", "-");
				_dataContext.Update(category);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Update category successfully";
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

