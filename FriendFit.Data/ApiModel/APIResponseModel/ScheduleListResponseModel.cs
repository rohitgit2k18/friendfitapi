using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class ScheduleListResponseModel
    {
        public ScheduleListResponseModel()
        {
            Response = new ScheduleListModel();
        }
       public ScheduleListModel Response { get; set; }
    }

    public class ScheduleList
    {
        public Int64 ScheduleId { get; set; }
        public string Description { get; set; }
        public string Schedule { get; set; }
        public long WorkOutId { get; set; }
       
        public int ScheduleWeekly { get; set; }
        public string ScheduleTime { get; set; }
        public Int32 TextMeTime { get; set; }
        public Int32 TextFriendTime { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public bool IsActive { get; set; }

      public string WorkoutStartTime { get; set; }
        public string WorkoutFinishTime { get; set; }
        public bool AutoFinishTime { get; set; }
        public bool ScheduleOrNot { get; set; }


        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }

    public class ScheduleListModel
    {
        public ScheduleListModel()
        {
            scheduleLists = new List<ScheduleList>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<ScheduleList> scheduleLists { get; set; }
    }
}
