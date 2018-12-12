using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.IRepository
{
   public interface IWorkoutRepository
    {
        int AddWorkout(AddWorkoutModelRequest objAddWorkoutModelRequest,Int64 UserId);
        List<WorkoutList> WorkoutList(ListOfWorkoutRequestModel objListOfWorkoutRequestModel);
        List<WorkoutList> WorkoutListForMobile(ListOfWorkoutRequestModel objListOfWorkoutRequestModel, string Search);
        WorkoutDetails Workoutdetails(WorkoutDetailsRequestModel objWorkoutDetailsRequestModel);
        int UpdateWorkoutDetailsById(UpdateWorkoutRequestModel objUpdateWorkoutRequestModel, Int64 UserId);
        PreviousExercise PreviousWorkOut(Int64 WorkoutId,Int64 UserId);
    }
}
