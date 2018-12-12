using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.IRepository
{
   public interface IExerciseRepository
    {
        int AddExercise(AddExerciseRequestModel objAddExerciseRequestModel);
        int AddExerciseSchedule(AddExerciseRequestModel objAddExerciseRequestModel);
        EditExerciseResponseModel ExerciseDetailsByWorkOutId(Int64 WorkOutId,Int64 userId);
        //int UpdateExercise(UpdateActualExerciseRequestModel objUpdateActualExerciseRequestModel);
        int UpdateExercise(UpdateActualExerciseRequest objUpdateActualExerciseRequest);
        //List<WeightExercise> WeightExerciseList(Int64 ExerciseSetId);
        int UpdateActualExercise(Int64 UserId, Int64 ExerciseId, UpdatingActExistingRequest objReq);
        all_EditExerciseResponseModel AllPreviouseExerciseDetailsByWorkOutId(Int64 WorkOutId, Int64 userId);
    }
}
