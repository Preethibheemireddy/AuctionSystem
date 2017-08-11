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
   public static class CustomerProduct
    {
        private static AuctionSystemEntities auctionEntities;
        public static ProductModel products;
        public static ProductResponse productResponse;

        //To post the product to product table in database
        public static ProductResponse Products(ProductRequest product)
        {
            //create product object
            product Product = new product();
            Product.customer_id = product.Customer_Id;
            Product.product_type_id = product.ProductType_Id;
            Product.product_Name = product.Product_Name;
            Product.product_bid_price = product.Product_bid_price;
            Product.product_bid_time = product.Product_bid_time;
            Product.product_description = product.Product_description;
            Product.max_price = product.Max_Price;
            Product.status_id = (int)StatusCode.Active;
            Product.status_reason_id = (int)StatusReasonCode.ProductIsAvailable;
            //create new database entity
            auctionEntities = new AuctionSystemEntities();
            //add product object to products table in database and save
            auctionEntities.products.Add(Product);
            auctionEntities.SaveChanges();

            //create productresponse object
            productResponse = new ProductResponse();
            //create new productmodel
            products = new ProductModel();
            products.Customer_Id = Product.customer_id;
            products.ProductType_Id = Product.product_type_id;
            products.Product_bid_price = Product.product_bid_price;
            products.Product_bid_time = Product.product_bid_time;
            products.Product_Name = product.Product_Name;
            products.Product_description = Product.product_description;
            products.Max_Price = Product.max_price;
            products.ProductID = Product.id;
            //add productmodel object to productmodel list
            productResponse.Products = new List<ProductModel>();
            productResponse.Products.Add(products);

            return productResponse;


        }
        //To retrieve the product types
        public static ProductTypesResponse GetProductTypes()
        {
            //create new producttypesresponse object
            ProductTypesResponse response = new ProductTypesResponse();
            auctionEntities = new AuctionSystemEntities();
            //retrieve producttypes from productTypes table in database to a list
            var ProductTypes = auctionEntities.product_type.ToList();
            //if list is null
            if (ProductTypes == null)
            {
                //set fault as no product types
                response.Fault = new Error { Code = ErrorCodes.NoProductTypes, Message = "There are no Products" };
                return response;
            }
            //if list is not null then create producttypes list
            response.Products = new List<ProductTypes>();
            //loop through each item in list
            foreach (product_type product in ProductTypes)
            {
                //create new products object and set values for properties
                ProductTypes products = new ProductTypes();
                products.ProductType_Id = product.id;
                products.Product_type = product.type;
                //add products object to list of products
                response.Products.Add(products);
            }
            return response;
           
        }

        //To update products details
        public static ProductResponse UpdateProducts( ProductRequest CustomerProduct)
        {
            auctionEntities = new AuctionSystemEntities();
            //create new product response object
            productResponse = new ProductResponse();
            //retrieve products from product table with productid and customer id

           Database.product product = auctionEntities.products.Where(c => c.id == CustomerProduct.ProductID && c.customer_id == CustomerProduct.Customer_Id).FirstOrDefault();
            //if product is null
            if (product == null)
            {
                //set fault as no auctions for this product
                productResponse.Error = new Error { Code = ErrorCodes.ProductUnavailable, Message = "This product is not available" };
                return productResponse;

            }

            //if product is not null then update values for the product from CustomerProduct object
            product.customer_id = CustomerProduct.Customer_Id;
            product.product_type_id = CustomerProduct.ProductType_Id;
            product.product_Name = CustomerProduct.Product_Name;
            product.product_bid_price = CustomerProduct.Product_bid_price;
            product.product_bid_time = CustomerProduct.Product_bid_time;
            product.product_description = CustomerProduct.Product_description;
            product.max_price = CustomerProduct.Max_Price;
            product.status_id = (int) StatusCode.Active;
            product.status_reason_id = (int) StatusReasonCode.ProductIsAvailable;
            auctionEntities.SaveChanges();

            //create product model object and set updated values to the properties
            products = new ProductModel();
            products.Customer_Id = product.customer_id;
            products.ProductType_Id = product.product_type_id;
            products.Product_bid_price = product.product_bid_price;
            products.Product_bid_time = product.product_bid_time;
            products.Product_Name = product.product_Name;
            products.Product_description = product.product_description;
            products.Max_Price = product.max_price;
            products.ProductID = product.id;
            //add productmodel object to list
            productResponse.Products = new List<ProductModel>();
            productResponse.Products.Add(products);
            return productResponse;
        }

      
        //To retrieve products with customerid
        public static ProductResponse GetProducts(int customerId)
        {
            //create new database entity
            auctionEntities = new AuctionSystemEntities();
           //create new productresponse object
            productResponse = new ProductResponse();
            //retrieve products from product table with customerid into an array
            Database.product[] product = auctionEntities.products.Where(c => c.customer_id == customerId).ToArray();
            //if array is empty
            if (product == null)
            {
                //set fault as no products
                productResponse.Error = new Error { Code = ErrorCodes.ProductsUnavailable, Message = "This customer does not contain any products" };
               
                return productResponse;
                
            }

            //create new list of productmodel
            productResponse.Products = new List<ProductModel>();
            //if array is not null then loop through each item in array
            foreach (Database.product item in product)
            {
                //create new productmoedl and set values for properties
                products = new ProductModel();

                var userAuction = auctionEntities.auctions.Where(c => c.product_id == item.id).FirstOrDefault();
                if (userAuction != null)
                {
                    var currentHighestBid = auctionEntities.auctions.Where(c => c.product_id == item.id).Max(p => p.bid_price);
                    products.CurrentHighestBid = currentHighestBid;

                }
                
                products.Customer_Id = item.customer_id;
                products.ProductType_Id = item.product_type_id;
                products.Product_bid_price = item.product_bid_price;
                products.Product_bid_time = item.product_bid_time;
                products.Product_Name = item.product_Name;
                products.Product_description = item.product_description;
                products.Max_Price = item.max_price;
                products.ProductID = item.id;
                products.Status = item.customer_status.status;
                products.StatusReason = item.status_reason.reason;
                
               
                //add productmodel object to list
                productResponse.Products.Add(products);
            }
            
            return productResponse;
        }
        


       //To retrieve products with product type id
        public static ProductResponse GetProductsByProductTypeId(int productTypeId)
        {
            productResponse = new ProductResponse();
            auctionEntities = new AuctionSystemEntities();
            int status =  (int) StatusCode.Active;
            //retrieve products from product table with producttypeid into array
            Database.product[] product = auctionEntities.products.Where(c => c.product_type_id == productTypeId && c.status_id == status).ToArray();
            //if array is empty set fault as no products
            if (product == null)
            {
                productResponse.Error = new Error { Code = ErrorCodes.ProductsUnavailable, Message = "There are no products for this category" };
                
                return productResponse;
                
            }

            productResponse.Products = new List<ProductModel>();
            //if array is not empty loop through each item in array 
            foreach (Database.product item in product)
            {
                //create new productmodel and set values to properties
                products = new ProductModel();
                var userAuction = auctionEntities.auctions.Where(c => c.product_id == item.id).FirstOrDefault();
                if (userAuction != null)
                {
                    var currentHighestBid = auctionEntities.auctions.Where(c => c.product_id == item.id).Max(p => p.bid_price);
                    products.CurrentHighestBid = currentHighestBid;

                }
                //To get the current highest bid of the product

                    products.ProductType_Id = item.product_type_id;
                    products.Product_bid_price = item.product_bid_price;
                    products.Product_bid_time = item.product_bid_time;
                    products.Product_Name = item.product_Name;
                    products.Product_description = item.product_description;
                    products.Max_Price = item.max_price;
                    products.ProductID = item.id;
                    products.Status = item.customer_status.status;
                    products.StatusReason = item.status_reason.reason;
                   
                //create new list of productmodel
                //add productmodel object to list
                productResponse.Products.Add(products);
            }
            
            return productResponse;
        }
        
        //get Product by product id
        public static ProductModel GetProduct(int productId)
        {
            auctionEntities = new AuctionSystemEntities();
            ProductModel product = new ProductModel();
            // To retrieve product from database using productid
            var dBproduct = auctionEntities.products.Where(p => p.id == productId).FirstOrDefault();
            //To  check if product is null
            if (dBproduct == null)
            {
                return product;
            }
            //if product is not null check if product is present in auction table
            var auction = auctionEntities.auctions.Where(c => c.product_id == productId).FirstOrDefault();
            //if auction is not null then retrieve current highest bid price of the product from auction table
            if (auction != null)
            {
                var currentHighestBid = auctionEntities.auctions.Where(c => c.product_id == productId).Max(p => p.bid_price);
                product.CurrentHighestBid = currentHighestBid;

            }
            //set model object values
            product.Product_Name = dBproduct.product_Name;
            product.Product_description = dBproduct.product_description;
            product.Product_bid_price = dBproduct.product_bid_price;
            product.Product_bid_time = dBproduct.product_bid_time;
            product.Status = dBproduct.customer_status.status;
            product.Customer_Id = dBproduct.id;
            product.ProductID = dBproduct.id;
            product.ProductType_Id = dBproduct.product_type_id;
            product.Max_Price = dBproduct.max_price;

            return product;
        }

    }
}
