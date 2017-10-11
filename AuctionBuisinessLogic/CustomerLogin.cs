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
    public static class CustomerLogin
    {
        private static AuctionSystemEntities auctionEntities;
        private static LoginModelResponse loginResponse;

        public static LoginModelResponse Login(LoginModelRequest login)
        {
            auctionEntities = new AuctionSystemEntities();
            loginResponse = new LoginModelResponse();
            //To retrive customers based on email.
            customer customer = auctionEntities.customers.Where(c => c.customer_email == login.Email).FirstOrDefault();
            //check if customer already exist or not
            if (customer == null)
            {
                //if customer do not exist set fault as invalid email
                loginResponse.Error = new Error { Code = ErrorCodes.InvalidEmail, Message = "User Email does not exist" };
                loginResponse.LoginIsValid = false;
                return loginResponse;
            }
            //if customer exists check if passwords match by hashing the password
            if (customer.customer_password != CustomerRegistration.HashPassword(login.Password))
            {
                //if paswords do not match set fault as invalid password
                loginResponse.Error = new Error { Code = ErrorCodes.InvalidPassword, Message = "User Email / Password does not match" };
                loginResponse.LoginIsValid = false;
                return loginResponse;
            }

            //if passwords match login the user by setting loginresponse
            loginResponse.Email = customer.customer_email;
            loginResponse.CustomerId = customer.id;
            loginResponse.CustomerLastName = customer.customer_lastname;
            loginResponse.CustomerFirstName = customer.customer_firstname;
            loginResponse.LoginIsValid = true;

            return loginResponse;
        }

    }
}
