using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;
using WebBanDT.Repository;

namespace WebBanDT.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContect dataContext;
		public BrandController(DataContect contect)
		{
			dataContext = contect;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brand == null) return RedirectToAction("Index");
			var productsByBrand = dataContext.Products.Where(p => p.BrandId == brand.Id);
			return View(await productsByBrand.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
