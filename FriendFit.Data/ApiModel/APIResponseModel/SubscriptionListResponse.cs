using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class SubscriptionListResponse
    {
        public SubscriptionListResponse()
        {
            Response = new SubscriptionListMethod();
        }
        public SubscriptionListMethod Response { get; set; }
    }

    public class SubscriptionList
    {
        public int Id { get; set; }
        public string SubcriptionType { get; set; }
    }
    public class SubscriptionListMethod
    {
        public SubscriptionListMethod()
        {
            listofSubscription = new List<SubscriptionList>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<SubscriptionList> listofSubscription { get; set; }
    }
}
