using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class NotificationRequestModel
    {
        public string DeviceId { get; set; }
        public string DeviceType { get; set; }
        public string MessageBody { get; set; }
        public string Subject { get; set; }
    }
}
