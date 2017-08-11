using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Model
{
    public enum StatusCode
    {
        Active = 1,
        Inactive = 2,
        OrderPlaced = 3,

    }

    public enum StatusReasonCode
    {
        ProductIsAvailable = 1,
        ProductIsUnavailable = 2,
        YourOrderIsPlaced = 3,
    }
}
