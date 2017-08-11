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
        public int? ProductType_Id { get; set; }
        public int ProductID { get; set; }

       [Required]
        [Display(Name = "Product Name")]
        public string Product_Name { get; set; }
        [Required]
        [Display(Name = "Product bid price")]
        public double Product_bid_price { get; set; }

        [Display(Name = "product bid time")]
        [Required]
        public System.DateTime Product_bid_time { get; set; }
        public int? Customer_Id { get; set; }

        [Display(Name = "product description")]
        [Required]
        public string Product_description { get; set; }

        [Required]
        [Display(Name = "Max price")]

        public double Max_Price { get; set; }

    }

    public class ProductModel
    {
        public int? ProductType_Id { get; set; }
        public int ProductID { get; set; }
        [Display(Name = "Product Name")]
        [Required]
        public string Product_Name { get; set; }
        [Required]
        [Display(Name = "Product bid price")]
        public double Product_bid_price { get; set; }
        [Required]
        [Display(Name = "product bid time")]
        public System.DateTime Product_bid_time { get; set; }

        public int? Customer_Id { get; set; }
        [Required]
        [Display(Name = "product description")]
        public string Product_description { get; set; }
        [Required]
        [Display(Name = "Max price")]
        public double Max_Price { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public double CurrentHighestBid { get; set; }
    }

    public class ProductResponse
    {
        public List<ProductModel> Products { get; set; }
        public Error Error { get; set; }
        public double Bid_price { get; set; }
    }

    


    public class ProductTypes
    {
        public int ProductType_Id { get; set; }
        public string Product_type { get; set; }
        
    }

    public class ProductTypesResponse
    {
       public List <ProductTypes> Products { get; set; }
        public Error Fault { get; set; }
    }
}
