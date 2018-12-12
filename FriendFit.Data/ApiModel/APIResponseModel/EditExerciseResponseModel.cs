using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class EditExerciseResponseModel
    {
        public EditExerciseResponseModel()
        {
            Response = new EditExerciseResponse();
        }
        public EditExerciseResponse Response { get; set; }
        public string IsActual { get; set; }
    } 
    public class ActualWeightModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string ImperialType { get; set; }
    }
    public class WeightModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string ImperialType { get; set; }
    }
    public class ActualLevelModel1
    {

        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string ImperialType { get; set; }
    }
    public class LevelModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string ImperialType { get; set; }
    }

    public class ActualTimedModel1
    {
        public Int64? Id { get; set; }
       public int? TimedSet { get; set; }
    }
    public class TimedModelView
    {
        public Int64? Id { get; set; }
        public int? TimedSet { get; set; }
    }
    public class ActualRapsModel1
    {
        public Int64? Id { get; set; }
        public int? RepsSets { get; set; }
    }
    public class RapsModelView
    {
        public Int64? Id { get; set; }
        public int? RepsSets { get; set; }
    }
    public class ActualDistanceModel1
    {
        public Int64? Id { get; set; }
        public int? RepsSetsTime { get; set; }
    }
    public class DistanceModelView
    {
        public Int64? Id { get; set; }
        public int? RepsSetsTime { get; set; }
    }
    public class ActualTextModel1
    {
        public Int64? Id { get; set; }
        public string Text { get; set; }

    }
    public class TextModelView
    {
        public Int64? Id { get; set; }
        public string Text { get; set; }

    }

    public class EditExercise
    {
      public EditExercise()
        {
            weightList = new List<WeightModelView>();
            levelList = new List<LevelModelView>();
            timeList = new List<TimedModelView>();
            respList = new List<RapsModelView>();
            distance = new DistanceModelView();
            text = new TextModelView();
            actualWeightModel = new List<ActualWeightModelView>();
            actualLevelList = new List<ActualLevelModel1>();
            actualTimeList = new List<ActualTimedModel1>();
            actualRapsList = new List<ActualRapsModel1>();
            actualDistance = new List<ActualDistanceModel1>();
            actualText = new List<ActualTextModel1>();
        }
        public Int64 UserId { get; set; }
        public Int64 ExerciseId { get; set; }
        public Int64 WorkOutId { get; set; }
        public string ExerciseName { get; set; }
        public Int64 ExerciseTypeId { get; set; }
        public string ExerciseTypename { get; set; }
        public DateTime Createdate { get; set; }
        public Int64 CreatedBy { get; set; }
        public Boolean IsActive { get; set; }       
        public Int64? ExerciseSetId { get; set; }
        public int? SetsNumber{ get; set; }
        public decimal? DistanceInKm { get; set; }
        public string ImperialType { get; set; }
        public List<WeightModelView> weightList { get; set; }
        public List<LevelModelView> levelList { get;  set;}
        public List<TimedModelView> timeList { get; set; }
        public List<RapsModelView> respList { get; set; }
        public DistanceModelView distance { get; set; }
        public TextModelView text { get; set; }

        public int? SetsNumberActual { get; set; }
        public decimal? ActualDistanceInKm { get; set; }
        public List<ActualWeightModelView> actualWeightModel { get; set; }
        public List<ActualLevelModel1> actualLevelList { get; set; }
        public List<ActualTimedModel1> actualTimeList { get; set; }
        public List<ActualRapsModel1> actualRapsList { get; set; }
        public List<ActualDistanceModel1> actualDistance { get; set; }
        public List<ActualTextModel1> actualText { get; set; }
    }
   
    public class EditExerciseResponse
    {
        public EditExerciseResponse()
        {
            editExercise = new List<EditExercise>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<EditExercise> editExercise { get; set; }
    }
}
