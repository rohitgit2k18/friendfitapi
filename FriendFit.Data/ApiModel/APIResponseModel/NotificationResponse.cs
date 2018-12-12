using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class NotificationResponse
    {
        public NotificationResponse()
        {
            Responses = new NotificationModel();
        }
        public NotificationModel Responses { get; set; }
    }
    public class NotificationModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
