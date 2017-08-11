using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Model.Data;


namespace Auction.Model.Message
{
   public class AuctionRequest
    {
        public int  ProductID { get; set; }
        public double Bid_price { get; set; }
        public System.DateTime Product_bid_time { get; set; }
        public int Customer_Id { get; set; }
    }

    public class AuctionResponse
    {
        public List<UserAuction> Auctions { get; set; }
        public Error Error { get; set; }
       
    }

    public class UserAuction
    {
        public int Auction_Id { get; set; }
        public int? Product_Id { get; set; }
        public int? Customer_Id { get; set; }
        public double Bid_price { get; set; }
        public System.DateTime Product_bid_time { get; set; }
        public string Product_Name { get; set; }
        public double CurrentHighestBidPrice { get; set; }
        public string Product_description { get; set; }
        public string BidStatus { get; set; }
        public string BidStatus_Reason { get; set; }
    }

}
