using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Model.Data;
using System.ComponentModel.DataAnnotations;


namespace Auction.Model.Message
{
    public class ProductRequest
    {
        public int? ProductTypeId { get; set; }
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Product bid price")]
        public double ProductBidPrice { get; set; }

        [Display(Name = "product bid time")]
        [Required]
        public System.DateTime ProductBidTime { get; set; }
        public int? CustomerId { get; set; }

        [Display(Name = "product description")]
        [Required]
        public string ProductDescription { get; set; }


        [Display(Name = "Max price")]
        public double MaxPrice { get; set; }
    }

    public class ProductResponse
    {
        public List<Product> Products { get; set; }
        public Error Error { get; set; }
        public double BidPrice { get; set; }
    }


    public class Product
    {
        public int? ProductTypeId { get; set; }
        public int ProductId { get; set; }

        [Display(Name = "Product Name")]
        [Required]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Product bid price")]
        public double ProductBidPrice { get; set; }

        public double BidPrice { get; set; }

        [Required]
        [Display(Name = "Product bid time")]
        public string ProductBidTime { get; set; }

        public int? CustomerId { get; set; }

        [Required]
        [Display(Name = "Product description")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "Max price")]
        public double MaxPrice { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public double CurrentHighestBid { get; set; }
    }

    public class ProductTypes
    {
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
    }

    public class ProductTypesResponse
    {
        public List<ProductTypes> ProductTypes { get; set; }
        public Error Fault { get; set; }
    }
}
