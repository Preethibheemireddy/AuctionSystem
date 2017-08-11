create table Customer_Status (
statusID  Int Not Null Primary Key Identity(1,1),
status Varchar(255) Not Null,
);

create table statusReason (
statusReason_ID  Int Not Null Primary Key Identity(1,1),
reason varchar(255) Not Null
);

create table customer (
customer_Id   Int  Not Null Primary key identity(1,1),
customer_Lastname  Varchar(255) Not Null,
customer_Firstname  Varchar(255) Not Null,
customer_Phone  Int Not Null,
customer_Email Varchar(255) Not Null,
customer_Password Varchar(255) Not Null,
statusID  Int Foreign Key References Customer_Status(statusID),
statusReason_ID   Int Foreign Key References statusReason(statusReason_ID),
);

create table customer_Address (
Address_Id  Int Not Null Primary Key Identity(1,1),
customer_Id  Int Foreign Key References customer(customer_Id),
Address1  Varchar(255) Not Null,
Address2 Varchar(255),
City Varchar(255) Not Null,
Address_State Varchar(255) Not Null,
Zipcode  Int Not Null,
country  Varchar(255) Not Null
);

create table paymentMethod (
paymentMethod_ID  Int Not Null Primary Key Identity(1,1),
payment_Method  Varchar(255) Not Null,
);

create table customerPayment (
customerPayment_ID  Int Not Null Primary Key identity(1,1),
paymentMethod_ID  Int Foreign Key References paymentMethod(paymentMethod_ID),
customer_Id  Int Foreign Key References customer(customer_Id),
Address_Id  Int Foreign Key References customer_Address(Address_Id),
statusID  Int Foreign Key References Customer_Status(statusID),
statusReason_ID   Int Foreign Key References statusReason(statusReason_ID),
);

create table productType (
productType_Id  Int Not Null Primary Key identity(1,1),
product_type   Varchar(255) Not Null
);

create table Product (
product_Id  Int Not Null Primary Key identity(1,1),
productType_Id Int Foreign Key References productType(productType_Id),
product_Name varchar(255) Not Null,
product_bid_price  Int Not Null,
product_bid_time  date  Not Null,
customer_Id Int Foreign Key References customer(customer_Id),
statusID  Int Foreign Key References Customer_Status(statusID),
statusReason_ID   Int Foreign Key References statusReason(statusReason_ID),
);

create table bidStatus (
bidStatus_ID  Int Not Null Primary Key identity(1,1),
bid_status  Varchar(255) Not Null,
);

create table bidStatusReason (
reason_ID Int Not Null Primary Key identity(1,1),
reason Varchar(255) Not Null,
);
create table Auction (
Auction_Id Int Not Null Primary Key Identity(1,1),
product_Id  Int Foreign Key References Product(product_Id),
bid_price  Int Not Null,
auction_datetime  date Not Null,
bidStatus_ID int Foreign Key references bisStatus(bidStatus_ID),
reason_ID  Int Foreign Key References bidStatusReason(reason_ID)
);

create table orders (
order_ID Int Not Null Primary Key identity(1,1),
order_amount Int Not Null,
order_datetime date Not Null,
customerPayment_ID Int Foreign Key references customerPayment(customerPayment_ID),
Auction_Id Int foreign Key references Auction(Auction_Id),
customer_Id Int Foreign Key references customer(customer_Id),
statusID  Int Foreign Key References Customer_Status(statusID),
statusReason_ID   Int Foreign Key References statusReason(statusReason_ID),
);