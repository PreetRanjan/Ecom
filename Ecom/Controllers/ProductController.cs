using Ecom.Data;
using Ecom.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Ecom.Controllers
{
    public class ProductController : Controller
    {
        private EcomContext _db;
        public ProductController()
        {
            _db = new EcomContext();
        }
        // GET: Product
        public ActionResult Index()
        {
            var products = _db.Products.ToList();
            return View(products);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Price = model.Price,
                    ProductName = model.Name
                };
                if (model != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.Image.InputStream.CopyToAsync(ms);
                        product.ProductImage = ms.ToArray();
                    }
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return new HttpStatusCodeResult(400);
            }
            return View(product);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> AddToCart(int id)
        {
            var product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return new HttpStatusCodeResult(400);
            }
            var userId = _db.AppUsers.SingleOrDefault(x => x.Email == User.Identity.Name).UserId;
            var cartItem = _db.ShoppingCarts.SingleOrDefault(x => x.ProductId == product.Id && x.UserId == userId);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                await _db.SaveChangesAsync();
                TempData["Success"] = true;
                return RedirectToAction("Index","Home");
            }
            else
            {
                var shoopingCartItem = new ShoppingCart()
                {
                    ProductId = product.Id,
                    UserId = userId,
                    Quantity = 1
                };
                _db.ShoppingCarts.Add(shoopingCartItem);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }

        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> RemoveItem(int id)
        {
            //var product = _db.Products.SingleOrDefault(x => x.Id == id);
            //if (product == null)
            //{
            //    return new HttpStatusCodeResult(400);
            //}
            //var userId = _db.AppUsers.SingleOrDefault(x => x.Email == User.Identity.Name).UserId;
            //var cartItem = _db.ShoppingCarts.SingleOrDefault(x => x.ProductId == product.Id && x.UserId == userId);
            var cartItem = await _db.ShoppingCarts.SingleOrDefaultAsync(x => x.Id == id);
            _db.ShoppingCarts.Remove(cartItem);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(MyCart));

        }
        public async Task<ActionResult> IncCount(int id)
        {
            var product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return new HttpStatusCodeResult(400);
            }
            var userId = _db.AppUsers.SingleOrDefault(x => x.Email == User.Identity.Name).UserId;
            var cartItem = _db.ShoppingCarts.SingleOrDefault(x => x.ProductId == product.Id && x.UserId == userId);
            //var cartItem = _db.ShoppingCarts.SingleOrDefault(x => x.Id ==id);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(MyCart));
            }
            return RedirectToAction(nameof(MyCart));
        }
        public async Task<ActionResult> DecCount(int id)
        {
            var product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return new HttpStatusCodeResult(400);
            }
            var ID = id;
            var userId = _db.AppUsers.SingleOrDefault(x => x.Email == User.Identity.Name).UserId;
            var cartItem = _db.ShoppingCarts.SingleOrDefault(x => x.ProductId == product.Id && x.UserId == userId);
            if (cartItem.Quantity>= 2)
            {
                cartItem.Quantity -= 1;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(MyCart));
            }
            else
            {
                _db.ShoppingCarts.Remove(cartItem);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(MyCart));
            }
            
        }
        [Authorize]
        public ActionResult MyCart()
        {
            var userId = _db.AppUsers.SingleOrDefault(x => x.Email == User.Identity.Name).UserId;
            var items = _db.ShoppingCarts.Include(x => x.Product).Where(x => x.UserId == userId).ToList();
            var model = new CartDetailModel(items);
            return View(model);
        }
    }
}