using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class EditScheduleExerciseResponseModel
    {
        public EditScheduleExerciseResponseModel()
            {
            Response = new EditScheduleExerciseModel();
            }
        public EditScheduleExerciseModel Response { get; set; }
    }
    public class ScheduleGoalSchExercise
    {
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
    }
    public class ScheduleActualSchExercise
    {
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
    }
    public class EditScheduleExerciseResponse
    {
        public EditScheduleExerciseResponse()
        {
            weightListSch = new List<ScheduleGoalSchExercise>();
            actualWeightModelSch = new List<ScheduleActualSchExercise>();
        }
        public Int64 UserId { get; set; }
        public Int64 WorkOutId { get; set; }
        public string ExerciseName { get; set; }
        public Int64 ExerciseTypeId { get; set; }
        public string ExerciseTypename { get; set; }
        public DateTime Createdate { get; set; }
        public Int64 CreatedBy { get; set; }
        public Boolean IsActive { get; set; }
        public int? SetsNumber { get; set; }
        public Int64? ExerciseSetId { get; set; }
        public List<ScheduleGoalSchExercise> weightListSch { get; set; }
        public List<ScheduleActualSchExercise> actualWeightModelSch { get; set; }
    }
    public class EditScheduleExerciseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public EditScheduleExerciseModel()
        {
           EditScheduleExerciselist = new List<EditScheduleExerciseResponse>();
        }
        public List<EditScheduleExerciseResponse> EditScheduleExerciselist { get; set; }
    }
}
