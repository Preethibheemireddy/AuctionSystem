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
        public int ProductId { get; set; }
        public double BidPrice { get; set; }
        public System.DateTime ProductBidTime { get; set; }
        public int CustomerId { get; set; }
    }

    public class AuctionResponse
    {
        public List<Auction> Auctions { get; set; }
        public Error Error { get; set; }
    }

    public class Auction
    {
        public int AuctionId { get; set; }
        public int? ProductId { get; set; }
        public int? CustomerId { get; set; }
        public double BidPrice { get; set; }
        public string ProductBidTime { get; set; }
        public string ProductName { get; set; }
        public double CurrentHighestBidPrice { get; set; }
        public string ProductDescription { get; set; }
        public string BidStatus { get; set; }
        public string BidStatusReason { get; set; }
    }

}
