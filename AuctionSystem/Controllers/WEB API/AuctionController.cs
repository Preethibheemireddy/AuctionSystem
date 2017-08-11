using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Auction.Model.Message;
using Auction.BuisinessLogic;
using Auction.Model.Data;
using Auction.Model;

namespace AuctionSystemWebApi.Controllers
{
    public class AuctionController : ApiController
    {
        //To retreive user bids with customerid
        [Route("api/Auction/GetBidsByCustomerId")]
        public IHttpActionResult GetBidsByCustomerId(int customerId)
        {
            var response = CustomerAuction.GetUserBids(customerId);
            return Ok(response);
        }

        //To get user selected bid
        [Route("api/Auction/GetBid")]
        public IHttpActionResult GetBid( int auctionid)
        {
            var response = CustomerAuction.GetCustomerBid(auctionid);
            return Ok(response);
        }

        //To post customer bid
        [Route("api/Auction/CustomerBid")]
        public IHttpActionResult CustomerBid(AuctionRequest auction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    AuctionResponse Response = new AuctionResponse();
                    Response.Error = new Error() { Code = ErrorCodes.ModelStateInvalid , Message = "model state is not valid" };
                    return Ok(Response);
                }
                var response = CustomerAuction.UserBid(auction);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //To update customer bid
        [Route("api/Auction/UpdateBid")]
        public IHttpActionResult UpdateBid(UserAuction auction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    AuctionResponse Response = new AuctionResponse();
                    Response.Error = new Error() { Code = ErrorCodes.ModelStateInvalid, Message = "model state is not valid" };
                    return Ok(Response);
                }
                var response = CustomerAuction.UpdateCustomerBid(auction);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
