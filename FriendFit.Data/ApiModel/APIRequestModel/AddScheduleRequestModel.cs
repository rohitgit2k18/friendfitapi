using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class AddScheduleRequestModel
    {
        public string WorkoutText { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public int RecurrenceId { get; set; }
        public TimeSpan ScheduleTime { get; set; }
        public TimeSpan TextMeTime { get; set; }
        public TimeSpan TextFriendTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }                   
}
