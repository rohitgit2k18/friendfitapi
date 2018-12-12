using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendFit.API.PayPal
{
    public class PayPalObjectInfo
    {
        public List<ProductInfo> ProductList { get; set; }
        public UserProduct up { get; set; }
        public string SiteURL { get; set; }
        public string InvoiceNumber { get; set; }
        public string Currency { get; set; }
        public string Tax { get; set; }
        public string ShippingFee { get; set; }
        public string OrderDescription { get; set; }    

        //for process payment
        public string PayerID { get; set; }
        public string PaymentID { get; set; }

        //getting payment history
        public int? Count { get; set; }
        public string UserId { get; set; }
        public string StartID { get; set; }
        public int? StartIndex { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartDate { get; set; }
        public string PayeeEmail { get; set; }
        public string PayeeID { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
    }


    public class UserProduct {
        public long DeliveryMethodId { get; set; }
        public Int32 SubscriptionTypeId { get; set; }
        public long DurationId { get; set; }
        public decimal Cost { get; set; }
        public long UserId { get; set; }       
    }
}