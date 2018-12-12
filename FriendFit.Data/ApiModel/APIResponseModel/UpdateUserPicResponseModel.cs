using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class UpdateUserPicResponseModel
    {
        public UpdateUserPicResponseModel()
        {
            Response = new UpdateUserPicResponse();
        }
        public UpdateUserPicResponse Response { get; set; }
    }
    public class UpdateUserPicResponse
    {
        public string ProfilePic { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
