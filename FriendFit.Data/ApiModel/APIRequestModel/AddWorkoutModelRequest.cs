using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class AddWorkoutModelRequest
    {
        public Int64 UserId { get; set; }
        public string Description { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? FinishTime { get; set; }
        public string WorkoutNotes { get; set; }
        public bool AutoFinishTime { get; set; }

    }
}
