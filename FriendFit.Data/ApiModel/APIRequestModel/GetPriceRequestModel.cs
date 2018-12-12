using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class GetPriceRequestModel
    {
        public string SendVia { get; set; }
        public string Billing { get; set; }
        public string Duration { get; set; }
    }
}
