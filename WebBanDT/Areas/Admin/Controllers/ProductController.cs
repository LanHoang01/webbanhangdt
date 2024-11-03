using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebBanDT.Models;
using WebBanDT.Repository;

namespace WebBanDT.Areas.Admin.Controllers
{
	[Area("Admin")]

	public class ProductController : Controller
	{
		private readonly DataContect dataContext;
		private readonly IWebHostEnvironment environment;
		public ProductController(DataContect contect, IWebHostEnvironment webHostEnvironment)
		{
			dataContext = contect;
			environment = webHostEnvironment;
		}
		public async Task<IActionResult> Index()
		{

			return View(await dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
		}
		[HttpGet]
        public IActionResult Create()
        {
			ViewBag.Categories = new SelectList(dataContext.Categories,"Id", "Name");
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name");
            return View();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductModel product)
		{
            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name",product.CategoryId);
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name",product.BrandId);
			if (ModelState.IsValid)
			{//code them du lieu
               
				product.Slug = product.Name.Replace(" ", "-");
				var slug = await dataContext.Products.FirstOrDefaultAsync(p=>p.Slug == product.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Sản phẩm đã có trong database");
					return View(product);
				}
				
				if(product.ImageUpload != null)
					{
						string uploadDir = Path.Combine(environment.WebRootPath,"media/products");
						string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
						string filePath = Path.Combine(uploadDir, imageName);

						FileStream fs = new FileStream(filePath, FileMode.Create);
						await product.ImageUpload.CopyToAsync(fs);
						fs.Close();
						product.Image = imageName;
				}
				
				dataContext.Add(product);
				await dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm sản phẩm thành công";
				return RedirectToAction("Index");
            }
			else
			{
				TempData["error"] = "Model có một vài thứ đang bị lỗi";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
                string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
            }
			
			return View(product);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await dataContext.Products.FirstOrDefaultAsync(c => c.Id == Id, CancellationToken.None);

            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( ProductModel product)
        {
            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name", product.BrandId);

            var existed_product = dataContext.Products.Find(product.Id);//tim sp theo id product

            if (ModelState.IsValid)
            {//code them du lieu

                product.Slug = product.Name.Replace(" ", "-");

                if (product.ImageUpload != null)
                {

                    string uploadDir = Path.Combine(environment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;

                    //delete old picture
                    string oldfilePath = Path.Combine(uploadDir, existed_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleting the product image.");
                    }
                }

                existed_product.Name = product.Name;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;
                existed_product.CategoryId = product.CategoryId;
                existed_product.BrandId = product.BrandId;

                dataContext.Update(existed_product);

                await dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await dataContext.Products.FirstOrDefaultAsync(c => c.Id == Id);
            if (product == null)
            {
                return NotFound();
            }
                string uploadDir = Path.Combine(environment.WebRootPath, "media/products");
                string oldfilePath = Path.Combine(uploadDir, product.Image);
            try
            {
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }
            catch (Exception ex) 
            {
                    ModelState.AddModelError("", "An error occurred while deleting the product image.");
            }
            dataContext.Products.Remove(product);
            await dataContext.SaveChangesAsync();
            TempData["error"] = "San pham da xoa";
            return RedirectToAction("Index");
        }
    }
}
