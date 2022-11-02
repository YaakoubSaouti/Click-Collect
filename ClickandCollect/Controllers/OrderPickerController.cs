using ClickandCollect.DAL;
using ClickandCollect.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ClickandCollect.Controllers
{
    public class OrderPickerController : Controller
    {
        private readonly IMarketsDAL _marketsDAL;
        private readonly IOrdersDAL _ordersDAL;
        private readonly IChoicesDAL _choicesDAL;

        public OrderPickerController(IMarketsDAL marketsDAL, IOrdersDAL ordersDAL, IChoicesDAL choicesDAL)
        {
            _marketsDAL = marketsDAL;
            _ordersDAL = ordersDAL;
            _choicesDAL = choicesDAL;
        }
        public IActionResult SeeOrdersToMake()
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    && string.IsNullOrEmpty((HttpContext.Session.GetString("id_market")))
                    || HttpContext.Session.GetString("type") != "3"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            
            ViewData["no_order"] = TempData["no_order"];
            ViewData["error_saving"] = TempData["error_saving"];
            ViewData["success_prepare"] = TempData["success_prepare"];
            ViewData["error_access"] = TempData["error_access"];
            TempData.Clear();
            HttpContext.Session.SetOrder("Order", null);
            Market m = Market.GetMarket(Convert.ToInt32(HttpContext.Session.GetString("id_market")), _marketsDAL);
            m.GetOrdersToMake(_ordersDAL);
            return View(m.Orders);
        }

        public IActionResult MarkOrderAsPrepared(string id)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    && string.IsNullOrEmpty((HttpContext.Session.GetString("id_market")))
                    || HttpContext.Session.GetString("type") != "3"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty(id))
            {
                TempData["error_access"] = "Error when accessing that page";
                return RedirectToAction("SeeOrdersToMake", "OrderPicker");
            }
            ViewData["error_boxes"] = TempData["error_boxes"];
            Order order = HttpContext.Session.GetOrder("Order");
            if (order == default)
            {
                order = Order.GetOrder(Convert.ToInt32(id), _ordersDAL);
                order.GetChoices(_choicesDAL);
                HttpContext.Session.SetOrder("Order", order);
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkOrderAsPrepared(int nbr_boxes)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    && string.IsNullOrEmpty((HttpContext.Session.GetString("id_market")))
                    || HttpContext.Session.GetString("type") != "3"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order order = HttpContext.Session.GetOrder("Order");
            if (order == default)
            {
                TempData["no_order"] = "An error occured when adding boxes to the order";
                return RedirectToAction("SeeOrdersToMake", "OrderPicker");
            }
            if (nbr_boxes < 1)
            {
                ViewData["error_boxes"] = "The number of boxes has to be at least 1!";
                return View(HttpContext.Session.GetOrder("Order"));
            }
            order.NumberOfBoxes = nbr_boxes;
            order.State = 2;
            if (order.SaveOrder(_ordersDAL))
            {
                HttpContext.Session.SetOrder("Order", null);
                TempData["success_prepare"] = $"The order number {order.Id} has been prepared";
            }else TempData["error_saving"] = "An error occured when preparing the order";
            return RedirectToAction("SeeOrdersToMake", "OrderPicker");
        }
    }
}
