using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Auction.Model.Message;
using System.Net.Http;
using Auction.Database;
using Auction.Model.Data;
using System.Configuration;

namespace AuctionSystemWebApi.Controllers.MVC
{
    public class ProductController : Controller
    {
        #region GetProductTypes
        //  To get product types
        [Route("ProductTypes")]
        public ActionResult ProductTypes()
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            ViewBag.LoginSuccess = "True";
            ProductTypesResponse productTypesResponse = new ProductTypesResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                //HTTP GET
                var responseMessageTask = client.GetAsync("api/Product/GetProductTypes");
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<ProductTypesResponse>();
                    responseContentTask.Wait();
                    productTypesResponse = responseContentTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("Index",productTypesResponse);
                }
            }
            return View("Index", productTypesResponse);
        }
        #endregion GetProductTypes

        #region GetMyAuctionProducts
        //To get customer products with customerid
        [Route("Product/GetCustomerProducts")]
        public ActionResult GetCustomerProducts()
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            int id = customerinfo.CustomerId;
            ViewBag.LoginSuccess = "True";
            ProductResponse productResponse = new ProductResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);

                //HTTP GET
                var responseMessageTask = client.GetAsync("api/Products?customerid=" + id);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<ProductResponse>();
                    responseContentTask.Wait();
                    productResponse = responseContentTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("MyAuctionsView", productResponse);
                }
            }
            return View("MyAuctionsView", productResponse);
        }
        #endregion GetMyAuctionProducts

        #region GetMyAuctionProductDetail
        //To display user selected product
        [Route("Product/DisplayCustomerProduct")]
        public ActionResult DisplayCustomerProduct(int productid)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            Product productModel = new Product();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);

                //HTTP GET
                var responseMessageTask = client.GetAsync("api/Product/GetProduct?productId=" + productid);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<Product>();
                    responseContentTask.Wait();
                    productModel = responseContentTask.Result;
                    // TempData["products"] = productResponse.Products;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("MyAuctionsView", productModel);
                }
            }

            ViewBag.LoginSuccess = "True";

            if (TempData["Error"] != null)
            {
                ModelState.AddModelError("", TempData["Error"].ToString());
                TempData["Error"] = null;
            }
            return View("CustomerProduct", productModel);
        }
        #endregion GetMyAuctionProductDetails

        #region UpdateMyAuctionProduct
        //To update customer product
        [Route("Product/UpdateCustomerProduct")]
        public ActionResult UpdateCustomerProduct(ProductRequest product)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            ProductResponse productResponse = new ProductResponse();
            ViewBag.LoginSuccess = "True";
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "All Fields are required";
                return RedirectToAction("DisplayCustomerProduct", new { productid = product.ProductID });
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                product.CustomerId = customerinfo.CustomerId;

                //HTTP POST
                var responseMessageTask = client.PostAsJsonAsync<ProductRequest>("api/Product/UpdateProduct", product);
                responseMessageTask.Wait();

                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<ProductResponse>();
                    responseContentTask.Wait();
                    productResponse = responseContentTask.Result;
                    if (productResponse.Error == null)
                    {
                        return RedirectToActionPermanent("GetCustomerProducts", "Product");
                    }
                    TempData["Error"] = productResponse.Error.Message;
                    return RedirectToAction("DisplayCustomerProduct", new { productid = product.ProductID });

                }
                TempData["Error"] = "server error";
                return RedirectToAction("DisplayCustomerProduct", new { productid = product.ProductID });
            }
        }
        #endregion UpdateMyAuctionProduct

        #region GetAuctionProductsByProductTypeId
        //To get products with product type id
        [Route("Product/GetProducts")]
        public ActionResult GetProducts(int productTypeId)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            ViewBag.LoginSuccess = "True";
            ProductResponse productResponse = new ProductResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                var url = "api/Product/GetProductByProductTypeId?productTypeId=" + productTypeId;
                //HTTP GET
                var responseMessageTask = client.GetAsync(url);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<ProductResponse>();
                    responseContentTask.Wait();
                    productResponse = responseContentTask.Result;
                    TempData["selectedProducts"] = productResponse.Products;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("DisplayProducts", productResponse);
                }
            }
            return View("DisplayProducts", productResponse);
        }
        #endregion GetAuctionProductsByProductTypeId

        #region GetAuctionProductDetailUsingProductId
        //To  get user selected product with productid
        [Route("Product/GetProductWithProductid")]
        public ActionResult GetProductWithProductid(int productid)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            Product productResponse = new Product();
            ViewData["BidPrice"] = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);

                //HTTP GET
                var responseMessageTask = client.GetAsync("api/Product/GetProduct?productId=" + productid);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<Product>();
                    responseContentTask.Wait();
                    productResponse = responseContentTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("MyProduct", productResponse);
                }
            }

            ViewBag.LoginSuccess = "True";
            if (TempData["Error"] != null)
            {
                ModelState.AddModelError("", TempData["Error"].ToString());
                TempData["Error"] = null;
            }
            return View("MyProduct", productResponse);
        }
        #endregion GetAuctionProductDetailUsingProductId

        #region AddProductForAuction
        //To display sell Product view
        [Route("Product/SellProduct")]
        public ActionResult SellProduct()
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            ViewBag.LoginSuccess = "True";
            ProductTypesResponse productTypesResponse = new ProductTypesResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                //HTTP GET
                var responseMessageTask = client.GetAsync("api/Product/GetProductTypes");
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<ProductTypesResponse>();
                    responseContentTask.Wait();
                    productTypesResponse = responseContentTask.Result;
                    TempData["products"] = productTypesResponse.ProductTypes;
                    TempData.Keep();
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View();
                }
            }
            return View();
        }

        //To post the user product
        [Route("Product/AddProduct")]
        public ActionResult AddProduct(ProductRequest product)
        {
            LoginModelResponse customerinfo = (LoginModelResponse)Session["Customer"];
            if (customerinfo == null)
                return RedirectToActionPermanent("Index", "Login");

            ProductResponse productResponse = new ProductResponse();
            ViewBag.LoginSuccess = "True";
            product.CustomerId = customerinfo.CustomerId;
            if (!ModelState.IsValid)
            {
                TempData.Keep();
                return View("SellProduct");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiBaseUrl"]);
                //HTTP POST
                var responseMessageTask = client.PostAsJsonAsync<ProductRequest>("api/CreateProduct", product);
                responseMessageTask.Wait();
                var responseMessage = responseMessageTask.Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContentTask = responseMessage.Content.ReadAsAsync<ProductResponse>();
                    responseContentTask.Wait();
                    productResponse = responseContentTask.Result;
                    if (productResponse.Error == null)
                    {
                        return RedirectToActionPermanent("GetCustomerProducts", "Product");
                    }
                    ModelState.AddModelError("", productResponse.Error.Message);
                    TempData.Keep();
                    return View("SellProduct");
                }
                ModelState.AddModelError("", "server error");
                TempData.Keep();
                return View("SellProduct");
            }
        }
        #endregion AddProductForAuction
    }
}
