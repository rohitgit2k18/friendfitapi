using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class WorkoutDetailsModelResponse
    {
        public WorkoutDetailsModelResponse()
        {
            Response = new WorkoutDetails();
        }
       public WorkoutDetails Response { get; set; }
    }
    public class WorkoutDetails
    {
        public Int64 WorkoutId { get; set; }
        public Int64 UserId { get; set; }
        public string Description { get; set; }
        public string DateOfWorkout { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string WorkoutNotes { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool AutoFinishTime { get; set; }
        public string Status { get; set; }
    }
}
