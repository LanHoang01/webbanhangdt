using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanDT.Repository.Components
{
	public class BrandsViewComponent : ViewComponent
	{
		private readonly DataContect dataContect;
		public BrandsViewComponent(DataContect Content)
		{
			dataContect = Content;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await dataContect.Brands.ToListAsync());
	}
}
