using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Auction.Model.Message;
using Auction.BusinessLogic;
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
            var response = CustomerAuction.GetCustomerBids(customerId);
            return Ok(response);
        }

        //To get user selected bid
        [Route("api/Auction/GetBid")]
        public IHttpActionResult GetBid(int auctionid)
        {
            var response = CustomerAuction.GetBid(auctionid);
            return Ok(response);
        }

        //To post customer bid
        [Route("api/Auction/CreateBid")]
        public IHttpActionResult CreateBid(AuctionRequest auction)
        {
            try
            {
                AuctionResponse auctionResponse = new AuctionResponse();
                if (!ModelState.IsValid)
                {
                    auctionResponse.Error = new Error() { Code = ErrorCodes.ModelStateInvalid, Message = "model state is not valid" };
                    return Ok(auctionResponse);
                }
                auctionResponse = CustomerAuction.CreateBid(auction);
                return Ok(auctionResponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //To update customer bid
        [Route("api/Auction/UpdateBid")]
        public IHttpActionResult UpdateBid(Auction.Model.Message.Auction auction)
        {
            try
            {
                AuctionResponse auctionResponse = new AuctionResponse();
                if (!ModelState.IsValid)
                {
                    auctionResponse.Error = new Error() { Code = ErrorCodes.ModelStateInvalid, Message = "model state is not valid" };
                    return Ok(auctionResponse);
                }
                auctionResponse = CustomerAuction.UpdateCustomerBid(auction);
                return Ok(auctionResponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
