using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Auction.Database;
using System.Net.Http;
using Auction.Model.Message;
using AuctionModel;
using System.Configuration;

namespace AuctionSystemWebApi.Controllers.MVC
{
    public class OrderController : Controller
    {
        //To get customer orders
        public ActionResult GetCustomerOrders()
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            OrdersResponse orders = new OrdersResponse();
            if (customerinfo != null)
            {
                ViewBag.LoginSuccess = "True";
                int customerid = customerinfo.CustomerId;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:54713/api/");
                    //HTTP GET
                    var url = "Orders/GetUserOrders?customerid=" + customerid;
                    var responseMessageTask = client.GetAsync(url);
                    responseMessageTask.Wait();

                    var result = responseMessageTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<OrdersResponse>();
                        readTask.Wait();
                        orders = readTask.Result;
                        return View(orders);

                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return View(orders);
                    }
                }
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        //To display customer selected order
        [Route("Order/DisplayCustomerOrder")]
        public ActionResult DisplayCustomerOrder(int orderid)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
           
            if (customerinfo == null)
            {
                return RedirectToActionPermanent("Index", "Login");
            }
                ViewBag.LoginSuccess = "True";
            Orders customerOrders = new Orders();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                //HTTP GET
                var responseMessageTask = client.GetAsync("api/Orders/GetOrder?orderid=" + orderid);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<Orders>();
                    responseContentTask.Wait();
                    customerOrders = responseContentTask.Result;
                    return View("DisplayCustomerOrder", customerOrders); 
                }
                else //web api sent error response 
                {
                    return View("DisplayCustomerOrder", customerOrders);
                }
            }

        }
    }
}
