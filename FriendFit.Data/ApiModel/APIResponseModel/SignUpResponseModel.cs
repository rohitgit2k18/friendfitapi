using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{

    public class SignUpResponseModelResponse
    {
        public SignUpResponseModelResponse()
        {
            Response = new SignUpResponseModel();
        }
        public SignUpResponseModel Response { get; set; }
    }

   public class SignUpResponseModel
    {
        public Int64 UserId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
