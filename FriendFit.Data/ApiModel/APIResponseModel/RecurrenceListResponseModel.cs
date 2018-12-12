using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class RecurrenceListResponseModel
    {
        public RecurrenceListResponseModel()
        {
            Response = new RecurrenceListModel();
        }
        public RecurrenceListModel Response { get; set; }
    }
    public class RecurrenceList
    {
        public int Id { get; set; }
        public string RecurrenceType { get; set; }
    }
    public class RecurrenceListModel
    {
        public RecurrenceListModel()
        {
            recurrenceList = new List<APIResponseModel.RecurrenceList>();
        }
        public List<RecurrenceList> recurrenceList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

    }
}
