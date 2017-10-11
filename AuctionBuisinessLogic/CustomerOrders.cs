using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Model.Message;
using Auction.Database;
using Auction.Model;
using Auction.Model.Data;

namespace Auction.BusinessLogic
{
   public  class CustomerOrders
    {
        //To retreive customer orders with customerid
        public static string format = "MMM d yyyy";

        #region Get Customer Orders
        public static OrdersResponse GetCustomerOrders(int customerid)
        {
            OrdersResponse ordersResponse = new OrdersResponse();
            AuctionSystemEntities auctionEntities = new AuctionSystemEntities();

            var orders = auctionEntities.orders.Where(c => c.customer_id == customerid).ToList();
            if (orders == null)
            {
                ordersResponse.Fault = new Error { Code = ErrorCodes.NoOrders, Message = "There are no orders for this customer" };
                return ordersResponse;
            }

            ordersResponse.CustomerOrders = new List<Order>();

            foreach (order order in orders)
            {
                Order customerOrders = new Order();
                customerOrders.AuctionId = order.auction_id;
                customerOrders.CustomerId = order.customer_id;
                customerOrders.CustomerPaymentId = order.customer_payment_id;
                customerOrders.OrderAmount = order.order_amount;
                customerOrders.OrderDatetime = order.order_datetime.ToString(format);
                customerOrders.OrderId = order.id;
                customerOrders.Status = order.customer_status.status;
                customerOrders.StatusReason = order.status_reason.reason;
                customerOrders.ProductName = order.auction.product.product_Name;
                customerOrders.ProductDescription = order.auction.product.product_description;
                ordersResponse.CustomerOrders.Add(customerOrders);
            }
            return ordersResponse;
        }
        #endregion Get Customer Orders

        #region Get Order
        //To retrieve customer order with orderid
        public static Order GetOrder(int orderid)
        {
            Order customerorder = new Order();
            AuctionSystemEntities auctionEntities = new AuctionSystemEntities();

            var order = auctionEntities.orders.Where(c => c.id == orderid).FirstOrDefault();
            if (order == null)
                return customerorder;

            customerorder.OrderId = order.id;
            customerorder.AuctionId = order.auction_id;
            customerorder.CustomerId = order.customer_id;
            customerorder.CustomerPaymentId = order.customer_payment_id;
            customerorder.OrderAmount = order.order_amount;
            customerorder.OrderDatetime = order.order_datetime.ToString(format);
            customerorder.ProductName = order.auction.product.product_Name;
            customerorder.ProductDescription = order.auction.product.product_description;
            customerorder.Status = order.customer_status.status;
            customerorder.StatusReason = order.status_reason.reason;
            return customerorder;
        }
        #endregion Get Order
    }
}
