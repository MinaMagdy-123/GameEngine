using GameEngine.Services;
using GameEngine.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameEngine.Controllers
{
    [Authorize]
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAccountService _accountService;
        private readonly IOrdersAndItemsService _ordersAndItemsController;

        public ShoppingCartsController(IShoppingCartService shoppingCartService, IAccountService accountService, IOrdersAndItemsService ordersAndItemsController)
        {
            _shoppingCartService = shoppingCartService;
            _accountService = accountService;
            _ordersAndItemsController = ordersAndItemsController;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Accounts");

            var items = _shoppingCartService.GetCartItems(userId);
            var total = _shoppingCartService.GetCartTotal(userId);

            var viewModel = new ShoppingCartViewModel
            {
                Items = items,
                Total = total
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToCart(int gameId, int deviceId, int quantity)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Accounts");

            _shoppingCartService.AddToCart(userId, gameId, deviceId, quantity);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int cartId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Accounts");

            _shoppingCartService.RemoveFromCart(cartId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Accounts");

            _shoppingCartService.ClearCart(userId);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Accounts");

            var user = await _accountService.GetById(userId);
            if (user is null || (user.Id).ToString() != userId)
                return RedirectToAction("Login", "Accounts");


            var items = _shoppingCartService.GetCartItems(userId);
            var total = _shoppingCartService.GetCartTotal(userId);

            var viewModel = new OrderSummaryViewModel
            {
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                Address = user.Address ?? "",
                Items = items,
                Total = total,
                OrderDate = DateTime.Now
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateStripeSession()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Accounts");

            var cartItems = _shoppingCartService.GetCartItems(userId);

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = Url.Action("PaymentSuccess", "ShoppingCarts", null, Request.Scheme)!,
                CancelUrl = Url.Action("Index", "ShoppingCarts", null, Request.Scheme)!,
                LineItems = cartItems.Select(item => new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = item.Game.Price * 100, // in cents
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Game.Name
                        }
                    },
                    Quantity = item.Quantity
                }).ToList()
            };

            var service = new Stripe.Checkout.SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }


        public IActionResult PaymentSuccess()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Accounts");

            var items = _shoppingCartService.GetCartItems(userId);
            var totalPrice = _shoppingCartService.GetCartTotal(userId);

            _ordersAndItemsController.Create(items, totalPrice, userId);
            _shoppingCartService.ClearCart(userId);

            return View();
        }

    }
}
