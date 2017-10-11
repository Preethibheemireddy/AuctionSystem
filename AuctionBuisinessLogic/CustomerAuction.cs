using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Model.Message;
using Auction.Database;
using Auction.Model.Data;
using Auction.Model;

namespace Auction.BusinessLogic
{
    public static class CustomerAuction
    {
        private static AuctionSystemEntities auctionEntities;
        public static string format = "MMM d yyyy";

        #region Get Customer Bids
        //To get user bids with customer id
        public static AuctionResponse GetCustomerBids(int customerId)
        {
            auctionEntities = new AuctionSystemEntities();
            AuctionResponse auctionResponse = new AuctionResponse();
            var auctions = auctionEntities.auctions.Where(c => c.customer_id == customerId).OrderByDescending(d => d.auction_datetime).ToList();

            if (auctions == null)
            {
                auctionResponse.Error = new Error { Code = ErrorCodes.NoAuctions, Message = "This customer does not contain any auctions" };
                return auctionResponse;
            }

            //if auction is not null retrieve current highest bid price from Auction table with productid and max bid price 
            auctionResponse.Auctions = new List<Model.Message.Auction>();

            foreach (auction auction in auctions)
            {
                var userAuction = new Model.Message.Auction();
                var highestbidprce = auctionEntities.auctions.Where(c => c.product_id == auction.product_id).Max(p => p.bid_price);

                userAuction.AuctionId = auction.id;
                userAuction.CustomerId = auction.customer_id;
                userAuction.ProductId = auction.product_id;
                userAuction.BidPrice = auction.bid_price;
                userAuction.ProductBidTime = auction.product.product_bid_time.ToString(format);
                userAuction.ProductName = auction.product.product_Name;
                userAuction.ProductDescription = auction.product.product_description;
                userAuction.CurrentHighestBidPrice = highestbidprce;
                userAuction.BidStatus = auction.bid_status.status;
                userAuction.BidStatusReason = auction.bid_status_reason.status_reason;
                auctionResponse.Auctions.Add(userAuction);
            }
            return auctionResponse;
        }
        #endregion Get Customer Bids

        #region Get Bid
        //To get customerselected  bid
        public static Model.Message.Auction GetBid(int auctionid)
        {
            auctionEntities = new AuctionSystemEntities();
            Model.Message.Auction auction = new Model.Message.Auction();

            //retrieve auction from Auctions table with auctionid
            var dbAuction = auctionEntities.auctions.Where(c => c.id == auctionid).FirstOrDefault();
            //if auction is null set fault as no auctions
            if (dbAuction != null)
            {
                var highestBid = auctionEntities.auctions.Where(c => c.product_id == dbAuction.product_id).Max(p => p.bid_price);
                auction.AuctionId = dbAuction.id;
                auction.ProductId = dbAuction.product_id;
                auction.BidPrice = dbAuction.bid_price;
                auction.ProductBidTime = dbAuction.product.product_bid_time.ToString(format);
                auction.ProductName = dbAuction.product.product_Name;
                auction.ProductDescription = dbAuction.product.product_description;
                auction.BidStatus = ((dbAuction.auction_datetime < DateTime.Now) && dbAuction.bidstatus_id == (int)BidStatusCode.Active) ? "Inactive" : dbAuction.bid_status.status;
                auction.BidStatusReason = dbAuction.bid_status_reason.status_reason;
                auction.CurrentHighestBidPrice = highestBid;
            }
            return auction;
        }
        #endregion Get Bid

