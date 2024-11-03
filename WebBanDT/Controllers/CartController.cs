using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;
using WebBanDT.Models.ViewModel;
using WebBanDT.Repository;

namespace WebBanDT.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContect dataContext;
		public CartController(DataContect contect)
		{
			dataContext = contect;
		}
		
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")?? new List<CartItemModel>();
			CartItemViewModel cartVM = new() 
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity*x.Price)
			};

			return View(cartVM);
		}
		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await dataContext.Products.FindAsync(Id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItems = cart.Where(c=>c.ProductId == Id).FirstOrDefault();
			if (cartItems == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItems.Quantity += 1;
			}
			HttpContext.Session.SetJson("Cart", cart);
			TempData["success"] = "Add Item to cart Successfuly";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity > 1)
			{
				cartItem.Quantity -= 1;
			}
			else
			{
				cart.RemoveAll(c => c.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Decrease Item quantity to cart Successfuly";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity >= 1)
			{
				++cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(c => c.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Increase Item quantity to cart Successfuly";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			cart.RemoveAll(p => p.ProductId == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart",cart);
			}
			TempData["success"] = "Remove Item quantity to cart Successfuly";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Clear Item quantity to cart Successfuly";
			return RedirectToAction("Index");
		}
	}
}
