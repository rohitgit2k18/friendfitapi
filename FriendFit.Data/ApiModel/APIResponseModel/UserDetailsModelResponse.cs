using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class UserDetailsModelResponse
    {
        
       public UserDetailsModelResponse()
        {
            Response = new UserDetails();
        }

        public UserDetails Response { get; set; }
    }

    public class UserDetailsModel
    {
        public Int64 UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public Int64 CountryId { get; set; }
        public string ProfilePic { get; set; }
        public int? OTP { get; set; }
        public int? DeviceTypeId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? AutoSMSSignUp { get; set; }
        public bool? FullWorkoutStatus { get; set; }
        public bool? WorkoutStatus { get; set; }
        public Int32 WeightImperial { get; set; }
        public Int32 DistenceImperial { get; set; }

        public Int32? GetTotalMonths_App { get; set; }
        public Int32? GetTotalMonths_SMS { get; set; }

        public string PurchaseDate_App { get; set; }
        public string ExpiryDate_App { get; set; }
        public string PurchaseDate_SMS { get; set; }
        public string ExpiryDate_SMS { get; set; }
        public string GetUserRegistrationDate { get; set; }
        public string GetUserTrialExpiryDate { get; set; }
    }
    public class UserDetails
    {
        public UserDetails()
        {
            details = new UserDetailsModel();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public UserDetailsModel details { get; set; }
    }
}
