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
        public List<Orders> CustomerOrders { get; set; }
        public Error Fault { get; set; }

    }


    public class Orders
    {
        public int Order_ID { get; set; }
        public int Order_amount { get; set; }
        public System.DateTime Order_datetime { get; set; }
        public int? CustomerPayment_ID { get; set; }
        public int? Auction_Id { get; set; }
        public int? Customer_Id { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public string Product_name { get; set; }
        public string Product_description { get; set; }
    }
}
