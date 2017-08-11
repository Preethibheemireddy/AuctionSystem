﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionModel;
using Auction.Database;
using Auction.Model.Data;
using Auction.Model;
using System.Security.Cryptography;

namespace AuctionBuisinessLogic
{
    public class CustomerRegistration
    {
        private static AuctionSystemEntities auctionEntities;
        private static PaymentMethodOption PaymentResponse;
        private static List<PaymentMethodOption> PaymentMethods;

        public static List<PaymentMethodOption> Payment()
        {
            auctionEntities = new AuctionSystemEntities();
            PaymentMethods = new List<PaymentMethodOption>();

            //To retrieve the paymentmethods into an array from paymentmethod table in database.
            payment_method[] PaymentOptions = auctionEntities.payment_method.ToArray();
            //To check if paymentoptions array is empty, if null return paymentmehods list.
            if (PaymentOptions == null)
            {
                
                return PaymentMethods;
            }

            //If not null then loop through the items in array
            foreach (payment_method item in PaymentOptions)
            {
                //Create paymentResponse object
                PaymentResponse = new PaymentMethodOption();
                PaymentResponse.PaymentMethod_ID = item.id;
                PaymentResponse.Payment_Method = item.payment_method1;
                //Add paymentResponse object to paymentmethods list 
                PaymentMethods.Add(PaymentResponse);
            }


            return PaymentMethods;
        }

        public static RegisterResponse RegisterCustomer(RegisterRequest register)
        {
            auctionEntities = new AuctionSystemEntities();
            RegisterResponse response = new RegisterResponse();
            //To retrieve customers based on customer email from customer table in database.
            customer existingCustomer = auctionEntities.customers.Where(c => c.customer_email == register.CustomerEmail).FirstOrDefault();
            //Check if customer exists or not
            if (existingCustomer != null)
            {
                //if customer alreay exist then set fault as customer already exist.
                response.Fault = new Error { Code = ErrorCodes.EmailAlreadyExist, Message = "User Email already exist" };
                return response;
            }
            //if customer does not exist create customer object
            customer customer = new customer();
            customer.customer_firstname = register.CustomerFirstName;
            customer.customer_lastname = register.CustomerLastName;
            customer.customer_email = register.CustomerEmail;
            customer.customer_password = HashPassword(register.CustomerPassword);
            customer.customer_phone = register.CustomerPhone;

            //Add customer object to customer table in database and save 
            auctionEntities.customers.Add(customer);
            auctionEntities.SaveChanges();


            customer_address address = new customer_address();
            address.Address1 = register.Address1;
            address.Address2 = register.Address2;
            address.City = register.City;
            address.Address_State = register.State;
            address.Zipcode = register.Zipcode;
            address.country = register.Country;
            address.customer_id = customer.id;

            auctionEntities.customer_address.Add(address);
            auctionEntities.SaveChanges();

            customer_payment payment = new customer_payment();
            payment.card_number = register.CardNumber;
            payment.card_pin = register.CardPin;
            payment.card_holdername = register.CardHolderName;
            payment.card_expirydate = register.CardExpiryDate;
            payment.payment_method_id = register.PaymentMethod;
            payment.customer_id = customer.id;
            payment.address_id = address.id;
           
            auctionEntities.customer_payment.Add(payment);
            auctionEntities.SaveChanges();

            response.CustomerId = customer.id;
            response.CustomerEmail = customer.customer_email;
            response.CustomerFirstName = customer.customer_firstname;
            response.CustomerLastName = customer.customer_lastname;


            //Address in the response to be added

            return response;
        }

        //To hash the password
        public static string HashPassword(string txtPassword)
        {
            byte[] bytesPassword = Encoding.ASCII.GetBytes(txtPassword);
            MD5 hash = MD5.Create();
            byte[] byteshash = hash.ComputeHash(bytesPassword);
            return Encoding.ASCII.GetString(byteshash);
        }
    }
}
