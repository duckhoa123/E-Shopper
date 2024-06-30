﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext dataContext)
		{
			_dataContext = dataContext;

		}
		public async  Task<IActionResult> Index()
		{
			return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
		}
        public async Task<IActionResult> ViewOrder(string ordercode)
        {var DetailsOrder=await _dataContext.OrderDetails.Include(od=>od.Product).Where(od=>od.OrderCode == ordercode).ToListAsync();
			ViewData["code"] = ordercode;
            return View(DetailsOrder);
        }
    }
}
