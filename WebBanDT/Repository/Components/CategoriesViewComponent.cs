using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanDT.Repository.Components
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly DataContect dataContect;
		public CategoriesViewComponent(DataContect Content)
		{
			dataContect = Content;
		}
		public async Task<IViewComponentResult> InvokeAsync()=> View(await dataContect.Categories.ToListAsync());
	}
}
