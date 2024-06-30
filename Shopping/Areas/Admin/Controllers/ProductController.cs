using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = dataContext;
			_webHostEnvironment = webHostEnvironment;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
		}
		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");

			return View();



		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
			if(ModelState.IsValid)
			{
				product.Slug = product.Name.Replace(" ", "-");
				var slug=await _dataContext.Products.FirstOrDefaultAsync(p=>p.Slug==product.Slug);
				if(slug!=null)
				{
					ModelState.AddModelError("", "Product already existed");
					return View(product);
				}
				
					if(product.ImageUpload!=null)
					{
						string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
						string imageName=Guid.NewGuid().ToString()+"_"+product.ImageUpload.FileName;
						string filePath=Path.Combine(uploadDir, imageName);
						FileStream fs=new FileStream(filePath, FileMode.Create);
						await product.ImageUpload.CopyToAsync(fs);
						fs.Close();
						product.Image = imageName;

					}
				
				_dataContext.Products.Add(product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Add product successfully";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Some fields are missing";
				List<string> errors = new List<string>();
				foreach(var value in ModelState.Values)
				{
					foreach(var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);

					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);

			}
			
		}
		public async Task<IActionResult> Edit(int Id)
		{ProductModel product=await _dataContext.Products.FindAsync(Id);
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
			return View(product);
		}
			[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
			var existed_product=_dataContext.Products.Find(product.Id);
			if (ModelState.IsValid)
			{
				existed_product.Slug = product.Name.Replace(" ", "-");

				if (product.ImageUpload != null)
				{
					string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadDir, imageName);
					string oldFilePath = Path.Combine(uploadDir, product.Image);
					try
					{
						if (System.IO.File.Exists(oldFilePath))
						{
							System.IO.File.Delete(oldFilePath);

						}
					}
					catch (Exception ex)
					{
						ModelState.AddModelError("", "An error occured while deleting the product image");
					}
					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					existed_product.Image = imageName;

				}
				existed_product.Name = product.Name;
				existed_product.Description = product.Description;
				existed_product.Price = product.Price;
				existed_product.CategoryId = product.CategoryId;
				existed_product.BrandId = product.BrandId;




				_dataContext.Update(existed_product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Update product successfully";
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
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			
				string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
				string oldFilePath = Path.Combine(uploadDir, product.Image);
				try
				{
					if (System.IO.File.Exists(oldFilePath))
					{
						System.IO.File.Delete(oldFilePath);

					}
				}
			catch(Exception ex) {
				ModelState.AddModelError("", "An error occured while deleting the product image");
			}
			_dataContext.Products.Remove(product);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Delete Successfully";
			return RedirectToAction("Index");
		}
	}
}
