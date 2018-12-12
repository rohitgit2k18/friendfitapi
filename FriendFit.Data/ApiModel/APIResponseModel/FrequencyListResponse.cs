using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class FrequencyListResponse
    {
        public FrequencyListResponse()
        {
            Response = new FrequencyListMethod();
        }
        public FrequencyListMethod Response { get; set; }
    }

    public class FrequencyList
    {
        public int Id { get; set; }
        public string Frequency { get; set; }
    }
    public class FrequencyListMethod
    {
        public FrequencyListMethod()
        {
            listofFreq = new List<FrequencyList>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<FrequencyList> listofFreq { get; set; }
    }
}
