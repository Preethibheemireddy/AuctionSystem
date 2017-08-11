
CREATE TABLE [dbo].[bid_status] (
    [id] INT           IDENTITY (1, 1) NOT NULL,
    [status]   VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[bid_status_reason] (
    [id] INT           IDENTITY (1, 1) NOT NULL,
    [status_reason]    VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[customer_status] (
    [id] INT           IDENTITY (1, 1) NOT NULL,
    [status]   VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[status_reason] (
    [id] INT           IDENTITY (1, 1) NOT NULL,
    [reason]          VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[customer] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [customer_lastname]  VARCHAR (255) NOT NULL,
    [customer_firstname] VARCHAR (255) NOT NULL,
    [customer_phone]     VARCHAR (255) NOT NULL,
    [customer_email]     VARCHAR (255) NOT NULL,
    [customer_password]  VARCHAR (255) NOT NULL,
    [status_id]           INT           NULL,
    [status_reason_id]    INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([status_id]) REFERENCES [dbo].[customer_status] ([id]),
    FOREIGN KEY ([status_reason_id]) REFERENCES [dbo].[status_reason] ([id])
);

CREATE TABLE [dbo].[customer_address] (
    [id]    INT           IDENTITY (1, 1) NOT NULL,
    [customer_id]   INT           NULL,
    [Address1]      VARCHAR (255) NOT NULL,
    [Address2]      VARCHAR (255) NULL,
    [City]          VARCHAR (255) NOT NULL,
    [Address_State] VARCHAR (255) NOT NULL,
    [Zipcode]       INT           NOT NULL,
    [country]       VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[customer] ([id])
);

CREATE TABLE [dbo].[payment_method] (
    [id] INT           IDENTITY (1, 1) NOT NULL,
    [payment_method]   VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[customer_payment] (
    [id] INT           IDENTITY (1, 1) NOT NULL,
    [payment_method_id]   INT           NULL,
    [customer_id]        INT           NULL,
    [address_id]         INT           NULL,
    [status_id]           INT           NULL,
    [status_reason_id]    INT           NULL,
    [card_number]        INT           NOT NULL,
    [card_pin]           INT           NOT NULL,
    [card_expirydate]    DATE          NOT NULL,
    [card_holdername]    VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([payment_method_id]) REFERENCES [dbo].[payment_method] ([id]),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[customer] ([id]),
    FOREIGN KEY ([address_id]) REFERENCES [dbo].[customer_address] ([id]),
    FOREIGN KEY ([status_id]) REFERENCES [dbo].[customer_status] ([id]),
    FOREIGN KEY ([status_reason_id]) REFERENCES [dbo].[status_reason] ([id])
);


CREATE TABLE [dbo].[product_type] (
    [id] INT           IDENTITY (1, 1) NOT NULL,
    [type]   VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[product] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [product_type_id]      INT           NULL,
    [product_Name]        VARCHAR (255) NOT NULL,
    [product_bid_price]   FLOAT (53)    NOT NULL,
    [product_bid_time]    DATE          NOT NULL,
    [customer_id]         INT           NULL,
    [status_id]            INT           NULL,
    [status_reason_id]     INT           NULL,
    [product_description] VARCHAR (255) NOT NULL,
    [max_price]           FLOAT (53)    NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([product_type_id]) REFERENCES [dbo].[product_type] ([id]),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[customer] ([id]),
    FOREIGN KEY ([status_id]) REFERENCES [dbo].[customer_status] ([id]),
    FOREIGN KEY ([status_reason_id]) REFERENCES [dbo].[status_reason] ([id])
);

CREATE TABLE [dbo].[auction] (
    [id]       INT        IDENTITY (1, 1) NOT NULL,
    [product_id]       INT        NULL,
    [bid_price]        FLOAT (53) NOT NULL,
    [auction_datetime] DATE       NOT NULL,
    [bidstatus_id]     INT        NULL,
    [reason_id]        INT        NULL,
    [customer_id]      INT        NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([product_id]) REFERENCES [dbo].[product] ([id]),
    FOREIGN KEY ([bidstatus_id]) REFERENCES [dbo].[bid_status] ([id]),
    FOREIGN KEY ([reason_id]) REFERENCES [dbo].[bid_status_reason] ([id]),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[customer] ([id])
);

CREATE TABLE [dbo].[order] (
    [id]           INT  IDENTITY (1, 1) NOT NULL,
    [order_amount]       INT  NOT NULL,
    [order_datetime]     DATE NOT NULL,
    [customer_payment_id] INT  NULL,
    [auction_id]         INT  NULL,
    [customer_id]        INT  NULL,
    [status_id]           INT  NULL,
    [status_reason_id]    INT  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([customer_payment_id]) REFERENCES [dbo].[customer_payment] ([id]),
    FOREIGN KEY ([auction_id]) REFERENCES [dbo].[auction] ([id]),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[customer] ([id]),
    FOREIGN KEY ([status_id]) REFERENCES [dbo].[customer_status] ([id]),
    FOREIGN KEY ([status_reason_id]) REFERENCES [dbo].[status_reason] ([id])
);


