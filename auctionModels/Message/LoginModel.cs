using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Database;
using Auction.Model.Data;
using System.ComponentModel.DataAnnotations;

namespace AuctionModel
{
   public class LoginModelRequest

    {
       
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

       
    }

   public class LoginModelResponse

    {
        public string Email { get; set; }
        public int CustomerId { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerFirstName { get; set; }

        public bool LoginIsValid { get; set; }
        
        public Error Error { get; set; }
    }
}
