//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FriendFit.Data
{
    using System;
    
    public partial class ListOfFriends_Result
    {
        public long FriendId { get; set; }
        public Nullable<long> UserId { get; set; }
        public Nullable<int> DeliveryTypeId { get; set; }
        public string DeliveryType { get; set; }
        public string FriendsName { get; set; }
        public Nullable<int> DurationId { get; set; }
        public string TotalMonths { get; set; }
        public string PurchaseDate { get; set; }
        public string ExpiryDate { get; set; }
        public Nullable<int> SubscriptionTypeId { get; set; }
        public string SubcriptionType { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsRowActive { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<long> CountryId { get; set; }
    }
}