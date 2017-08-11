using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Model.Message;
using Auction.Database;
using Auction.Model.Data;
using Auction.Model;

namespace Auction.BuisinessLogic
{
    public static class CustomerAuction
    {
        private static AuctionSystemEntities auctionEntities;
        public static AuctionResponse auctionResponse;
        public static UserAuction userAuction;

        //To post user bids
        public static AuctionResponse UserBid(AuctionRequest auction)
        {
            //create new auctionresponse object
            auctionResponse = new AuctionResponse();
            auctionEntities = new AuctionSystemEntities();
            //To retrieve user products from product table with productid and customerid
            var userproduct = auctionEntities.products.Where(c => c.id == auction.ProductID && c.customer_id == auction.Customer_Id).FirstOrDefault();
            //To check if user is bidding for own product
            if (userproduct != null)
            {
                auctionResponse.Error = new Error { Code = ErrorCodes.BidNotAllowed, Message = "Bidding on your product is not allowed" };
                return auctionResponse;
            }

            //retrieve auctions from autions table with productid
            var userAuction = auctionEntities.auctions.Where(c => c.product_id == auction.ProductID).FirstOrDefault();
            //check if auction is null
            if (userAuction == null)
            {
                //if there are no auctions for this productid then retrieve product from product table
                var product = auctionEntities.products.Where(c => c.id == auction.ProductID).FirstOrDefault();
                //if productt is null set fault as product unavailable
                if (product == null)
                {

                    auctionResponse.Error = new Error { Code = ErrorCodes.ProductUnavailable, Message = "Product is not available" };
                    return auctionResponse;

                }
                //if product is not null compare user entered bid price with product bid price
                if (product.product_bid_price > auction.Bid_price)
                {
                    //if user bid price is less than product bid price then set fault as invalid bid price
                    auctionResponse.Error = new Error { Code = ErrorCodes.InvalidBidPrice, Message = "Bid price cannot be less than minimum bid price" };
                    return auctionResponse;

                }

                //create Auction object and set values to properties
                Database.auction Auction = new Database.auction()
                {

                    customer_id = auction.Customer_Id,
                    product_id = auction.ProductID,
                    bid_price = auction.Bid_price,
                    auction_datetime = auction.Product_bid_time,
                    bidstatus_id = (int)BidStatusCode.Active,
                    reason_id = (int)BidReasonCode.AuctionIsOpen
                };
                //add Auction object to Auctions table in database and save the database
                auctionEntities.auctions.Add(Auction);
                auctionEntities.SaveChanges();
                //create new userauction object
                CustomerAuction.userAuction = new UserAuction();
                //create new auctionresponse object
                auctionResponse = new AuctionResponse();
                //set values to userauction object properties
                CustomerAuction.userAuction.Auction_Id = Auction.id;
                CustomerAuction.userAuction.Product_Id = Auction.product_id;
                //create new list of userauction
                auctionResponse.Auctions = new List<UserAuction>();
                //add userauction object to list
                auctionResponse.Auctions.Add(CustomerAuction.userAuction);
                return auctionResponse;


            }
            var currentHighestBid = auctionEntities.auctions.Where(c => c.product_id == auction.ProductID).Max(p => p.bid_price);
            //To check if user has product in auction table already
            var response = auctionEntities.auctions.Where(c => c.product_id == auction.ProductID && c.customer_id == auction.Customer_Id).FirstOrDefault();

            if (response != null)
            {
                if (auction.Bid_price < currentHighestBid)
                {
                    auctionResponse.Error = new Error { Code = ErrorCodes.InvalidBidPrice, Message = "Bid price cannot be less than current highest bid price" };
                    return auctionResponse;
                }
                CustomerAuction.userAuction = new UserAuction();
                response.bid_price = auction.Bid_price;
                auctionEntities.SaveChanges();
                CustomerAuction.userAuction.Auction_Id = response.id;
                CustomerAuction.userAuction.Product_Id = response.product_id;
                auctionResponse.Auctions = new List<UserAuction>();
                auctionResponse.Auctions.Add(CustomerAuction.userAuction);
                return auctionResponse;
            }

            //compare currenthighestbid with userbid 
            if (currentHighestBid >= auction.Bid_price)
            {
                //if user bid is less then set fault as invalid price
                auctionResponse.Error = new Error { Code = ErrorCodes.InvalidBidPrice, Message = "Bid price cannot be less than current highest bid" };
                return auctionResponse;
            }
            //if user bid is higher than currenthighestbid then create new auction object and set values to properties
            Database.auction Auctions = new Database.auction()
            {
                customer_id = auction.Customer_Id,
                product_id = auction.ProductID,
                bid_price = auction.Bid_price,
                auction_datetime = auction.Product_bid_time,
                bidstatus_id = (int)BidStatusCode.Active,
                reason_id = (int)BidReasonCode.AuctionIsOpen
            };
            // add auction object to Auctions table in databse and save
            auctionEntities.auctions.Add(Auctions);
            auctionEntities.SaveChanges();
            //create new userauction object
            CustomerAuction.userAuction = new UserAuction();
            //create new auctionresponse object and set values to properties 
            auctionResponse = new AuctionResponse();
            CustomerAuction.userAuction.Auction_Id = Auctions.id;
            CustomerAuction.userAuction.Product_Id = Auctions.product_id;
            //create new list of usrauction
            auctionResponse.Auctions = new List<UserAuction>();
            //add auctions object to list
            auctionResponse.Auctions.Add(CustomerAuction.userAuction);
            return auctionResponse;

        }

