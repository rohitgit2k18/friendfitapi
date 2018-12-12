using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class DurationListResponse
    {
        public DurationListResponse()
        {
            Response = new DurationListMethod();
        }
        public DurationListMethod Response { get; set; }
    }
    public class DurationList
    {
        public int Id { get; set; }
        public string TotalMonths { get; set; }
    }
    public class DurationListMethod
    {
        public DurationListMethod()
        {
            listofDuration = new List<DurationList>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<DurationList> listofDuration { get; set; }
    }
}
