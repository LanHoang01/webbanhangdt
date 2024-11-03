using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Diagnostics;
using WebBanDT.Models;
using System.Linq;
using WebBanDT.Repository;
using Microsoft.EntityFrameworkCore;

namespace WebBanDT.Controllers
{
	public class HomeController : Controller
	{
		private readonly DataContect dataContext;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, DataContect context)
		{
			_logger = logger;
			dataContext = context;
		}

		public IActionResult Index()
		{
			var products = dataContext.Products.Include("Category").Include("Brand").ToList();
			return View(products);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statuscode)
		{
			if (statuscode == 404)
			{
				return View("NotFound");
			}
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            
		}

	}
}
