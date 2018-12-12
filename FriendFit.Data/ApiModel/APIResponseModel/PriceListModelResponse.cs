using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class PriceListModelResponse
    {
        public PriceListModelResponse()
        {
            Response = new PriceListModel();
        }
        public PriceListModel Response { get; set; }
    }

    public class PriceList
    {
        public Int64 Id { get; set; }
        public string Duration { get; set; }
        public decimal Recurring { get; set; }
        public decimal One_Off { get; set; }
        public string IsSMS { get; set; }
        public DateTime? RowInsertDate { get; set; }
        public DateTime? RowInsertUpdate { get; set; }
    }

    public class PriceListModel
    {
        public PriceListModel()
        {
            PriceList = new List<APIResponseModel.PriceList>();
        }
        public List<PriceList> PriceList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }


    }


    public class PriceModelResponse
    {       
        public double? Price { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
