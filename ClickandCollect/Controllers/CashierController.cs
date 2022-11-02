using ClickandCollect.DAL;
using ClickandCollect.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ClickandCollect.Controllers
{
    public class CashierController : Controller
    {
        private readonly IMarketsDAL _marketsDAL;
        private readonly IOrdersDAL _ordersDAL;
        private readonly IChoicesDAL _choicesDAL;

        public CashierController(IMarketsDAL marketsDAL, IOrdersDAL ordersDAL,IChoicesDAL choicesDAL)
        {
            _marketsDAL = marketsDAL;
            _ordersDAL = ordersDAL;
            _choicesDAL = choicesDAL;
        }
        public IActionResult SeeClientsOfTheDay()
        {
            if (
                    HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type"))
                    && string.IsNullOrEmpty((HttpContext.Session.GetString("id_market")))
                    || HttpContext.Session.GetString("type") != "2"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            ViewData["error_saving"] = TempData["error_saving"];
            ViewData["no_order"] = TempData["no_order"];
            ViewData["success_finalize"]=TempData["success_finalize"];
            ViewData["error_access"] = TempData["error_access"];
            TempData.Clear();
            HttpContext.Session.SetOrder("Order", null);
            Market m=Market.GetMarket(Convert.ToInt32(HttpContext.Session.GetString("id_market")),_marketsDAL);
            m.GetAllOrdersOfTheDay(_ordersDAL);
            return View(m.Orders);
        }

        public IActionResult FinalizeOrder(string id)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    && string.IsNullOrEmpty((HttpContext.Session.GetString("id_market")))
                    || HttpContext.Session.GetString("type") != "2"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty(id))
            {
                TempData["error_access"] = "Error when accessing that page";
                return RedirectToAction("SeeClientsOfTheDay", "Cashier");
            }
            ViewData["error_boxes"] = TempData["error_boxes"];
            Order order = HttpContext.Session.GetOrder("Order");
            if (order == default) order = Order.GetOrder(Convert.ToInt32(id),_ordersDAL);
            order.GetChoices(_choicesDAL);
            HttpContext.Session.SetOrder("Order", order);
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizeOrder()
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    && string.IsNullOrEmpty((HttpContext.Session.GetString("id_market")))
                    || HttpContext.Session.GetString("type") != "2"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order order = HttpContext.Session.GetOrder("Order");
            if (order == default)
            {
                TempData["no_order"] = "An error occured when removing boxes from the order";
                return RedirectToAction("SeeClientsOfTheDay", "Cashier");
            }
            order.State = 3;
            if (order.SaveOrder(_ordersDAL))
            {
                HttpContext.Session.SetOrder("Order", null);
                TempData["success_prepare"] = $"The order number {order.Id} has been finalized";
            }
            else TempData["error_saving"] = "An error occured when preparing the order";
            return RedirectToAction("SeeClientsOfTheDay", "Cashier");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveReturnedBoxesFromOrder(int nbr_boxes)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    && string.IsNullOrEmpty((HttpContext.Session.GetString("id_market")))
                    || HttpContext.Session.GetString("type") != "2"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order order = HttpContext.Session.GetOrder("Order");
            if (order == default) {
                TempData["no_order"] = "An error occured when removing boxes from the order";
                return RedirectToAction("SeeClientsOfTheDay", "Cashier");
            }
            if (nbr_boxes > order.NumberOfBoxes || nbr_boxes <= 0)
            {
                TempData["error_boxes"] = "You can't remove this number of boxes, it's greater than the current number of boxes or is equal to 0 or lower!";
                return RedirectToAction("FinalizeOrder", "Cashier");
            }
            order.NumberOfBoxes -= nbr_boxes;
            HttpContext.Session.SetOrder("Order", order);
            return RedirectToAction("FinalizeOrder", "Cashier");
        }
    }
}
