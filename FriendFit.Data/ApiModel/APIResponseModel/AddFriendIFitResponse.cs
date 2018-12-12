using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class AddFriendIFitResponse
    {
        public AddFriendIFitResponse()
        {
            Response = new AddFriendIFitResponseModel();
        }
        public AddFriendIFitResponseModel Response { get; set; }
    }
    public class AddFriendIFitResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
