using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Auction.Model.Message;
using Auction.BusinessLogic;
using Auction.Model;
using Auction.Model.Data;

namespace AuctionSystemWebApi.Controllers
{
    public class LoginController : ApiController
    {
        [Route("api/Login")]
        public IHttpActionResult UserLogin([FromBody]LoginModelRequest loginRequest)
        {
            LoginModelResponse loginResponse = new LoginModelResponse();
            try
            {
                if (!ModelState.IsValid)
                {
                    loginResponse.Error = new Error { Code = ErrorCodes.ModelStateInvalid, Message = "Please correct the errors" };
                    return Ok(loginResponse);
                }
                loginResponse = CustomerLogin.Login(loginRequest);
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
