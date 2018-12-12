using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class PreActualWeightModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
    }
    public class PreWeightModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
    }
    public class PreActualLevelModel1
    {

        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
    }
    public class PreLevelModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
    }

    public class PreActualTimedModel1
    {
        public Int64? Id { get; set; }
        public int? TimedSet { get; set; }
    }
    public class PreTimedModelView
    {
        public Int64? Id { get; set; }
        public int? TimedSet { get; set; }
    }
    public class PreActualRapsModel1
    {
        public Int64? Id { get; set; }
        public int? RepsSets { get; set; }
    }
    public class PreRapsModelView
    {
        public Int64? Id { get; set; }
        public int? RepsSets { get; set; }
    }
    public class PreActualDistanceModel1
    {
        public Int64? Id { get; set; }
        public int? RepsSetsTime { get; set; }
    }
    public class PreDistanceModelView
    {
        public Int64? Id { get; set; }
        public int? RepsSetsTime { get; set; }
    }
    public class PreActualTextModel1
    {
        
        public Int64? Id { get; set; }
        public string Text { get; set; }

    }
    public class PreTextModelView
    {
        public Int64? Id { get; set; }
        public string Text { get; set; }

    }
 public class ListOfActualData
    {
        public ListOfActualData()
        {
            actualWeightModel = new List<PreActualWeightModelView>();
            actualLevelList = new List<PreActualLevelModel1>();
            actualTimeList = new List<PreActualTimedModel1>();
            actualRapsList = new List<PreActualRapsModel1>();
            actualDistance = new PreActualDistanceModel1();
            actualText = new PreActualTextModel1();
        }
        public int? SetsNumberActual { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal? ActualDistanceInKm { get; set; }
        public List<PreActualWeightModelView> actualWeightModel { get; set; }
        public List<PreActualLevelModel1> actualLevelList { get; set; }
        public List<PreActualTimedModel1> actualTimeList { get; set; }
        public List<PreActualRapsModel1> actualRapsList { get; set; }
        public PreActualDistanceModel1 actualDistance { get; set; }
        public PreActualTextModel1 actualText { get; set; }
    }
    
    public class FilterDataByDate
    {
        public DateTime Createddate { get; set; }
        public List<ListOfActualData> actualDataByDate { get; set; }
    }
    public class PreviousExercise
    {
        public PreviousExercise()
        {
            weightList = new List<PreWeightModelView>();
            levelList = new List<PreLevelModelView>();
            timeList = new List<PreTimedModelView>();
            respList = new List<PreRapsModelView>();
            distance = new PreDistanceModelView();
            text = new PreTextModelView();
            
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
        public int? SetsNumber { get; set; }
        public decimal? DistanceInKm { get; set; }
        public List<PreWeightModelView> weightList { get; set; }
        public List<PreLevelModelView> levelList { get; set; }
        public List<PreTimedModelView> timeList { get; set; }
        public List<PreRapsModelView> respList { get; set; }
        public PreDistanceModelView distance { get; set; }
        public PreTextModelView text { get; set; }

   public List<FilterDataByDate> dateResponse { get; set; }

    }
  
    public class PreviousExerciseModelResponse
    {
        public PreviousExerciseModelResponse()
        {
            Response = new PreviousExercise();
        }
       
        public PreviousExercise Response { get; set; }
       
    }
}
