using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Repository;

namespace Shopping.Controllers
{
    public class CartController:Controller
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IActionResult Index()
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
        };

            return View(cartVM);
        }
		public async Task<IActionResult> Add(int Id)
		{
            ProductModel product = await _dataContext.Products.FindAsync(Id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems=cart.Where(c=>c.ProductId == Id).FirstOrDefault();
            if(cartItems == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItems.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);
			TempData["success"] = "Add Item to Cart Successfully";
            return Redirect(Request.Headers["Referer"].ToString());


		}
		public async Task<IActionResult> Decrease(int Id)
		{
			
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if(cartItems.Quantity>1)
            {
                --cartItems.Quantity;

			}
            else
            {
                cart.RemoveAll(p=>p.ProductId == Id);
            }
            if(cart.Count == 0) {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);

			}
			TempData["success"] = "Decrease Successfully";
			return RedirectToAction("Index");

		}
		public async Task<IActionResult> Increase(int Id)
		{

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			++cartItems.Quantity;
			HttpContext.Session.SetJson("Cart", cart);

			TempData["success"] = "Increase Successfully";
			return RedirectToAction("Index");

		}
		public async Task<IActionResult> Remove(int Id)
		{

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			
				cart.RemoveAll(p => p.ProductId == Id);
			
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);

			}
			TempData["success"] = "Remove Successfully";
			return RedirectToAction("Index");

		}
		public async Task<IActionResult> Clear()
		{

		
				HttpContext.Session.Remove("Cart");
			TempData["success"] = "Clear Successfully";
			return RedirectToAction("Index");

		}

		public IActionResult CheckOut()
		{
			return View("~/Views/CheckOut/Index.cshtml");
		}

	}
}
