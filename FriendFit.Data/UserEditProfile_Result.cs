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
    
    public partial class UserEditProfile_Result
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<long> CountryId { get; set; }
        public string ProfilePic { get; set; }
        public Nullable<long> OTP { get; set; }
        public Nullable<int> DeviceTypeId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> EmailConfirmed { get; set; }
        public Nullable<bool> AutoSMSSignUp { get; set; }
        public Nullable<bool> FullWorkoutStatus { get; set; }
        public Nullable<bool> WorkoutStatus { get; set; }
    }
}