using GameEngine.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameEngine.Controllers
{
    public class OrdersAndItemsController : Controller
    {

        private readonly IOrdersAndItemsService _ordersAndItemsController;

        public OrdersAndItemsController(IOrdersAndItemsService rdersAndItemsController)
        {
            _ordersAndItemsController = rdersAndItemsController;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orders = _ordersAndItemsController.GetAll();
            return View(orders);
        }

        [HttpGet]
        public IActionResult ItemsDetails(int orderId)
        {
            var items = _ordersAndItemsController.GetItemsByOrderId(orderId);
            return View(items);
        }

    }
}