        //To get customerselected  bid
        public static UserAuction GetCustomerBid(int auctionid)
        {
            auctionEntities = new AuctionSystemEntities();
            UserAuction userAuction = new UserAuction();
            //create new AuctionResponse object

            //retrieve auction from Auctions table with auctionid
            var auction = auctionEntities.auctions.Where(c => c.id == auctionid).FirstOrDefault();
            //if auction is null set fault as no auctions
            if (auction != null)
            {
                var HighestBid = auctionEntities.auctions.Where(c => c.product_id == auction.product_id).Max(p => p.bid_price);


                userAuction.Auction_Id = auction.id;
                userAuction.Product_Id = auction.product_id;
                userAuction.Bid_price = auction.bid_price;
                userAuction.Product_bid_time = auction.auction_datetime;
                userAuction.Product_Name = auction.product.product_Name;
                userAuction.Product_description = auction.product.product_description;
                userAuction.BidStatus = auction.bid_status.status;
                userAuction.BidStatus_Reason = auction.bid_status_reason.status_reason;
                userAuction.CurrentHighestBidPrice = HighestBid;
            }
           
            return userAuction;
        }


        //To get user bids with customer id
        public static AuctionResponse GetUserBids(int customerId)
        {
            auctionEntities = new AuctionSystemEntities();
            var auction = auctionEntities.auctions.Where(c => c.customer_id == customerId).ToList();
            auctionResponse = new AuctionResponse();
            if (auction == null)
            {
                auctionResponse.Error = new Error { Code = ErrorCodes.NoAuctions, Message = "This customer does not contain any auctions" };
                return auctionResponse;
            }
            //if auction is not null retrieve current highest bid price from Auction table with productid and max bid price 
            // create new UserAuction list
            auctionResponse.Auctions = new List<UserAuction>();
            
            foreach (Database.auction item in auction)
            {
                var userAuction = new UserAuction();
                var Highestbidprce = auctionEntities.auctions.Where(c => c.product_id == item.product_id).Max(p => p.bid_price);

                userAuction.Auction_Id = item.id;
                userAuction.Customer_Id = item.customer_id;
                userAuction.Product_Id = item.product_id;
                userAuction.Bid_price = item.bid_price;
                userAuction.Product_bid_time = item.auction_datetime;
                userAuction.Product_Name = item.product.product_Name;
                userAuction.Product_description = item.product.product_description;
                userAuction.CurrentHighestBidPrice = Highestbidprce;
                userAuction.BidStatus = item.bid_status.status;
                userAuction.BidStatus_Reason = item.bid_status_reason.status_reason;
               
                auctionResponse.Auctions.Add(userAuction);
            }

            return auctionResponse;
        }
        //To update customer bid
        public static AuctionResponse UpdateCustomerBid(UserAuction auction)
        {
            auctionEntities = new AuctionSystemEntities();

            var Auction = auctionEntities.auctions.Where(c => c.customer_id == auction.Customer_Id && c.id == auction.Auction_Id).FirstOrDefault();
            auctionResponse = new AuctionResponse();

            //int statuscode = (int)StatusCode.Active;
            if (Auction == null || Auction.bidstatus_id != (int) StatusCode.Active)

            {
                auctionResponse.Error = new Error { Code = ErrorCodes.NoAuctions, Message = "Auction unavailable" };

                return auctionResponse;

            }

            var currentHighestBid = auctionEntities.auctions.Where(c => c.product_id == Auction.product_id).Max(p => p.bid_price);
            if (currentHighestBid >= auction.Bid_price)
            {
                auctionResponse.Error = new Error { Code = ErrorCodes.InvalidBidPrice, Message = "Bid price cannot be less than current highest bid" };
                return auctionResponse;
            }

            Auction.bid_price = auction.Bid_price;
            auctionEntities.SaveChanges();
           
            auctionResponse.Auctions = new List<UserAuction>();
            var auctions = new UserAuction();
            auctions.Auction_Id = Auction.id;
            auctions.BidStatus = Auction.bid_status.status;
            auctions.BidStatus_Reason = Auction.bid_status_reason.status_reason;
            auctions.Bid_price = auction.Bid_price;
            auctions.CurrentHighestBidPrice = currentHighestBid;
            auctions.Customer_Id = auction.Customer_Id;
            auctions.Product_description = Auction.product.product_description;
            auctions.Product_Name = Auction.product.product_Name;
            auctions.Product_Id = Auction.product_id;
            auctionResponse.Auctions.Add(auctions);
            return auctionResponse;

        }
    }
}
