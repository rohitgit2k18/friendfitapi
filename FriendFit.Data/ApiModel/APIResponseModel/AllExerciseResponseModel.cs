using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class all_EditExerciseResponseModel
    {
        public all_EditExerciseResponseModel()
        {
            Response = new all_EditExerciseResponse();
        }
        public all_EditExerciseResponse Response { get; set; }
        public string IsActual { get; set; }
    }
    public class all_ActualWeightModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SetsNumberActual { get; set; }
        public Int64? ActualExcerciseId { get; set; }
    }
    public class all_WeightModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public string SetsNumberActual { get; set; }
    }
    public class all_ActualLevelModel1
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SetsNumberActual { get; set; }
        public Int64? ActualExcerciseId { get; set; }
    }
    public class all_LevelModelView
    {
        public Int64 Id { get; set; }
        public decimal? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SetsNumberActual { get; set; }
    }

    public class all_ActualTimedModel1
    {
        public Int64? Id { get; set; }
        public int? TimedSet { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SetsNumberActual { get; set; }
        public Int64? ActualExcerciseId { get; set; }
    }
    public class all_TimedModelView
    {
        public Int64? Id { get; set; }
        public int? TimedSet { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SetsNumberActual { get; set; }
    }
    public class all_ActualRapsModel1
    {
        public Int64? Id { get; set; }
        public int? RepsSets { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SetsNumberActual { get; set; }
        public Int64? ActualExcerciseId { get; set; }
    }
    public class all_RapsModelView
    {
        public Int64? Id { get; set; }
        public int? RepsSets { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SetsNumberActual { get; set; }
    }
    public class all_ActualDistanceModel1
    {
        public Int64? Id { get; set; }
        public int? RepsSetsTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Km { get; set; }
        public Int64? ActualExcerciseId { get; set; }
    }
    public class all_DistanceModelView
    {
        public Int64? Id { get; set; }
        public int? RepsSetsTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Km { get; set; }
    }
    public class all_ActualTextModel1
    {
        public Int64? Id { get; set; }
        public string Text { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Int64? ActualExcerciseId { get; set; }
    }
    public class all_TextModelView
    {
        public Int64? Id { get; set; }
        public string Text { get; set; }
    }

    public class All_ActualDateExcerciseSet
    {
        public Int64? ExcerciseId { get; set; }
        public Int64? SetNum { get; set; }
        public DateTime? ExcerciseDate { get; set; }
    }

    

    public class all_EditExercise
    {
        public all_EditExercise()
        {
            weightList = new List<WeightModelView>();
            levelList = new List<LevelModelView>();
            timeList = new List<TimedModelView>();
            respList = new List<RapsModelView>();
            distance = new DistanceModelView();
            text = new TextModelView();
            actualWeightModel = new List<all_ActualWeightModelView>();
            actualLevelList = new List<all_ActualLevelModel1>();
            actualTimeList = new List<all_ActualTimedModel1>();
            actualRapsList = new List<all_ActualRapsModel1>();
            actualDistance = new List<all_ActualDistanceModel1>();
            actualText = new List<all_ActualTextModel1>();
            _ActualDateExcerciseSet = new List<All_ActualDateExcerciseSet>();
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
        public List<WeightModelView> weightList { get; set; }
        public List<LevelModelView> levelList { get; set; }
        public List<TimedModelView> timeList { get; set; }
        public List<RapsModelView> respList { get; set; }
        public DistanceModelView distance { get; set; }
        public TextModelView text { get; set; }

        public int? SetsNumberActual { get; set; }
        public decimal? ActualDistanceInKm { get; set; }
        public List<all_ActualWeightModelView> actualWeightModel { get; set; }
        public List<all_ActualLevelModel1> actualLevelList { get; set; }
        public List<all_ActualTimedModel1> actualTimeList { get; set; }
        public List<all_ActualRapsModel1> actualRapsList { get; set; }
        public List<all_ActualDistanceModel1> actualDistance { get; set; }
        public List<all_ActualTextModel1> actualText { get; set; }
        public List<All_ActualDateExcerciseSet> _ActualDateExcerciseSet { get; set; }

        
    }

    public class all_EditExerciseResponse
    {
        public all_EditExerciseResponse()
        {
            all_editExercise = new List<all_EditExercise>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<all_EditExercise> all_editExercise { get; set; }
    }
}
