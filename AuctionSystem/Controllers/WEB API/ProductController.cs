using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Auction.Model.Message;
using Auction.BuisinessLogic;
using Auction.Model;
using Auction.Model.Data;

namespace AuctionSystemWebApi.Controllers
{
    public class ProductController : ApiController
    {
        // GET: api/Product/5
        [Route("api/Product")]
        public IHttpActionResult GetProductsByCustomerId(int customerid)
        {
            var products = CustomerProduct.GetProducts(customerid);
            return Ok(products);
        }

        [Route("api/Product/GetProduct")]
        public IHttpActionResult GetProduct(int productId)
        {
            var product = CustomerProduct.GetProduct(productId);
            return Ok(product);
        }

        [Route("api/Product")]
        public IHttpActionResult GetProductById(int productId)
        {
            var products = CustomerProduct.GetProducts(productId);
            return Ok(products);
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
        [Route("api/Product")]
        public IHttpActionResult UserProduct([FromBody] ProductRequest product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ProductResponse products = new ProductResponse();
                    products.Error = new Error { Code = ErrorCodes.ModelStateInvalid, Message = "Model state is invalid" };
                    return Ok(products);
                }
                var Product = CustomerProduct.Products(product);
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // PUT: api/Product/5
        [Route("api/Product/UpdateProducts")]
        public IHttpActionResult UpdateProducts([FromBody] ProductRequest product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ProductResponse products = new ProductResponse();
                    products.Error = new Error { Code = ErrorCodes.ModelStateInvalid, Message = "Model state is invalid" };
                    return Ok(products);
                }
                var Product = CustomerProduct.UpdateProducts(product);
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
