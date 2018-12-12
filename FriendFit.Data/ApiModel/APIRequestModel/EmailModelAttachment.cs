using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class EmailModelAttachment
    {
        public string ToEmail { get; set; }
        public string messagebody { get; set; }
        public string Subject { get; set; }
        public string PayPalPaymentId { get; set; }
        public string FileURL { get; set; }
        public string CustomerName { get; set; }

        public string IsSMS { get; set; }
        public string Isrecurringmonthly { get; set; }
        public string ProductAmount { get; set; }
        public string includingGST { get; set; }
        public string TotalAmount { get; set; }

        public string FriendsHTML { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
