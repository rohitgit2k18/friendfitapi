using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class DashboardWorkoutDataResponse
    {
        public DashboardWorkoutDataResponse()
        {
            Response = new DashboardWorkoutModel();
        }
        public DashboardWorkoutModel Response { get; set; }
    }
    public class DashboardWorkout
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Missed { get; set; }
        public int Completed_Percentage { get; set; }
        public int Missed_Percentage { get; set; }
    }
    public class DashboardWorkoutModel
    {
        public DashboardWorkoutModel()
        {
            compWorkout = new DashboardWorkout();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public DashboardWorkout compWorkout { get; set; }
    }

}
