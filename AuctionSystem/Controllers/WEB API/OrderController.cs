using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Auction.BusinessLogic;
using Auction.Model.Message;

namespace AuctionSystemWebApi.Controllers
{
    public class OrderController : ApiController
    {
        [Route("api/Order/GetCustomerOrders")]
        public IHttpActionResult GetCustomerOrders(int customerid)
        {
            var response = CustomerOrders.GetCustomerOrders(customerid);
            return Ok(response);
        }

        [Route("api/Order/GetOrder")]
        public IHttpActionResult GetOrder(int orderid)
        {
            var response = CustomerOrders.GetOrder(orderid);
            return Ok(response);
        }
    }
}
