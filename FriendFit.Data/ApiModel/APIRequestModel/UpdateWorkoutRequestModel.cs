using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
    public class UpdateWorkoutRequestModel
    {
       
        public Int64 UserId { get; set; }
        public Int64 WorkOutId { get; set; }
        public string Description { get; set; }
        public string DateOfWorkout { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string WorkoutNotes { get; set; }
       
    }
}
