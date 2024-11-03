using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;

namespace WebBanDT.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContect contect)
		{
			contect.Database.Migrate();
			if (!contect.Products.Any())
			{
				CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "Macbook", Description = "Macbook is Large Brand in the work", Status = 1 };
				CategoryModel pc = new CategoryModel { Name = "Pc", Slug = "Pc", Description = "Pc is Large Brand in the work", Status = 1 };

				BrandModel apple = new BrandModel { Name = "Apple", Slug = "Apple", Description = "Apple is Large Brand in the work", Status = 1 };
				BrandModel ss = new BrandModel { Name = "SamSung", Slug = "SamSung", Description = "SamSung is Large Brand in the work", Status = 1 };
				contect.Products.AddRange(

					new ProductModel { Name = "Macbook", Slug = "Macbook", Description = "Macbook is best", Image = "1.jpg", Category = macbook, Brand = apple, Price = 122 },
					new ProductModel { Name = "Pc", Slug = "Pc", Description = "Pc is best", Image = "1.jpg", Category = pc, Brand = ss, Price = 122 });
			}
			contect.SaveChanges();
		}
		
	}
}
