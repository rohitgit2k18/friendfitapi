using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class AddFriendInvitationRequestModel
    {
        public Int64 UserId { get; set; }
        public int DeliveryTypeId { get; set; }
        public string FriendsName { get; set; }
        public string DurationId { get; set; }

        public Int64? CountryId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int SubscriptionTypeId { get; set; }
        public decimal Cost { get; set; }
        public string Email { get; set; }
       
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The Mobile must contains 6 characters")]
        public string MobileNumber { get; set; }
    }
}
