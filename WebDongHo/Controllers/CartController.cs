using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebDongHo.Models;
using WebDongHo.ViewModels;
namespace WebDongHo.Controllers
{

    public class CartController : Controller
    {
        private readonly WebsiteBanAoContext _context;
        private const string CartSession = "CartSession";
        public CartController(WebsiteBanAoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var cart = HttpContext.Session.GetString(CartSession);
            var list = new List<CartItem>();
            if (!string.IsNullOrEmpty(cart))
            {
                list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
            }
            var cartViewModel = new CartViewModel
            {
                Menus = menus,
                Blogs = blogs,
                CartItems = list
            };
            return View(cartViewModel);
        }
        public IActionResult AddItem(int ProductId, int Quantity)
        {
            var product = _context.Products.Find(ProductId);
            var cart = HttpContext.Session.GetString(CartSession);
            if (!string.IsNullOrEmpty(cart))
            {
                var list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                if (list.Exists(x => x.Product.IdPro == ProductId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.IdPro == ProductId)
                        {
                            item.Quantity += Quantity;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = Quantity;
                    list.Add(item);
                }
                HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(list));
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = Quantity;
                var list = new List<CartItem>();
                list.Add(item);
                HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(list));
            }
            return RedirectToAction("Index");
        }
       
        public IActionResult Update(string cartModel)
        {
            var jsonCart = JsonConvert.DeserializeObject<List<CartItem>>(cartModel);
            var sessionCart =
            JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(CartSession));

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.IdPro == item.Product.IdPro);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }

            HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(sessionCart));
            return Json(new
            {
                status = true
            });
        }
        public IActionResult Delete(int id)
        {
            try
            {
                var sessionCart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(CartSession));
                sessionCart.RemoveAll(x => x.Product.IdPro == id);
                HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(sessionCart));

                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Delete: " + ex.Message);
                return Json(new { status = false });
            }
        }

        public IActionResult DeleteAll()
        {
            HttpContext.Session.Remove(CartSession);
            return Json(new
            {
                status = true
            });
        }

       

     



    }




}
