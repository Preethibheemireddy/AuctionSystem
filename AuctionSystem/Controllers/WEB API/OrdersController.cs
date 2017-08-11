using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Auction.BuisinessLogic;
using Auction.Model.Message;

namespace AuctionSystemWebApi.Controllers
{
    public class OrdersController : ApiController
    {
        [Route("api/Orders/GetUserOrders")]
        public IHttpActionResult GetUserOrders(int customerid)
        {
            var response = CustomerOrders.GetUserOrders(customerid);
            return Ok(response);
        }

        [Route("api/Orders/GetOrder")]
        public IHttpActionResult GetOrder(int orderid)
        {
            var response = CustomerOrders.GetOrder(orderid);
            return Ok(response);
        }
        
    }
}
