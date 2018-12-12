using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class AddActualResponse
    {
        public AddActualResponse()
        {
            Response = new AddActualModel();
        }
        public AddActualModel Response { get; set; }
    }
    public class AddActualModel
    {

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
