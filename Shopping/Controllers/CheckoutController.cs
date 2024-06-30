using Microsoft.AspNetCore.Mvc;
using Shopping.Models;
using Shopping.Repository;
using System.Security.Claims;

namespace Shopping.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail=User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login","Account");
			}
			else
			{
				var ordercode=Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = ordercode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cart in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName= userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId=cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quantity = cart.Quantity;
					_dataContext.Add(orderdetails);
					_dataContext.SaveChanges();


				}
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Creating order successfully";
				return RedirectToAction("Index","Cart");
			}
			return View();
		}
	}
}
