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
    public class CustomerProduct
    {
        private static AuctionSystemEntities auctionEntities;
        public static Product product;
        public static string format = "MMM d yyyy";

        #region Get Customer Products
        //To retrieve products with customerid
        public static ProductResponse GetCustomerProducts(int customerId)
        {
            auctionEntities = new AuctionSystemEntities();
            ProductResponse productResponse = new ProductResponse();

            //retrieve products from product table with customerid
            product[] products = auctionEntities.products.Where(c => c.customer_id == customerId).ToArray();
            //if empty set fault
            if (products == null)
            {
                productResponse.Error = new Error { Code = ErrorCodes.ProductsUnavailable, Message = "This customer does not have any products" };
                return productResponse;
            }

            productResponse.Products = new List<Product>();
            //if array is not null then loop through each item in array
            foreach (product product in products)
            {
                CustomerProduct.product = new Product();

                var userAuction = auctionEntities.auctions.Where(c => c.product_id == product.id).ToList();
                if (userAuction.Count != 0)
                {
                    var currentHighestBid = userAuction.Max(p => p.bid_price);
                    CustomerProduct.product.CurrentHighestBid = currentHighestBid;
                }

                CustomerProduct.product.CustomerId = product.customer_id;
                CustomerProduct.product.ProductTypeId = product.product_type_id;
                CustomerProduct.product.ProductBidPrice = product.product_bid_price;
                CustomerProduct.product.ProductBidTime = product.product_bid_time.ToString(format);
                CustomerProduct.product.ProductName = product.product_Name;
                CustomerProduct.product.ProductDescription = product.product_description;
                CustomerProduct.product.MaxPrice = product.max_price;
                CustomerProduct.product.ProductId = product.id;
                CustomerProduct.product.Status = product.customer_status.status;
                CustomerProduct.product.StatusReason = product.status_reason.reason;

                productResponse.Products.Add(CustomerProduct.product);
            }
            return productResponse;
        }

        #endregion Get Customer Products

        #region Get Product
        //get Product by product id
        public static Product GetProduct(int productId)
        {
            auctionEntities = new AuctionSystemEntities();
            Product product = new Product();
            // To retrieve product from database using productid
            var dBproduct = auctionEntities.products.Where(p => p.id == productId).FirstOrDefault();
            //To  check if product is null
            if (dBproduct == null)
                return product;

            //if product is not null check if product is present in auction table
            var auctions = auctionEntities.auctions.Where(c => c.product_id == productId).ToList();
            //if auction is not null then retrieve current highest bid price of the product from auction table
            if (auctions.Count > 0)
            {
                var currentHighestBid = auctions.Max(p => p.bid_price);
                product.CurrentHighestBid = currentHighestBid;
            }

            product.ProductName = dBproduct.product_Name;
            product.ProductDescription = dBproduct.product_description;
            product.ProductBidPrice = dBproduct.product_bid_price;
            product.ProductBidTime = dBproduct.product_bid_time.ToString(format);
            product.Status = (dBproduct.product_bid_time < DateTime.Now && dBproduct.status_id == (int)StatusCode.Active) ? "Inactive" : dBproduct.customer_status.status;
            product.CustomerId = dBproduct.customer_id;
            product.ProductId = dBproduct.id;
            product.ProductTypeId = dBproduct.product_type_id;
            product.MaxPrice = dBproduct.max_price;

            return product;
        }
        #endregion Get Product

        #region Get Product Types
        //To retrieve the product types
        public static ProductTypesResponse GetProductTypes()
        {
            ProductTypesResponse productTypesResponse = new ProductTypesResponse();
            auctionEntities = new AuctionSystemEntities();
            //retrieve producttypes from productTypes table in database to a list
            var dbProductTypes = auctionEntities.product_type.ToList();
            //if list is null set fault as no product types
            if (dbProductTypes == null)
            {
                productTypesResponse.Fault = new Error { Code = ErrorCodes.NoProductTypes, Message = "There are no Products" };
                return productTypesResponse;
            }

            productTypesResponse.ProductTypes = new List<ProductTypes>();
            //loop through each item in list
            foreach (product_type dbProductType in dbProductTypes)
            {
                //create new products object and set values for properties
                ProductTypes productType = new ProductTypes()
                {
                    ProductTypeId = dbProductType.id,
                    ProductType = dbProductType.type
                };
                productTypesResponse.ProductTypes.Add(productType);
            }
            return productTypesResponse;
        }
        #endregion Get Product Types

        #region GetProductsByProductTypeId
        //To retrieve products with product type id
        public static ProductResponse GetProductsByProductTypeId(int productTypeId)
        {
            ProductResponse productResponse = new ProductResponse();
            auctionEntities = new AuctionSystemEntities();
            int status = (int)StatusCode.Active;
            //retrieve products from product table with producttypeid into array
            product[] dbProducts = auctionEntities.products.Where(c => c.product_type_id == productTypeId && c.status_id == status).ToArray();
            //if array is empty set fault as no products
            if (dbProducts == null)
            {
                productResponse.Error = new Error { Code = ErrorCodes.ProductsUnavailable, Message = "There are no products in this category" };
                return productResponse;
            }

            productResponse.Products = new List<Product>();
            //if array is not empty loop through each item in array 
            foreach (product dbProduct in dbProducts)
            {
                product = new Product();
                product.ProductTypeId = dbProduct.product_type_id;
                product.ProductBidPrice = dbProduct.product_bid_price;
                product.ProductBidTime = dbProduct.product_bid_time.ToString(format);
                product.ProductName = dbProduct.product_Name;
                product.ProductDescription = dbProduct.product_description;
                product.MaxPrice = dbProduct.max_price;
                product.ProductId = dbProduct.id;
                product.Status = dbProduct.customer_status.status;
                product.StatusReason = dbProduct.status_reason.reason;

                productResponse.Products.Add(product);
            }
            return productResponse;
        }
        #endregion GetProductsByProductTypeId

        #region CreateProduct
        //To create the product 
        public static ProductResponse CreateProduct(ProductRequest product)
        {
            //create product object
            product Product = new product();
            Product.customer_id = product.CustomerId;
            Product.product_type_id = product.ProductTypeId;
            Product.product_Name = product.ProductName;
            Product.product_bid_price = product.ProductBidPrice;
            Product.product_bid_time = product.ProductBidTime;
            Product.product_description = product.ProductDescription;
            Product.status_id = (int)StatusCode.Active;
            Product.status_reason_id = (int)StatusReasonCode.ProductIsAvailable;

            auctionEntities = new AuctionSystemEntities();
            auctionEntities.products.Add(Product);
            auctionEntities.SaveChanges();

            //create productresponse object
            ProductResponse productResponse = new ProductResponse();

            CustomerProduct.product = new Product();
            CustomerProduct.product.CustomerId = Product.customer_id;
            CustomerProduct.product.ProductTypeId = Product.product_type_id;
            CustomerProduct.product.ProductBidPrice = Product.product_bid_price;
            CustomerProduct.product.ProductBidTime = Product.product_bid_time.ToString(format);
            CustomerProduct.product.ProductName = product.ProductName;
            CustomerProduct.product.ProductDescription = Product.product_description;
            CustomerProduct.product.ProductId = Product.id;
            //add productmodel object to productmodel list
            productResponse.Products = new List<Product>();
            productResponse.Products.Add(CustomerProduct.product);

            return productResponse;
        }

        #endregion CreateProduct

        #region UpdateProduct
        //To update products details
        public static ProductResponse UpdateProduct(ProductRequest productRequest)
        {
            auctionEntities = new AuctionSystemEntities();
            //create new product response object
            ProductResponse productResponse = new ProductResponse();

            //retrieve products from product table with productid and customer id
            product dbproduct = auctionEntities.products.Where(c => c.id == productRequest.ProductID && c.customer_id == productRequest.CustomerId).FirstOrDefault();
            //if product is null
            if (dbproduct == null || dbproduct.product_bid_time <= DateTime.Now)
            {
                //set fault as no auctions for this product
                productResponse.Error = new Error { Code = ErrorCodes.ProductUnavailable, Message = "This product is not available" };
                return productResponse;
            }

            //if product is not null then update values for the product from CustomerProduct object
            dbproduct.customer_id = productRequest.CustomerId;
            dbproduct.product_type_id = productRequest.ProductTypeId;
            dbproduct.product_Name = productRequest.ProductName;
            dbproduct.product_bid_price = productRequest.ProductBidPrice;
            dbproduct.product_bid_time = productRequest.ProductBidTime;
            dbproduct.product_description = productRequest.ProductDescription;
            dbproduct.status_id = (int)StatusCode.Active;
            dbproduct.status_reason_id = (int)StatusReasonCode.ProductIsAvailable;

            auctionEntities.SaveChanges();

            //create product model object and set updated values to the properties
            CustomerProduct.product = new Product();
            CustomerProduct.product.CustomerId = dbproduct.customer_id;
            CustomerProduct.product.ProductTypeId = dbproduct.product_type_id;
            CustomerProduct.product.ProductBidPrice = dbproduct.product_bid_price;
            CustomerProduct.product.ProductBidTime = dbproduct.product_bid_time.ToString(format);
            CustomerProduct.product.ProductName = dbproduct.product_Name;
            CustomerProduct.product.ProductDescription = dbproduct.product_description;
            CustomerProduct.product.ProductId = dbproduct.id;
            //add productmodel object to list
            productResponse.Products = new List<Product>();
            productResponse.Products.Add(CustomerProduct.product);
            return productResponse;
        }

        #endregion UpdateProduct

    }
}
