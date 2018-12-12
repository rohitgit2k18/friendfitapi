using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
    public class ScheduleWeightModel
    {
        public Int64? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
    }
    public class AddScheduleExerciseRequestModel
    {
        public Int64 UserId { get; set; }
        public Int64 WorkOutScheduleId { get; set; }
        public string ExerciseName { get; set; }
        public Int64? ExerciseTypeId { get; set; }
        public int? SetsNumber { get; set; }
        public List<ScheduleWeightModel> weightScheduleList { get; set; }
    }
}
