using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
    public class ActualWeightModel
    {
        public Int64? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string ImperialType { get; set; }
    }

    public class ActualLevelModel
    {
        public Int64? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string ImperialType { get; set; }
    }
    public class ActualTimedModel
    {
        public int? TimedSet { get; set; }
    }
    public class ActualRapsModel
    {
        public int? RepsSets { get; set; }
    }
    public class ActualDistanceModel
    {
        public int? RepsSetsTime { get; set; }

    }
    public class ActualTextModel
    {
     
        public string Text { get; set; }

    }

    public class UpdateActualExerciseRequestModel
    {
        public Int64 ExcerciseId { get; set; }     
        public string ExerciseName { get; set; }
        public Int64? ExerciseTypeId { get; set; }
        public int? SetsNumber { get; set; }
        public decimal? DistanceInKm { get; set; }
        public string ImperialType { get; set; }
        public string Text { get; set; }
        public List<ActualWeightModel> weightList { get; set; }
        public List<ActualLevelModel> levList { get; set; }
        public List<ActualTimedModel> timList { get; set; }
        public List<ActualRapsModel> rapList { get; set; }
        public List<ActualDistanceModel> disList { get; set; }
        public List<ActualTextModel> textListOfActual { get; set; }

    }
    public class UpdateActualExerciseRequest
    {
        public Int64 UserId { get; set; }
        public Int64 WorkOutId { get; set; }
        public List<UpdateActualExerciseRequestModel> UpdateList { get; set; }
    }



}
