using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class LoginModelResponse
    {
     
        public LoginModelResponse()
        {
            Response = new LoginResult();
        }
        public LoginResult Response { get; set; }
    }
    public class LoginResult
    {
       
        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public string TokenCode { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ProfilePic { get; set; }
    }
  
}
