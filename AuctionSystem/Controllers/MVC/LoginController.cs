using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Auction.Database;
using System.Configuration;
using Auction.Model.Message;

namespace AuctionSystemWebApi.Controllers.MVC
{
    public class LoginController : Controller
    {
        // GET: Login
        [Route("Login/Index")]
        public ActionResult Index()
        {
            ViewBag.LoginSuccess = "False";
            return View();
        }

        [Route("Login/Signin")]
        [HttpPost]
        public ActionResult Signin(LoginModelRequest login)
        {
            LoginModelResponse loginModelResponse = new LoginModelResponse();
            ViewBag.LoginSuccess = "False";

            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);

                //HTTP POST
                var responseMessageTask = client.PostAsJsonAsync<LoginModelRequest>("api/Login", login);
                responseMessageTask.Wait();

                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<LoginModelResponse>();
                    responseContentTask.Wait();
                    loginModelResponse = responseContentTask.Result;
                    if (loginModelResponse.Error == null)
                    {
                        Session["Customer"] = loginModelResponse;
                        return RedirectToActionPermanent("ProductTypes", "Product");
                    }
                    ModelState.AddModelError("", loginModelResponse.Error.Message);
                    return View("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server error");
                    return View("Index");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session["Customer"] = null;
            return RedirectToActionPermanent("Index", "Login");
        }
    }
}
