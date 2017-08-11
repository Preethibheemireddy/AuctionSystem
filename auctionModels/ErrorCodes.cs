using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Model
{
    public enum ErrorCodes
    {
        InvalidEmail = 1,
        InvalidPassword = 2,
        EmailAlreadyExist = 3,
        NoAuctions = 4,
        ModelStateInvalid = 5,
        NoProductTypes = 6,
        NoPaymentOptions = 7,
        NoOrders = 8,
        InvalidBidPrice = 9,
        ProductUnavailable = 10,
        ProductsUnavailable = 11,
        BidNotAllowed = 12,

    }
   
}
