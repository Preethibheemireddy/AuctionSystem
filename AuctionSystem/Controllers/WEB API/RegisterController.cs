using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Auction.Database;
using Auction.Model.Message;
using Auction.Model;
using Auction.BusinessLogic;

namespace AuctionSystemWebApi.Controllers
{
    public class RegisterController : ApiController
    {
        // GET: api/Register/5
        [Route("api/Register")]
        [HttpGet]
        public IHttpActionResult Register()
        {
            PaymentMethodTypes paymentMethodTypesResponse = new PaymentMethodTypes();
            var paymentMethods = CustomerRegistration.GetPaymentMethodOptions();
            if (paymentMethods.Count == 0)
                paymentMethodTypesResponse.Fault = new Auction.Model.Data.Error { Code = ErrorCodes.NoPaymentOptions, Message = "There are no payment options" };
            else
                paymentMethodTypesResponse.PaymentMethodOptions = paymentMethods;
            return Ok(paymentMethodTypesResponse);
        }

        // POST: api/Register
        [Route("api/Register")]
        [HttpPost]
        public IHttpActionResult Register([FromBody]RegisterRequest register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(CustomerRegistration.RegisterCustomer(register));
                }
                else
                {
                    return Ok(CustomerRegistration.GetPaymentMethodOptions());
                }
            }
            catch (Exception ex)
            {
                return Ok("unable to complete registration");
            }
        }
    }
}
