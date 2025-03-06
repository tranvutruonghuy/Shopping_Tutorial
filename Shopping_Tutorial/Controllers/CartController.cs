using Microsoft.AspNetCore.Mvc;
using Shopping_Tutorial.Models;
using Shopping_Tutorial.Models.ViewModels;
using Shopping_Tutorial.Repository;

namespace Shopping_Tutorial.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            //Get Json trong sessin và trả về list CartItemModel
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            //Lấy từ sesson về chỉ để hiện thị
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price),
            };
            return View(cartVM);
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }

        public async Task<IActionResult> Add(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItem == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);

            TempData["success"] = "Add item to cart successfully!";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Decrease(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["success"] = "Decrease item quantity to cart successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Increase(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            cartItem.Quantity++;
            HttpContext.Session.SetJson("Cart", cart);
            TempData["success"] = "Increase item quantity to cart successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            cart.RemoveAll(p => p.ProductId == Id);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["success"] = "Remove item in cart successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Clear(int Id)
        {
            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Clear all item in cart successfully!";
            return RedirectToAction("Index");
        }
    }
}