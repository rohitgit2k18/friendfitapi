using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class ScheduleDetailsResponseModel
    {
        public ScheduleDetailsResponseModel()
        {
            Response = new ScheduleDetailsModels();
        }
        public ScheduleDetailsModels Response { get; set; }
    }

    public class ScheduleDetails
    {
        public Int64 ScheduleId { get; set; }       
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public int RecurrenceId { get; set; }
        public string ScheduleTime { get; set; }
        public Int32 TextMeTime { get; set; }
        public Int32 TextFriendTime { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Int64 WorkoutId { get; set; }
        public string Description { get; set; }
        public string Schedule { get; set; }

    }
    public class ScheduleDetailsModels
    {
        public ScheduleDetailsModels()
        {
            deatils = new ScheduleDetails();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ScheduleDetails deatils { get; set; }
    }

}
