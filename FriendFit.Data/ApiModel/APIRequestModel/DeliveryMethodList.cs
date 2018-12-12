using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{

    public class DeliveryMethodListResponse
    {
        public DeliveryMethodListResponse()
        {
            Response = new DeliveryMethodListModel();
        }
        public DeliveryMethodListModel Response { get; set; }
    }
   public class DeliveryMethodList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class DeliveryMethodListModel
    {
        public DeliveryMethodListModel()
        {
            ListOfDelivery = new List<DeliveryMethodList>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<DeliveryMethodList> ListOfDelivery { get; set; }
    }
}
