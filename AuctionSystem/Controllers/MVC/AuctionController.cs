﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Auction.Model.Message;
using System.Net.Http;
using Auction.Database;
using Auction.Model.Data;
using AuctionModel;
using System.Configuration;

namespace AuctionSystemWebApi.Controllers.MVC
{
    public class AuctionController : Controller
    {
        #region PostedBid
        //To post user bid
        [Route("Auction/UserBid")]
        public ActionResult UserBid(AuctionRequest auction)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            AuctionResponse auctionResponse = new AuctionResponse();
            ViewBag.LoginSuccess = "True";
            auction.Customer_Id = customerinfo.CustomerId;
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Bid Price is required";
                return RedirectToAction("GetProductWithProductid", "Product", new { productid = auction.ProductID });
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                //HTTP POST
                var responseMessageTask = client.PostAsJsonAsync<AuctionRequest>("api/Auction/CustomerBid", auction);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<AuctionResponse>();
                    responseContentTask.Wait();
                    auctionResponse = responseContentTask.Result;
                    if (auctionResponse.Error == null)
                    {
                        return RedirectToActionPermanent("GetMyBids", "Auction");
                    }
                    TempData["Error"] = auctionResponse.Error.Message;
                    return RedirectToAction("GetProductWithProductid", "Product", new { productid = auction.ProductID });
                }
                else
                {
                    TempData["Error"] = "Server error. Please contact administrator.";
                    return RedirectToAction("GetProductWithProductid", "Product", new { productid = auction.ProductID });
                }
            }
        }
        #endregion PostedBid

        #region GetMyBids
        //To display customer bids with customer id
        [Route("Auction/GetMyBids")]
        public ActionResult GetMyBids()
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            ViewBag.LoginSuccess = "True";
            AuctionResponse auctionResponse = new AuctionResponse();
            using (var client = new HttpClient())
            {
                int id = customerinfo.CustomerId;
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                var url = "api/Auction/GetBidsByCustomerId?customerId=" + id;
                //HTTP GET
                var responseMessageTask = client.GetAsync(url);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<AuctionResponse>();
                    responseContentTask.Wait();
                    auctionResponse = responseContentTask.Result;
                    TempData["Bid"] = auctionResponse.Auctions;
                    return View("MyBids", auctionResponse);
                }
                else //web api sent error response 
                {
                    //log response status here..
                    return View("MyBids", auctionResponse);
                }
            }
        }
        #endregion GetMyBids

        #region BidDetail
        //To display customer selected bid
        [Route("Auction/MyBidDetail")]
        public ActionResult MyBidDetail(int auctionid)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");
            UserAuction userAuction = new UserAuction();
            ViewBag.LoginSuccess = "True";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                //HTTP GET
                var responseMessageTask = client.GetAsync("api/Auction/GetBid?auctionid=" + auctionid);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<UserAuction>();
                    responseContentTask.Wait();
                    userAuction = responseContentTask.Result;
                    TempData["Bid"] = userAuction;

                    if (TempData["Error"] != null)
                    {

                        ModelState.AddModelError("", TempData["Error"].ToString() );
                        TempData["Error"] = null;
                    }
                    return View("CustomerBid",userAuction);
                }
                else //web api sent error response 
                {
                    //log response status here..
                    return View("MyBids", userAuction);
                }
            }
            
        }
        #endregion BidDetail

        #region UpdateCustomerBid
        //To update customer bid
        [Route("Auction/UpdateCustomerBid")]
        public ActionResult UpdateCustomerBid(UserAuction auction)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            AuctionResponse auctionResponse = new AuctionResponse();
            ViewBag.LoginSuccess = "True";
            auction.Customer_Id = customerinfo.CustomerId;
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Bid Price is required";
                return RedirectToAction("MyBidDetail", new { auctionid = auction.Auction_Id});
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                //HTTP POST
                var responseMessageTask = client.PostAsJsonAsync<UserAuction>("api/Auction/UpdateBid", auction);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<AuctionResponse>();
                    responseContentTask.Wait();
                    auctionResponse = responseContentTask.Result;
                    if (auctionResponse.Error == null)
                    {
                        return RedirectToActionPermanent("GetMyBids", "Auction");
                    }
                    TempData["Error"] = auctionResponse.Error.Message;
                    return RedirectToAction("MyBidDetail", new { auctionid = auction.Auction_Id });
                }
                else
                {
                    TempData["Error"] = "Server error";
                    return RedirectToAction("MyBidDetail", new { auctionid = auction.Auction_Id });
                }
            }
        }
        #endregion UpdateCustomerBid

        //To get current status of user selected bid
        [Route("Auction/GetBid")]
        public ActionResult GetBid(int auctionid)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            ViewBag.LoginSuccess = "True";
            AuctionResponse auctionResponse = new AuctionResponse();
            AuctionDetails auction = new AuctionDetails();
            using (var client = new HttpClient())
            {
                int customerid = customerinfo.CustomerId;
                auction.customerid = customerid;
                auction.auctionid = auctionid;
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                var responseMessageTask = client.PostAsJsonAsync<AuctionDetails>("api/GetUserBids", auction);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<AuctionResponse>();
                    responseContentTask.Wait();
                    auctionResponse = responseContentTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("MyBids", auctionResponse);
                }
            }
            return View("MyBids", auctionResponse);
        }
    }
}
