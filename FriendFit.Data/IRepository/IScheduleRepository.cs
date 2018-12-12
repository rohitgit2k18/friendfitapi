using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.IRepository
{
   public interface IScheduleRepository
    {
        int AddSchedule(AddScheduleRequestModel objAddScheduleRequestModel, Int64 UserId);
        List<ScheduleList> ScheduleList(Int64 UserId);
        ScheduleDetails ScheduleDetails(Int64 UserId,Int64 ScheduleId);
        int UpdateScheduleWorkout(UpdateScheduleWorkoutRequestModel objUpdateScheduleWorkoutRequestModel, Int64 ScheduleId, Int64 UserId);
        int DeleteSchedule(Int64 UserId, Int64 ScheduleId);
    }
}
