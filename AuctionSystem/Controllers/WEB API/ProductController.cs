using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Auction.Model.Message;
using Auction.BusinessLogic;
using Auction.Model;
using Auction.Model.Data;

namespace AuctionSystemWebApi.Controllers
{
    public class ProductController : ApiController
    {
        // GET: api/Product/5
        [Route("api/Products")]
        public IHttpActionResult GetProductsByCustomerId(int customerid)
        {
            var products = CustomerProduct.GetCustomerProducts(customerid);
            return Ok(products);
        }

        [Route("api/Product/GetProduct")]
        public IHttpActionResult GetProduct(int productId)
        {
            var product = CustomerProduct.GetProduct(productId);
            return Ok(product);
        }

        //Retrieve all product types.
        [Route("api/Product/GetProductTypes")]
        public IHttpActionResult GetProductTypes()
        {
            ProductTypesResponse products = CustomerProduct.GetProductTypes();
            return Ok(products);
        }

        //To retrieve all products with product type id.
        [Route("api/Product/GetProductByProductTypeId")]
        public IHttpActionResult GetProductByProductTypeId(int productTypeId)
        {
            ProductResponse product = CustomerProduct.GetProductsByProductTypeId(productTypeId);
            return Ok(product);
        }

        // POST products to database
        [Route("api/CreateProduct")]
        public IHttpActionResult CreateProduct([FromBody]ProductRequest productRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ProductResponse products = new ProductResponse();
                    products.Error = new Error { Code = ErrorCodes.ModelStateInvalid, Message = "Model state is invalid" };
                    return Ok(products);
                }
                var Product = CustomerProduct.CreateProduct(productRequest);
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // PUT: api/Product/5
        [Route("api/Product/UpdateProduct")]
        public IHttpActionResult UpdateProduct([FromBody] ProductRequest productRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ProductResponse products = new ProductResponse();
                    products.Error = new Error { Code = ErrorCodes.ModelStateInvalid, Message = "Model state is invalid" };
                    return Ok(products);
                }
                var Product = CustomerProduct.UpdateProduct(productRequest);
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
