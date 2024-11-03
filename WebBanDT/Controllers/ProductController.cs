using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using WebBanDT.Repository;

namespace WebBanDT.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContect dataContext;
		public ProductController(DataContect contect)
		{
			dataContext = contect;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Details(int Id)
		{
			if(Id == null) return RedirectToAction("Index");
			var productsById = dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
			return View(productsById);
		}
	}
}
