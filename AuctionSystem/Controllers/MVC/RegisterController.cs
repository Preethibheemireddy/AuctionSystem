using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionModel;
using Auction.Database;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;

namespace AuctionSystemWebApi.Controllers.MVC
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo != null)
            {
                return RedirectToActionPermanent("Index", "Product");
            }
            PaymentMethodTypes paymentTypes = new PaymentMethodTypes();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                //HTTP GET
                var responseMessageTask = client.GetAsync("api/Register");
                responseMessageTask.Wait();

                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<PaymentMethodTypes>();
                    responseContentTask.Wait();
                    paymentTypes = responseContentTask.Result;
                    TempData["PaymentMethodOptions"] = paymentTypes.PaymentMethodOptions;
                    TempData.Keep();
                    return View();
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View();
                }
            }
        }

        [Route("Register/Create")]
        public ActionResult Create(RegisterRequest register)
        {
            RegisterResponse registerResponse = new RegisterResponse();
            if (!ModelState.IsValid)
            {
                TempData.Keep();
                return View("Index");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);

                //HTTP POST
                var responseMessageTask = client.PostAsJsonAsync<RegisterRequest>("api/Register", register);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<RegisterResponse>();
                    responseContentTask.Wait();
                    registerResponse = responseContentTask.Result;
                    if (registerResponse.Fault == null)
                    {
                        return RedirectToActionPermanent("Index", "Login");
                    }
                    ModelState.AddModelError("", registerResponse.Fault.Message);
                    TempData.Keep();
                    return View("Index");
                }
                ModelState.AddModelError("", "server error");
                TempData.Keep();
                return View("Index");
            }
        }
    }
}