        #region Create Bid
        //To post user bids
        public static AuctionResponse CreateBid(AuctionRequest auctionRequest)
        {
            AuctionResponse auctionResponse = new AuctionResponse();
            auctionEntities = new AuctionSystemEntities();
            Model.Message.Auction auction = new Model.Message.Auction();

            //Retrieve user products to check if user is bidding on own product
            var userproduct = auctionEntities.products.Where(c => c.id == auctionRequest.ProductId && c.customer_id == auctionRequest.CustomerId).FirstOrDefault();
            if (userproduct != null)
            {
                auctionResponse.Error = new Error { Code = ErrorCodes.BidNotAllowed, Message = "Bidding on your product is not allowed" };
                return auctionResponse;
            }

            //retrieve auctions from autions table with productid to see if this is the first bid on product
            var dbAuctions = auctionEntities.auctions.Where(c => c.product_id == auctionRequest.ProductId).ToList();
            if (dbAuctions == null || dbAuctions.Count == 0)
            {
                //Retrieve product from product table to compare the bidding price
                var dbProduct = auctionEntities.products.Where(c => c.id == auctionRequest.ProductId).FirstOrDefault();
                if (dbProduct == null)
                {
                    auctionResponse.Error = new Error { Code = ErrorCodes.ProductUnavailable, Message = "Product is not available" };
                    return auctionResponse;
                }
                if (dbProduct.product_bid_price > auctionRequest.BidPrice)
                {
                    auctionResponse.Error = new Error { Code = ErrorCodes.InvalidBidPrice, Message = "Bid price cannot be less than minimum bid price" };
                    return auctionResponse;
                }

                auction dbAuction = new auction()
                {
                    customer_id = auctionRequest.CustomerId,
                    product_id = auctionRequest.ProductId,
                    bid_price = auctionRequest.BidPrice,
                    auction_datetime = auctionRequest.ProductBidTime,
                    bidstatus_id = (int)BidStatusCode.Active,
                    reason_id = (int)BidReasonCode.AuctionIsOpen
                };
                auctionEntities.auctions.Add(dbAuction);
                auctionEntities.SaveChanges();


                auction = new Model.Message.Auction()
                {
                    AuctionId = dbAuction.id,
                    ProductId = dbAuction.product_id
                };

                auctionResponse.Auctions = new List<Model.Message.Auction>();
                //add userauction object to list
                auctionResponse.Auctions.Add(auction);
                return auctionResponse;
            }

            var currentHighestBid = dbAuctions.Max(p => p.bid_price);

            //check if user has product in auction table already
            var dbExistingAuction = auctionEntities.auctions.Where(c => c.product_id == auctionRequest.ProductId && c.customer_id == auctionRequest.CustomerId).FirstOrDefault();
            if (dbExistingAuction != null)
            {
                if (auctionRequest.BidPrice < currentHighestBid)
                {
                    auctionResponse.Error = new Error { Code = ErrorCodes.InvalidBidPrice, Message = "Bid price cannot be less than current highest bid price" };
                    return auctionResponse;
                }
                auction = new Model.Message.Auction();
                dbExistingAuction.bid_price = auctionRequest.BidPrice;
                auctionEntities.SaveChanges();

                auction.AuctionId = dbExistingAuction.id;
                auction.ProductId = dbExistingAuction.product_id;
                auctionResponse.Auctions = new List<Model.Message.Auction>();
                auctionResponse.Auctions.Add(auction);
                return auctionResponse;
            }

            //compare currenthighestbid with userbid 
            if (currentHighestBid >= auctionRequest.BidPrice)
            {
                //if user bid is less then set fault as invalid price
                auctionResponse.Error = new Error { Code = ErrorCodes.InvalidBidPrice, Message = "Bid price cannot be less than current highest bid" };
                return auctionResponse;
            }

            //if user bid is higher than currenthighestbid 
            auction Auctions = new auction()
            {
                customer_id = auctionRequest.CustomerId,
                product_id = auctionRequest.ProductId,
                bid_price = auctionRequest.BidPrice,
                auction_datetime = auctionRequest.ProductBidTime,
                bidstatus_id = (int)BidStatusCode.Active,
                reason_id = (int)BidReasonCode.AuctionIsOpen
            };

            auctionEntities.auctions.Add(Auctions);
            auctionEntities.SaveChanges();

            auction = new Model.Message.Auction()
            {
                AuctionId = Auctions.id,
                ProductId = Auctions.product_id
            };
            auctionResponse.Auctions = new List<Model.Message.Auction>();
            auctionResponse.Auctions.Add(auction);
            return auctionResponse;
        }
        #endregion Create Bid

        #region Update Customer Bid
        //To update customer bid
        public static AuctionResponse UpdateCustomerBid(Model.Message.Auction auctionRequest)
        {
            auctionEntities = new AuctionSystemEntities();
            AuctionResponse auctionResponse = new AuctionResponse();

            var dbAuction = auctionEntities.auctions.Where(c => c.customer_id == auctionRequest.CustomerId && c.id == auctionRequest.AuctionId).FirstOrDefault();
            if (dbAuction == null || dbAuction.bidstatus_id != (int)StatusCode.Active || auctionRequest.BidStatus == "Inactive")
            {
                auctionResponse.Error = new Error { Code = ErrorCodes.NoAuctions, Message = "Auction unavailable" };
                return auctionResponse;
            }

            var currentHighestBid = auctionEntities.auctions.Where(c => c.product_id == dbAuction.product_id).Max(p => p.bid_price);
            if (currentHighestBid >= auctionRequest.BidPrice)
            {
                auctionResponse.Error = new Error { Code = ErrorCodes.InvalidBidPrice, Message = "Bid price should be more than current highest bid" };
                return auctionResponse;
            }

            dbAuction.bid_price = auctionRequest.BidPrice;
            auctionEntities.SaveChanges();

            auctionResponse.Auctions = new List<Model.Message.Auction>();
            Model.Message.Auction auction = new Model.Message.Auction()
            {
                AuctionId = dbAuction.id,
                BidStatus = dbAuction.bid_status.status,
                BidStatusReason = dbAuction.bid_status_reason.status_reason,
                BidPrice = dbAuction.bid_price,
                CurrentHighestBidPrice = currentHighestBid,
                CustomerId = dbAuction.customer_id,
                ProductDescription = dbAuction.product.product_description,
                ProductName = dbAuction.product.product_Name,
                ProductId = dbAuction.product_id
            };
            auctionResponse.Auctions.Add(auction);
            return auctionResponse;
        }
        #endregion Update Customer Bid
    }
}
