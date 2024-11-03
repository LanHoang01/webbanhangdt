using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;

namespace WebBanDT.Repository
{
	public class DataContect :IdentityDbContext<AppUserModel>
	{
		public DataContect(DbContextOptions<DataContect> options): base(options)
		{
		
		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }

	}
}
