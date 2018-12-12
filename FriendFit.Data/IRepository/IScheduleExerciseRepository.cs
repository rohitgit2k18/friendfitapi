using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.IRepository
{
    public interface IScheduleExerciseRepository
    {
        int AddScheduleExercise(AddScheduleExerciseRequestModel objAddScheduleExerciseRequestModel);
        List<ScheduleList> ScheduleList(ListOfWorkoutRequestModel objListOfWorkoutRequestModel, string Search);       
        //ScheduleList ScheduleDetailsById(Int64 ScheduleId, Int64 userId);
        int AddScheduleExercise1(AddExerciseRequestModel objAddExerciseRequestModel);
        int EditScheduleExercise1(UpdateActualExerciseRequest objUpdateActualExerciseRequest);
        EditExerciseResponseModel ScheduleDetailsById(Int64 ScheduleId, Int64 userId);
    }
}
