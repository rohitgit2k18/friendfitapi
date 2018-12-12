using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class CompletedWorkoutResponse
    {
        public CompletedWorkoutResponse()
        {
            Response = new CompletedWorkoutModel();
        }
        public CompletedWorkoutModel Response { get; set; }
    }
    public class CompletWorkout
    {

        public int CompletedWorkout { get; set; }
    }
    public class CompletedWorkoutModel
    {
        public CompletedWorkoutModel()
        {
            compWorkout = new CompletWorkout();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public CompletWorkout compWorkout { get; set; }
    }

}
