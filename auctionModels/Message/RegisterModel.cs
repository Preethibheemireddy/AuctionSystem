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
    public class RegisterRequest
    {
        
        public int CustomerId { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string CustomerLastName { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string CustomerFirstName { get; set; }

        [Display(Name = "Phone")]
        [Required]
        public string CustomerPhone { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string CustomerEmail { get; set; }

        [Display(Name = "Password")]
        [Required]
        public string CustomerPassword { get; set; }

        [Compare("CustomerPassword", ErrorMessage = "The Passwords do not match")]
        [Display(Name = "Confirm Password")]
        [Required]
        public string ConfirmPassword { get; set; }
        
        public int AddressId { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "state")]
        [Required]
        public string State { get; set; }

        [Display(Name = "ZipCode")]
        [Required]
        public int Zipcode { get; set; }

        [Display(Name = "Country")]
        [Required]
        public string Country { get; set; }

        [Display(Name = "Payment Method")]
        [Required]
        public int PaymentMethod { get; set; }


        [Display(Name = "Card Number")]
        [Required]
        public int CardNumber { get; set; }

        [Display(Name = "CardPin")]
        [Required]
        public int CardPin { get; set; }

        [Display(Name = "Card Expiry Date")]
        [Required]
        public System.DateTime CardExpiryDate { get; set; }

        [Display(Name = "Card Holder Name")]
        [Required]
        public string CardHolderName { get; set; }


    }

    public class RegisterResponse
    {
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerFirstName { get; set; }
        public List<PaymentMethodOption> PaymentMethodOptions { get; set; }
        public Error Fault { get; set; }
    }

    public class PaymentMethodTypes
    {
        public List<PaymentMethodOption> PaymentMethodOptions { get; set; }
        public Error Fault { get; set; }
    }



    public class PaymentMethodOption
    {
        public int PaymentMethod_ID { get; set; }
        public string Payment_Method { get; set; }
       

    }
}
