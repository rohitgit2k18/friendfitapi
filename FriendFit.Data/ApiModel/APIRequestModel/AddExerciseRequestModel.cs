using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
    public class WeightModel
    {
        public Int64? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string ImperialType { get; set; }
    }
    public class LevelModel
    {
        public Int64? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string ImperialType { get; set; }
    }
    public class TimedModel
    {
        public int? TimedSet { get; set; }
    }
    public class RapsModel
    {
        public int? RepsSets { get; set; }
    }
    public class DistanceModel
    {
        public int? RepsSetsTime { get; set; }
        
    }
    public class FreeTextModel
    {
        public string Text { get; set; }
    }
    public class AddExerciseRequest
    {       
        public string ExerciseName { get; set; }

        public Int64? ExerciseTypeId { get; set; }

        public int? SetsNumber { get; set; }
        public decimal? DistanceInKm { get; set; }
        public string ImperialType { get; set; }

        public string Text { get; set; }

        public List<WeightModel> weightList { get; set; }
        public List<LevelModel> levelList { get; set; }
        public List<TimedModel> timedList { get; set; }
        public List<RapsModel> rapList { get; set; }
        public DistanceModel distance { get; set; }
        public FreeTextModel textList { get; set; }
    }
    public class AddExerciseRequestModel
    {
        public Int64 UserId { get; set; }
        public Int64 WorkOutId { get; set; }
        public List<AddExerciseRequest> mainList { get; set; }
    }
}
