using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Model.Data;

namespace Auction.Model.Message
{
   public class OrdersResponse
    {
        public List<Order> CustomerOrders { get; set; }
        public Error Fault { get; set; }

    }
    public class Order
    {
        public int OrderId { get; set; }
        public int OrderAmount { get; set; }
        public string OrderDatetime { get; set; }
        public int? CustomerPaymentId { get; set; }
        public int? AuctionId { get; set; }
        public int? CustomerId { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }
}
