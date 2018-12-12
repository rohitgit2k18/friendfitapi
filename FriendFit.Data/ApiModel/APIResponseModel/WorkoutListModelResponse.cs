using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class WorkoutListModelResponse
    {
       public WorkoutListModelResponse()
        {
            Response = new WorkoutListModel();
        }
        public WorkoutListModel Response { get; set; }
    }

    public class WorkoutList
    {
        public Int64 WorkoutId { get; set; }
        public Int64 UserId { get; set; }
        public string Description { get; set; }
        public DateTime WorkDate { get; set; }
        public string DateOfWorkout { get; set; }
        public string StartTime { get; set; }
        public string Actual_StartTime { get; set; }
        public string FinishTime { get; set; }
        //public Nullable<System.TimeSpan> StartTime { get; set; }
        //public Nullable<System.TimeSpan> Actual_StartTime { get; set; }
        //public Nullable<System.TimeSpan> FinishTime { get; set; }
        public string WorkoutNotes { get; set; }
        public DateTime Createdate { get; set; }
        public bool IsActive { get; set; }
        public bool RowStatus { get; set; }
        public string Status { get; set; }
        public bool ScheduleWorkout { get; set; }
    }

    public class WorkoutListModel
    {
        public WorkoutListModel()
        {
            workoutlist = new List<APIResponseModel.WorkoutList>();
        }
        public List<WorkoutList> workoutlist { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

      
    }
}
