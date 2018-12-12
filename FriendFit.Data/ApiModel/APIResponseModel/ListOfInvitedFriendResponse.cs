using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
  public  class ListOfInvitedFriendResponse
    {
        public ListOfInvitedFriendResponse()
        {
            Response = new ListOfInvitedFriendsModel();
        }
        public ListOfInvitedFriendsModel Response { get; set; }
    }
    public class ListOfInvitedFriends
    {
        public Int64 FriendId { get; set; }
        public Int64 UserId { get; set; }
        public int DeliveryTypeId { get; set; }
        public string DeliveryType { get; set; }
        public string FriendsName { get; set; }
        public string DurationId { get; set; }
        public int TotalMonths { get; set; }

        public string PurchaseDate { get; set; }
        public string ExpiryDate { get; set; }
        public int SubscriptionTypeId { get; set; }
        public string SubcriptionType { get; set; }
        public decimal Cost { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public Int64? CountryId { get; set; }

        public Int32? PaymentDone { get; set; }
        public string SKU{ get; set; }

    }
    public class ListOfInvitedFriendsModel
    {
        public ListOfInvitedFriendsModel()
        {
            listofFriends = new List<ListOfInvitedFriends>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<ListOfInvitedFriends> listofFriends { get; set; }
        
    }
}
