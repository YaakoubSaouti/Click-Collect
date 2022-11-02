using ClickandCollect.DAL;
using ClickandCollect.Models;
using ClickandCollect.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ClickandCollect.Controllers
{
    public class ClientController : Controller
    {
        private readonly IProductsDAL _productsDAL;

        private readonly ICategorysDAL _categoriesDAL;

        private readonly IMarketsDAL _marketsDAL;

        private readonly ITimeslotsDAL _timeslotsDAL;

        private readonly IOrdersDAL _ordersDAL;

        private readonly IClientsDAL _clientsDAL;

        public ClientController(IProductsDAL productsDAL, ICategorysDAL categoriesDAL, IMarketsDAL marketsDAL,ITimeslotsDAL timeslotsDAL,IOrdersDAL ordersDAL, IClientsDAL clientsDAL)
        {
            _productsDAL = productsDAL;
            _categoriesDAL = categoriesDAL;
            _marketsDAL = marketsDAL;
            _timeslotsDAL = timeslotsDAL;
            _ordersDAL = ordersDAL;
            _clientsDAL = clientsDAL;
        }

        public IActionResult SeeProducts(string id)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type")!="1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.successAddProduct = TempData["successAddProduct"];
            ViewBag.errorAddProduct = TempData["errorAddProduct"];
            ViewBag.errorOrder = TempData["errorOrder"];
            ViewBag.success = TempData["success"];
            TempData.Clear();
            ProductsCategoriesViewModel pcs;
            if (!string.IsNullOrEmpty(id))
            {
                int idint = Convert.ToInt32(id);
                Category chosenCategory = Category.GetCategory(idint, _categoriesDAL);
                chosenCategory.GetProducts(_productsDAL);
                pcs = new ProductsCategoriesViewModel
                {
                    Categories = Category.GetCategories(_categoriesDAL),
                    Products = chosenCategory.Products,
                };
            }
            else
            {
                pcs = new ProductsCategoriesViewModel
                {
                    Categories = Category.GetCategories(_categoriesDAL),
                    Products = Product.GetProducts(_productsDAL),
                };
            }
            return View(pcs);
        }

        public IActionResult AddProduct(string id)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty(id))
            {
                TempData["errorAddProduct"]="Error when accessing that page";
                return RedirectToAction("SeeProducts", "Client");
            }
            Choice choice = new Choice
            {
                Quantity = 0,
                Product = Product.GetProduct(Convert.ToInt32(id), _productsDAL)
            };
            return View(choice);
        }

        [HttpPost]
        public IActionResult AddProduct(string id_product, Choice choice)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                TempData["success_addProduct"] = "Your choice has been added to cart ( you can see it by clicking on the cart button ) ";
                Choice c = new Choice(choice.Quantity, Product.GetProduct(Convert.ToInt32(id_product), _productsDAL));
                Order currentOrder = HttpContext.Session.GetOrder("Order");
                if (currentOrder == default) currentOrder = new Order(Client.GetClient((int)HttpContext.Session.GetInt32("id"),_clientsDAL));
                currentOrder.AddChoice(c);
                HttpContext.Session.SetOrder("Order", currentOrder);
                return RedirectToAction("SeeProducts", "Client");
            }
            Choice ch = new Choice
            {
                Quantity = 0,
                Product = Product.GetProduct(Convert.ToInt32(id_product), _productsDAL)
            };
            return View(ch);
        }
        public IActionResult SeeCart()
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            if (currentOrder == default || currentOrder.Choices.Count == 0) ViewData["error_session"] = "No Item in Cart";
            return View(currentOrder);
        }
        public IActionResult DeleteProduct(string id)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            if (currentOrder == default || currentOrder.Choices.Count == 0)
                return RedirectToAction("SeeProducts", "Client");
            currentOrder.RemoveAChoiceByProductId(Convert.ToInt32(id));
            HttpContext.Session.SetOrder("Order", currentOrder);
            return RedirectToAction("SeeCart", "Client");
        }

        public IActionResult ChooseMarket()
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            if (currentOrder == default || currentOrder.Choices.Count == 0)
            {
                TempData["errorOrder"]="You can not access this page !";
                return RedirectToAction("SeeProducts", "Client");
            }
            List<Market> marketList = Market.GetMarkets(_marketsDAL);
            MarketSelectViewModel marketSelectViewModel = new MarketSelectViewModel();
            marketSelectViewModel.FillTheSelect(marketList);
            return View(marketSelectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChooseMarket(MarketSelectViewModel m)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            currentOrder.Market = Market.GetMarket(Convert.ToInt32(m.MarketSelect), _marketsDAL);
            HttpContext.Session.SetOrder("Order", currentOrder);
            TempData["success"] = "You succesfully choosed a market!";
            return RedirectToAction("ChooseDate", "Client");
        }

        public IActionResult ChooseDate()
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            if (currentOrder == default || currentOrder.Choices.Count == 0 || currentOrder.Market == null) 
            {
                TempData["errorOrder"] = "You can not access this page !";
                return RedirectToAction("SeeProducts", "Client");
            }
            ViewBag.success = TempData["success"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChooseDate(DateViewModel d)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.dateisnotvalid = "";
            DateTime todaysdate = DateTime.Now;
            string todaysdatestring = todaysdate.ToString("yyyy-MM-dd");
            int comparison = String.Compare(d.Date, todaysdatestring, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (comparison <= 0) 
            {
                ViewBag.dateisnotvalid = "You can not order for today or before!";
                return View();
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            currentOrder.Date = d.Date;
            HttpContext.Session.SetOrder("Order", currentOrder);
            TempData["success"] = "You succesfully choosed a date!";
            return RedirectToAction("ChooseTimeslot", "Client");
        }

        public IActionResult ChooseTimeslot() 
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            if (currentOrder == default || currentOrder.Choices.Count == 0 || currentOrder.Market == null || currentOrder.Date == null) 
            {
                TempData["errorOrder"] = "You can not access this page !";
                return RedirectToAction("SeeProducts", "Client");

            }
            currentOrder.Market.GetTimeslots(currentOrder.Date,_timeslotsDAL);
            TimeslotSelectViewModel s = new TimeslotSelectViewModel();
            s.FillTheSelect(currentOrder.Market.Timeslots);
            HttpContext.Session.SetOrder("Order", currentOrder);
            ViewBag.success = TempData["success"];

            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChooseTimeslot(TimeslotSelectViewModel t)
        {
            if (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            currentOrder.Timeslot = Timeslot.GetTimeslot(Convert.ToInt32(t.TimeslotSelect), _timeslotsDAL);
            HttpContext.Session.SetOrder("Order", currentOrder);
            TempData["success"] = "You succesfully choosed a Timeslot! You can now send your order!";
            return RedirectToAction("SendOrder", "Client");
        }

        public IActionResult SendOrder()
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            Order currentOrder = HttpContext.Session.GetOrder("Order");
            if (currentOrder == default || currentOrder.Choices.Count == 0 || currentOrder.Market == null || currentOrder.Date == null || currentOrder.Timeslot == null)
            {
                TempData["errorOrder"] = "You can not access this page !";
                return RedirectToAction("SeeProducts", "Client");
            }
            ViewBag.success = TempData["success"];
            return View(currentOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendOrder(Order currentOrder)
        {
            if (
                    (HttpContext.Session.GetInt32("id") == null
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                    && string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                    || HttpContext.Session.GetString("type") != "1"
            )
            {
                TempData["not_connected"] = "Your are not/no longer in session!";
                return RedirectToAction("Index", "Home");
            }
            currentOrder = HttpContext.Session.GetOrder("Order");
            if (currentOrder.CreateOrder(_ordersDAL))
            {
                TempData["success"] = "The Order was successfully sent!";
                HttpContext.Session.SetOrder("Order", null);
                return RedirectToAction("SeeProducts", "Client");
            }
            HttpContext.Session.SetOrder("Order", null);
            ViewBag.errorOrder = "An error occured when sending the order!";
            return View();
        }


    }
}
