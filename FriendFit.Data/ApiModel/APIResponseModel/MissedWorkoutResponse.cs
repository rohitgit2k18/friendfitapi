using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class MissedWorkoutResponse
    {
        public MissedWorkoutResponse()
        {
            Response = new MissedWorkoutModel();
        }
        public MissedWorkoutModel Response { get; set; }
    }
    public class MissedWorkout
    {

        public int MissedWorkoutNo { get; set; }
    }
    public class MissedWorkoutModel
    {
        public MissedWorkoutModel()
        {
            missedWorkout = new MissedWorkout();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public MissedWorkout missedWorkout { get; set; }
    }
}
