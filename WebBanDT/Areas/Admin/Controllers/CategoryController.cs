using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using WebBanDT.Models;
using WebBanDT.Repository;

namespace WebBanDT.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly DataContect dataContext;
		public CategoryController(DataContect contect)
		{
			dataContext = contect;
			
		}
		public async Task<IActionResult> Index()
		{

			return View(await dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}


        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await dataContext.Categories.FirstOrDefaultAsync(c => c.Id == Id, CancellationToken.None);

            return View(category);
        }


        public IActionResult Create()
        {
          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
           
            if (ModelState.IsValid)
            {
                //code them du lieu

                category.Slug = category.Name.Replace(" ", "-");
                var slug = await dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }

             
                dataContext.Add(category);
                await dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
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

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {

            if (ModelState.IsValid)
            {
                //code them du lieu

                category.Slug = category.Name.Replace(" ", "-");
                var slug = await dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }


                dataContext.Update(category);
                await dataContext.SaveChangesAsync();
                TempData["success"] = "Update danh mục thành công";
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

            return View(category);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await dataContext.Categories.FirstOrDefaultAsync(c => c.Id == Id);
           
            dataContext.Categories.Remove(category);
            await dataContext.SaveChangesAsync();
            TempData["success"] = "Danh muc da xoa";
            return RedirectToAction("Index");
        }


    }
}
