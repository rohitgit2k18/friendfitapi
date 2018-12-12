using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class AppUserDetailsResponseModel
    {
        public AppUserDetailsResponseModel()
        {
            Response = new AppUser();
        }
        public AppUser Response { get; set; }

    }
    public class AppUserDetails
    {
      public Int64 Id { get; set; }
      public string FirstName { get; set; }
      public string MiddleName { get; set; }
      public string LastName { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }
      public string MobileNumber { get; set; }
      public string CountryName { get; set; }

       public int RoleId { get; }
     public DateTime CreatedDate { get; set; }
     public Int64 CreatedBy { get; set; }
      public bool IsActive { get; set; }
     public bool EmailConfirmed { get; set; }
     public int OTP { get; set; }
     public string ProfilePic { get; set; }
     public int DeviceTypeId { get; set; }
    }

    public class AppUser
    {
        public AppUser()
        {
            appUser = new AppUserDetails();
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        public AppUserDetails appUser { get; set; }
    }
}
