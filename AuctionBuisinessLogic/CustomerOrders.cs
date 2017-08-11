using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Model.Message;
using Auction.Database;
using Auction.Model;
using Auction.Model.Data;

namespace Auction.BuisinessLogic
{
   public  class CustomerOrders
    {
        //To retreive customer orders with customerid
        public static OrdersResponse GetUserOrders(int customerid)
        {
            
            OrdersResponse UserOrders = new OrdersResponse();

            AuctionSystemEntities auctionEntities = new AuctionSystemEntities();

            var orders = auctionEntities.orders.Where(c => c.customer_id == customerid).ToList();

            if (orders == null)
            {
                UserOrders.Fault = new Error { Code = ErrorCodes.NoOrders, Message = "There are no orders for this customer" };

                return UserOrders;
            }

            UserOrders.CustomerOrders = new List<Orders>();

            foreach (Database.order item in orders)
            {
                Orders customer_orders = new Orders();
                customer_orders.Auction_Id = item.auction_id;
                customer_orders.Customer_Id = item.customer_id;
                customer_orders.CustomerPayment_ID = item.customer_payment_id;
                customer_orders.Order_amount = item.order_amount;
                customer_orders.Order_datetime = item.order_datetime;
                customer_orders.Order_ID = item.id;
                customer_orders.Status = item.customer_status.status;
                customer_orders.StatusReason = item.status_reason.reason;
                customer_orders.Product_name = item.auction.product.product_Name;
                customer_orders.Product_description = item.auction.product.product_description;
                UserOrders.CustomerOrders.Add(customer_orders);

            }


            return UserOrders;
        }
//To retrieve customer order with orderid
        public static Orders GetOrder(int orderid)
        {
            Orders customerorder = new Orders();
            AuctionSystemEntities auctionEntities = new AuctionSystemEntities();
            var order = auctionEntities.orders.Where(c => c.id == orderid).FirstOrDefault();
            if (order == null)
            {
                return customerorder;

            }
            customerorder.Order_ID = order.id;
            customerorder.Auction_Id = order.auction_id;
            customerorder.Customer_Id = order.customer_id;
            customerorder.CustomerPayment_ID = order.customer_payment_id;
            customerorder.Order_amount = order.order_amount;
            customerorder.Order_datetime = order.order_datetime;
            customerorder.Product_name = order.auction.product.product_Name;
            customerorder.Product_description = order.auction.product.product_description;
            customerorder.Status = order.customer_status.status;
            customerorder.StatusReason = order.status_reason.reason;
            return customerorder;
        }
    }
}
