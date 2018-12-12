using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class PicUploadResponse
    {
        public PicUploadResponse()
        {
            Response = new PicUploadModel();
        }
        public PicUploadModel Response { get; set; }
    }
    public class PicUploadModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

}
