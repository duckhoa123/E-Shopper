using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext dataContext)
		{
			_dataContext = dataContext;

		}

		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
		}
		[HttpGet]
		public IActionResult Create()
		{


			return View();



		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BrandModel brand)
		{

			if (ModelState.IsValid)
			{
				brand.Slug = brand.Name.Replace(" ", "-");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Brand already existed");
					return View(brand);
				}



				_dataContext.Add(brand);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Add brand successfully";
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
			BrandModel brand = await _dataContext.Brands.FindAsync(Id);


			_dataContext.Remove(brand);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Delete Successfully";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int Id)
		{
			BrandModel brand = await _dataContext.Brands.FindAsync(Id);

			return View(brand);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(BrandModel brand)
		{


			if (ModelState.IsValid)
			{
				brand.Slug = brand.Name.Replace(" ", "-");
				_dataContext.Update(brand);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Update brand successfully";
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
