using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;
using WebBanDT.Repository;
namespace WebBanDT.Controllers
{
	public class CategoryController:Controller
	{
		private readonly DataContect dataContext;
		public CategoryController(DataContect contect)
		{
			dataContext = contect;
		}
		public async Task<IActionResult> Index(string Slug="")
		{
			CategoryModel category = dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
            if (category==null) return RedirectToAction("Index");
			var productsByCategory = dataContext.Products.Where(p => p.CategoryId == category.Id);
            return View(await productsByCategory.OrderByDescending(p => p.Id).ToListAsync());
		}

	}
}
