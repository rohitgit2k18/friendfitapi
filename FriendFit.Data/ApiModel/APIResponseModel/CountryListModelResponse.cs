using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class CountryListModelResponse
    {
        public CountryListModelResponse()
        {
            Response = new CountryModel();
        }
        public CountryModel Response { get; set; }
    }

 public class CountryList
    {
        public Int64 Id { get; set; }
        public string CountryName { get; set; }
    }

    public class CountryModel
    {
        public CountryModel()
        {
            CountryList = new List<APIResponseModel.CountryList>();
        }
        public List<CountryList> CountryList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

}
