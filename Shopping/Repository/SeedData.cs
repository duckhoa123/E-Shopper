using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if(!_context.Products.Any())
			{
				CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is one of the best laptop in 2024", Status = 1 };
				CategoryModel gaming = new CategoryModel { Name = "Gaming", Slug = "gaming", Description = "Gaming laptop with high resolution", Status = 1 };
				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is still popular in 2024 ", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "samsung", Description = "Samsung alwways update with the latest technology", Status = 1 };
				_context.Products.AddRange(
					new ProductModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is the best", Image = "1.jpg", Category = macbook, Brand = apple, Price = 1233 },
					new ProductModel { Name = "Pc", Slug = "pc", Description = "Pc is the best", Image = "1.jpg", Category = gaming, Brand = samsung, Price = 1233 }
					);
				_context.SaveChanges();
			}
		}
	}
}
