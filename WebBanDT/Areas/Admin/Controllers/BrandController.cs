using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;
using WebBanDT.Repository;

namespace WebBanDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly DataContect dataContext;
        public BrandController(DataContect contect)
        {
            dataContext = contect;

        }
        public async Task<IActionResult> Index()
        {

            return View(await dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await dataContext.Brands.FirstOrDefaultAsync(c => c.Id == Id, CancellationToken.None);

            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {

            if (ModelState.IsValid)
            {
                //code them du lieu

                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã có trong database");
                    return View(brand);
                }


                dataContext.Add(brand);
                await dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công";
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

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {

            if (ModelState.IsValid)
            {
                //code them du lieu

                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã có trong database");
                    return View(brand);
                }


                dataContext.Update(brand);
                await dataContext.SaveChangesAsync();
                TempData["success"] = "Update thương hiệu thành công";
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

            return View(brand);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await dataContext.Brands.FirstOrDefaultAsync(c => c.Id == Id);

            dataContext.Brands.Remove(brand);
            await dataContext.SaveChangesAsync();
            TempData["success"] = "Thương hiệu đã bị xóa";
            return RedirectToAction("Index");
        }

    }
}
