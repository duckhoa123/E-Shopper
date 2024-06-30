using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Controllers
{
    public class CategoryController:Controller
    {
		private readonly DataContext _dataContext;
		
        public CategoryController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index( string Slug="")
		{
			CategoryModel category=_dataContext.Categories.Where(c=>c.Slug == Slug).FirstOrDefault();
			if (category == null) { return RedirectToAction("Index"); }
			var productsByCategory = _dataContext.Products.Where(c => c.CategoryId == category.Id);
			return View( await productsByCategory.OrderByDescending(p=>p.Id).ToListAsync()) ;
		}

	}
}
