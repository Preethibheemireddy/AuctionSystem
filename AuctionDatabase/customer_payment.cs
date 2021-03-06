//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Auction.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class customer_payment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public customer_payment()
        {
            this.orders = new HashSet<order>();
        }
    
        public int id { get; set; }
        public Nullable<int> payment_method_id { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> address_id { get; set; }
        public Nullable<int> status_id { get; set; }
        public Nullable<int> status_reason_id { get; set; }
        public int card_number { get; set; }
        public int card_pin { get; set; }
        public System.DateTime card_expirydate { get; set; }
        public string card_holdername { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual customer_address customer_address { get; set; }
        public virtual payment_method payment_method { get; set; }
        public virtual customer_status customer_status { get; set; }
        public virtual status_reason status_reason { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> orders { get; set; }
    }
}
