using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Model
{
   public enum BidStatusCode
    {
        Active = 1,
        Inactive = 2,
        Won = 3,
        Lost = 4,
        
    }

    public enum BidReasonCode
    {
        AuctionIsOpen = 1,
        AuctionIsClosed = 2,
        WonAuction = 3,
        LostAuction = 4,
    }
}
