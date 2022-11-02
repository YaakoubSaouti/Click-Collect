using ClickandCollect.DAL;
using ClickandCollect.Models;
using ClickandCollect.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ClickandCollect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IClientsDAL _clientsDAL;

        private readonly IPersonsDAL _personsDAL;

        public HomeController(ILogger<HomeController> logger, IClientsDAL clientsDAL, IPersonsDAL personsDAL)
        {
            _logger = logger;
            _clientsDAL = clientsDAL;
            _personsDAL = personsDAL;
        }

        public IActionResult Index()
        {

            HttpContext.Session.Clear();
            ViewBag.not_connected = TempData["not_connected"];
            return View();
        }
        public IActionResult LogIn()
        {
            HttpContext.Session.Clear();
            ViewBag.sor = TempData["success_of_registration"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(LoginViewModel p)
        {
            HttpContext.Session.Clear();
            Person pers = Person.LogIn(p.Username, p.Password, _personsDAL);
            if (pers != null)
            {
                if (HttpContext.Session.GetInt32("id") == null
                        && string.IsNullOrEmpty(HttpContext.Session.GetString("username"))
                        && string.IsNullOrEmpty(HttpContext.Session.GetString("lastname"))
                        && string.IsNullOrEmpty(HttpContext.Session.GetString("firstname"))
                        && string.IsNullOrEmpty(HttpContext.Session.GetString("type"))
                )
                {
                    HttpContext.Session.SetInt32("id", pers.Id);
                    HttpContext.Session.SetString("username", pers.Username);
                    HttpContext.Session.SetString("lastname", pers.Lastname);
                    HttpContext.Session.SetString("firstname", pers.Firstname);
                    if (pers is Client)
                    {
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                            HttpContext.Session.SetString("type", "1");
                        return RedirectToAction("SeeProducts", "Client");
                    }
                    if (pers is Cashier)
                    {
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                        {
                            HttpContext.Session.SetString("type", "2");
                            Cashier c = pers as Cashier;
                            if (c != null) HttpContext.Session.SetString("id_market", (c.Market.Id).ToString());
                        }
                        return RedirectToAction("SeeClientsOfTheDay", "Cashier");
                    }
                    if (pers is OrderPicker)
                    {
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString("type")))
                        {
                            HttpContext.Session.SetString("type", "3");
                            OrderPicker op = pers as OrderPicker;
                            if (op != null) HttpContext.Session.SetString("id_market", (op.Market.Id).ToString());
                        }
                        return RedirectToAction("SeeOrdersToMake", "OrderPicker");
                    }
                }
            }
            ViewData["error_db"] = "The account doesn't exist!";
            return View();
        }

        public IActionResult SignUp()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(SignUpViewModel c) 
        {
            HttpContext.Session.Clear();
            ViewData["error_db"] = "";
            TempData["success_of_registration"] = "";
            if (ModelState.IsValid) 
            {
                Client client = Client.SignUp(c.Firstname, c.Lastname, c.Username, c.Password, _clientsDAL);
                if (client==null)
                {
                    ViewData["error_db"] = "The username already exist!";
                    return View();
                }
                if(!client.CreateAccount(_clientsDAL))
                    ViewData["error_db"] = "The systeme had a problem creating the account!";
                TempData["success_of_registration"]="Your registration was successfull!";
                return RedirectToAction("LogIn");
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
