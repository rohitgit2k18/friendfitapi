using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.Repository
{
    public class ScheduleExerciseRepository : IScheduleExerciseRepository
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        public int AddScheduleExercise(AddScheduleExerciseRequestModel objAddScheduleExerciseRequestModel)
        {
            try
            {

                int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleExercise @UserId=@UserId,@WorkOutScheduleId=@WorkOutScheduleId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber",
                                                                                    new SqlParameter("UserId", objAddScheduleExerciseRequestModel.UserId),
                                                                                    new SqlParameter("WorkOutScheduleId", objAddScheduleExerciseRequestModel.WorkOutScheduleId),
                                                                                    new SqlParameter("ExerciseName", objAddScheduleExerciseRequestModel.ExerciseName),
                                                                                    new SqlParameter("ExerciseTypeId", objAddScheduleExerciseRequestModel.ExerciseTypeId),
                                                                                    new SqlParameter("SetsNumber", objAddScheduleExerciseRequestModel.SetsNumber));

                Int64 ScheduleGoalExcerciseSetsId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ScheduleGoalExcerciseSets ORDER BY ID DESC").FirstOrDefault();


                if (objAddScheduleExerciseRequestModel.weightScheduleList.Count > 0)
                {
                    foreach (var item in objAddScheduleExerciseRequestModel.weightScheduleList)
                    {
                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@Generated_ScheduleGoalExerciseSet_Id=@Generated_ScheduleGoalExerciseSet_Id",
                                                                                     new SqlParameter("TotalWeight", item.TotalWeight),
                                                                                     new SqlParameter("TotalRaps", item.TotalRaps),
                                                                                     new SqlParameter("Generated_ScheduleGoalExerciseSet_Id", ScheduleGoalExcerciseSetsId));

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return 1;
        }

        public List<ScheduleList> ScheduleList(ListOfWorkoutRequestModel objListOfWorkoutRequestModel, string Search)
        {
            List<ScheduleList> model = new List<ApiModel.APIResponseModel.ScheduleList>();
            try
            {
                model = _objFriendFitDBEntity.Database.SqlQuery<ScheduleList>("ScheduleList @UserId=@UserId, @Search = @Search",
                                                            new SqlParameter("UserId", objListOfWorkoutRequestModel.UserId),
                                                            new SqlParameter("Search", Search)).ToList();
            }
            catch (Exception ex)
            {
            }
            return model;
        }

        //this is for exercise details
        public EditExerciseResponseModel ScheduleDetailsById(Int64 ScheduleId, Int64 userId)
        {
            EditExerciseResponseModel objEditExerciseResponseModel = new EditExerciseResponseModel();
           
            var ActualWeightExercisesCollection = _objFriendFitDBEntity.ScheduleActualWeightExercises.ToList();
            var ActualLevelExercisesCollection = _objFriendFitDBEntity.ScheduleActualLevelExercises.ToList();
            var ActualRepsExercisesCollection = _objFriendFitDBEntity.ScheduleActualRepsExercises.ToList();
            var ActualDistanceExercisesCollection = _objFriendFitDBEntity.ScheduleActualDistanceExercises.ToList();
            var ActualTimeExercisesCollection = _objFriendFitDBEntity.ScheduleActualTimedExercises.ToList();
            var ActualTextExercisesCollection = _objFriendFitDBEntity.ScheduleActualFreeTextExercises.ToList();
            try
            {
                objEditExerciseResponseModel.Response.editExercise = _objFriendFitDBEntity.Database.SqlQuery<EditExercise>("EditScheduleexercise @WorkOutId=@WorkOutId,@UserId=@UserId",
                    new SqlParameter("WorkOutId", ScheduleId),
                    new SqlParameter("UserId", userId)).ToList();

                foreach (var item in objEditExerciseResponseModel.Response.editExercise)
                {
                    if (item.ExerciseTypeId == 1)
                    {
                        List<WeightModelView> listOfWeight = _objFriendFitDBEntity.Database.SqlQuery<WeightModelView>("Select Id,TotalWeight,TotalRaps, ImperialType from ScheduleGoalWeightExercise where ScheduleGoalExcerciseSetsId={0}", item.ExerciseSetId).ToList();
                        item.weightList = listOfWeight;
                    }
                    else if (item.ExerciseTypeId == 2)
                    {
                        List<LevelModelView> listOfLevel = _objFriendFitDBEntity.Database.SqlQuery<LevelModelView>("Select Id,TotalWeight,TotalRaps, ImperialType from ScheduleGoalLevelExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.levelList = listOfLevel;
                    }
                    else if (item.ExerciseTypeId == 3)
                    {
                        List<TimedModelView> listOfTime = _objFriendFitDBEntity.Database.SqlQuery<TimedModelView>("select Id,TimedSet from ScheduleGoalTimedExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.timeList = listOfTime;
                    }
                    else if (item.ExerciseTypeId == 4)
                    {
                        List<RapsModelView> listOfRaps = _objFriendFitDBEntity.Database.SqlQuery<RapsModelView>("select Id,RepsSets from ScheduleGoalRepsExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.respList = listOfRaps;
                    }
                    else if (item.ExerciseTypeId == 5)
                    {
                        DistanceModelView exOfDistance = _objFriendFitDBEntity.Database.SqlQuery<DistanceModelView>("select Id,RepsSetsTime from ScheduleGoalDistanceExercise where ExerciseSetId={0}", item.ExerciseSetId).FirstOrDefault();
                        item.distance = exOfDistance;
                    }
                    else if (item.ExerciseTypeId == 6)
                    {
                        TextModelView exOfText = _objFriendFitDBEntity.Database.SqlQuery<TextModelView>("select Id,Text from ScheduleGoalTextExercise where ExerciseId={0}", item.ExerciseId).FirstOrDefault();
                        item.text = exOfText;
                    }
                }

                var var_ActualWeightExercise = objEditExerciseResponseModel.Response.editExercise.Where(x => x.ExerciseTypeId == 1).FirstOrDefault();
                var var_ActualLevelExercise = objEditExerciseResponseModel.Response.editExercise.Where(x => x.ExerciseTypeId == 2).FirstOrDefault();
                var var_ActualTimedExercise = objEditExerciseResponseModel.Response.editExercise.Where(x => x.ExerciseTypeId == 3).FirstOrDefault();
                var var_ActualRepsExercise = objEditExerciseResponseModel.Response.editExercise.Where(x => x.ExerciseTypeId == 4).FirstOrDefault();
                var var_ActualDistanceExercise = objEditExerciseResponseModel.Response.editExercise.Where(x => x.ExerciseTypeId == 5).FirstOrDefault();
                var var_ActualFreeTextExercise = objEditExerciseResponseModel.Response.editExercise.Where(x => x.ExerciseTypeId == 6).FirstOrDefault();


                if (var_ActualWeightExercise != null)
                {
                    var Last_ActualWeightExercise_ExcerciseId = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == var_ActualWeightExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualWeightExercise_ExcerciseId != null)
                    {
                        ActualWeightExercisesCollection = _objFriendFitDBEntity.ScheduleActualWeightExercises.Where(x => x.ActualExerciseSetId == Last_ActualWeightExercise_ExcerciseId.Id).ToList();
                    }
                }

                if (var_ActualLevelExercise != null)
                {
                    var Last_ActualLevelExercise_ExcerciseId = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == var_ActualLevelExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualLevelExercise_ExcerciseId != null)
                    {
                        ActualLevelExercisesCollection = _objFriendFitDBEntity.ScheduleActualLevelExercises.Where(x => x.ActualExerciseSetId == Last_ActualLevelExercise_ExcerciseId.Id).ToList();
                    }
                }

                if (var_ActualRepsExercise != null)
                {
                    var Last_ActualRepsExercise_ExcerciseId = _objFriendFitDBEntity.ScheduleActualExcerciseSets.OrderByDescending(x => x.CreatedDate).Where(x => x.ExcerciseId == var_ActualRepsExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualRepsExercise_ExcerciseId != null)
                    {
                        ActualRepsExercisesCollection = _objFriendFitDBEntity.ScheduleActualRepsExercises.Where(x => x.ActualExerciseSetId == Last_ActualRepsExercise_ExcerciseId.Id).ToList();
                    }
                }

                if (var_ActualDistanceExercise != null)
                {
                    var Last_ActualDistanceExercise_ExcerciseId = _objFriendFitDBEntity.ScheduleActualExcerciseSets.OrderByDescending(x => x.CreatedDate).Where(x => x.ExcerciseId == var_ActualDistanceExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualDistanceExercise_ExcerciseId != null)
                    {
                        ActualDistanceExercisesCollection = _objFriendFitDBEntity.ScheduleActualDistanceExercises.Where(x => x.ActualExerciseSetId == Last_ActualDistanceExercise_ExcerciseId.Id).ToList();
                    }
                }

                if (var_ActualFreeTextExercise != null)
                {
                    var Last_ActualFreeTextExercise_ExcerciseId = _objFriendFitDBEntity.ScheduleActualExcerciseSets.OrderByDescending(x => x.CreatedBy).Where(x => x.ExcerciseId == var_ActualFreeTextExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualFreeTextExercise_ExcerciseId != null)
                    {
                        ActualTextExercisesCollection = _objFriendFitDBEntity.ScheduleActualFreeTextExercises.OrderByDescending(x => x.CreatedDate).Where(x => x.ExerciseId == Last_ActualFreeTextExercise_ExcerciseId.ExcerciseId).Take(1).ToList();
                    }
                }

                if (var_ActualTimedExercise != null)
                {
                    var Last_ActualTimedExercise_ExcerciseId = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == var_ActualTimedExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualTimedExercise_ExcerciseId != null)
                    {
                        ActualTimeExercisesCollection = _objFriendFitDBEntity.ScheduleActualTimedExercises.Where(x => x.ActualExerciseSetId == Last_ActualTimedExercise_ExcerciseId.Id).ToList();
                    }
                }

                foreach (var item in objEditExerciseResponseModel.Response.editExercise)
                {
                    if (item.ExerciseTypeId == 1)
                    {
                        var ActualExcerciseSetCollection = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (ActualExcerciseSetCollection != null)
                        {
                            //for actual Weight exercise data
                            item.SetsNumberActual = ActualExcerciseSetCollection.SetsNumber;
                            foreach (var item2 in ActualWeightExercisesCollection)
                            {
                                ActualWeightModelView model = new ActualWeightModelView();
                                model.TotalRaps = item2.TotalRaps;
                                model.TotalWeight = Convert.ToInt64(item2.TotalWeight);                      
                                model.Id = item2.Id;
                                item.actualWeightModel.Add(model);
                            }
                        }
                    }

                    if (item.ExerciseTypeId == 2)
                    {
                        var ActualExcerciseSetCollection1 = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (ActualExcerciseSetCollection1 != null)
                        {
                            item.SetsNumberActual = ActualExcerciseSetCollection1.SetsNumber;
                            foreach (var item3 in ActualLevelExercisesCollection)
                            {
                                ActualLevelModel1 model1 = new ActualLevelModel1();
                                model1.TotalRaps = item3.TotalRaps;
                                model1.TotalWeight = Convert.ToInt64(item3.TotalWeight);
                                item.actualLevelList.Add(model1);
                            }
                        }
                    }


                    if (item.ExerciseTypeId == 3)
                    {
                        var ActualExcerciseSetCollection2 = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (ActualExcerciseSetCollection2 != null)
                        {
                            item.SetsNumberActual = ActualExcerciseSetCollection2.SetsNumber;
                            foreach (var item3 in ActualTimeExercisesCollection)
                            {
                                ActualTimedModel1 model2 = new ActualTimedModel1();
                                model2.TimedSet = item3.TimedSet;
                                item.actualTimeList.Add(model2);
                            }
                        }
                    }

                    if (item.ExerciseTypeId == 4)
                    {
                        var ActualExcerciseSetCollection3 = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (ActualExcerciseSetCollection3 != null)
                        {
                            item.SetsNumberActual = ActualExcerciseSetCollection3.SetsNumber;
                            foreach (var item4 in ActualRepsExercisesCollection)
                            {
                                ActualRapsModel1 model3 = new ActualRapsModel1();
                                model3.Id = item4.Id;
                                model3.RepsSets = item4.RepsSets;
                                item.actualRapsList.Add(model3);
                            }
                        }
                    }

                    if (item.ExerciseTypeId == 5)
                    {
                        var ActualExcerciseSetCollection4 = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (ActualExcerciseSetCollection4 != null)
                        {
                            item.ActualDistanceInKm = ActualExcerciseSetCollection4.DistanceInKm;
                            item.SetsNumberActual = ActualExcerciseSetCollection4.SetsNumber;
                            foreach (var item5 in ActualDistanceExercisesCollection)
                            {
                                ActualDistanceModel1 model4 = new ActualDistanceModel1();
                                model4.RepsSetsTime = item5.RepsSetsTime;
                                model4.Id = item5.Id;
                                item.actualDistance.Add(model4);
                            }
                        }
                    }


                    if (item.ExerciseTypeId == 6)
                    {
                        var ActualExcerciseSetCollection5 = _objFriendFitDBEntity.ScheduleActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (ActualExcerciseSetCollection5 != null)
                        {
                            item.SetsNumberActual = ActualExcerciseSetCollection5.SetsNumber;
                            foreach (var item6 in ActualTextExercisesCollection)
                            {
                                ActualTextModel1 model5 = new ActualTextModel1();
                                model5.Id = item6.Id;
                                model5.Text = item6.Text;
                                item.actualText.Add(model5);
                            }
                        }
                    }
                }
                return objEditExerciseResponseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
      
        //this is for exercise details
        //public ScheduleList ScheduleDetailsById(Int64 ScheduleId, Int64 userId)
        //{
        //    ScheduleList obj_schedule = new ScheduleList();
        //    try
        //    {
        //        var var_Schedule = _objFriendFitDBEntity.Database.SqlQuery<ScheduleList>("ScheduleList @UserId=@UserId",
        //                                                    new SqlParameter("UserId", userId)).FirstOrDefault();

        //        obj_schedule.Schedule = var_Schedule.Schedule;
        //        obj_schedule.TextMeTime = var_Schedule.TextMeTime;
        //        obj_schedule.TextFriendTime = var_Schedule.TextFriendTime;
        //        obj_schedule.CreatedDate = var_Schedule.CreatedDate;
        //        obj_schedule.CreatedBy = var_Schedule.CreatedBy;
        //        obj_schedule.IsActive = var_Schedule.IsActive;
        //        obj_schedule.ScheduleId = var_Schedule.ScheduleId;
        //        obj_schedule.Description = var_Schedule.Description;
        //        obj_schedule.ScheduleTime = var_Schedule.ScheduleTime;
        //        obj_schedule.StartDate = var_Schedule.StartDate;
        //        obj_schedule.EndDate = var_Schedule.EndDate;
        //        obj_schedule.EndDate = var_Schedule.EndDate;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return obj_schedule;
        //}

        public int AddScheduleExercise1(AddExerciseRequestModel objAddExerciseRequestModel)
        {
            try
            {
                foreach (var item in objAddExerciseRequestModel.mainList)
                {
                    if (item.ExerciseTypeId == 6)
                    {
                        int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleGoalFreeTextExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@Text=@Text",
                                                                                       new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                       new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
                                                                                       new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                       new SqlParameter("ExerciseTypeId", (Object)item.ExerciseTypeId ?? DBNull.Value),
                                                                                       new SqlParameter("Text", item.Text));
                    }
                    else
                    {
                        int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleGoalExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm, @ImperialType=@ImperialType",
                                                                                      new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                      new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
                                                                                      new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                      new SqlParameter("ExerciseTypeId", item.ExerciseTypeId),
                                                                                      new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
                                                                                      new SqlParameter("ImperialType", item.ImperialType),
                                                                                      new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));


                        Int64 ExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ScheduleGoalExcerciseSets ORDER BY ID DESC").FirstOrDefault();
                        if (item.ExerciseTypeId == 1)
                        {
                            if (item.weightList.Count > 0)
                            {
                                foreach (var item1 in item.weightList)
                                {
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleGoalWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@Generated_ExerciseSet_Id=@Generated_ExerciseSet_Id, @ImperialType=@ImperialType",
                                                                                                 new SqlParameter("TotalWeight", item1.TotalWeight),
                                                                                                 new SqlParameter("TotalRaps", item1.TotalRaps),
                                                                                                 new SqlParameter("ImperialType", item1.ImperialType),
                                                                                                 new SqlParameter("Generated_ExerciseSet_Id", ExerciseSetId));

                                }
                            }
                        }
                        else if (item.ExerciseTypeId == 2)
                        {
                            if (item.levelList.Count > 0)
                            {
                                foreach (var item2 in item.levelList)
                                {
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleGoalLevelExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@ExerciseSetId=@ExerciseSetId, @ImperialType=@ImperialType",
                                                                                                 new SqlParameter("TotalWeight", item2.TotalWeight),
                                                                                                 new SqlParameter("TotalRaps", item2.TotalRaps),
                                                                                                 new SqlParameter("ImperialType", item2.ImperialType),
                                                                                                 new SqlParameter("ExerciseSetId", ExerciseSetId));

                                }
                            }
                        }
                        else if (item.ExerciseTypeId == 3)
                        {
                            if (item.timedList.Count > 0)
                            {
                                foreach (var item3 in item.timedList)
                                {
                                    if (item3.TimedSet != null)
                                    {
                                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleGoalTimedExercise @ExerciseSetId=@ExerciseSetId,@TimedSet=@TimedSet",
                                                                                                     new SqlParameter("ExerciseSetId", ExerciseSetId),
                                                                                                     new SqlParameter("TimedSet", item3.TimedSet));
                                    }
                                }
                            }
                        }
                        else if (item.ExerciseTypeId == 4)
                        {
                            if (item.rapList.Count > 0)
                            {
                                foreach (var item4 in item.rapList)
                                {
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleGoalRepsExercise @ExerciseSetId=@ExerciseSetId,@RepsSets=@RepsSets",
                                                                                                 new SqlParameter("ExerciseSetId", ExerciseSetId),
                                                                                                 new SqlParameter("RepsSets", item4.RepsSets));

                                }
                            }
                        }
                        else if (item.ExerciseTypeId == 5)
                        {
                            if (item.distance != null)
                            {

                                int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleGoalDistanceExercise @RepsSetsTime=@RepsSetsTime,@ExerciseSetId=@ExerciseSetId,@DistanceInKm=@DistanceInKm",
                                                                                             new SqlParameter("RepsSetsTime", item.distance.RepsSetsTime),
                                                                                             new SqlParameter("ExerciseSetId", ExerciseSetId),
                                                                                            new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));


                            }
                        }
                    }


                }



                ///// Insert Bulk
                var AllWorkout = _objFriendFitDBEntity.WorkOuts.ToList().Where(s => s.ScheduleId == objAddExerciseRequestModel.WorkOutId);
                foreach (var AllWorkout_item in AllWorkout)
                {
                    foreach (var item in objAddExerciseRequestModel.mainList)
                    {
                        if (item.ExerciseTypeId == 6)
                        {
                            int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddFreeTextExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@Text=@Text",
                                                                                           new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                           new SqlParameter("WorkOutId", AllWorkout_item.Id),
                                                                                           new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                           new SqlParameter("ExerciseTypeId", (Object)item.ExerciseTypeId ?? DBNull.Value),
                                                                                           new SqlParameter("Text", item.Text));
                        }
                        else
                        {
                            if (item.DistanceInKm!=null)
                            {
                                int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm, @ImperialType=@ImperialType",
                                                                  new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                  new SqlParameter("WorkOutId", AllWorkout_item.Id),
                                                                  new SqlParameter("ExerciseName", item.ExerciseName),
                                                                  new SqlParameter("ExerciseTypeId", item.ExerciseTypeId),
                                                                  new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
                                                                  new SqlParameter("ImperialType", item.ImperialType),
                                                                  new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));
                            }
                            else
                            {
                                decimal dis = 0;
                                int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm, @ImperialType=@ImperialType",
                                  new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                  new SqlParameter("WorkOutId", AllWorkout_item.Id),
                                  new SqlParameter("ExerciseName", item.ExerciseName),
                                  new SqlParameter("ExerciseTypeId", item.ExerciseTypeId),
                                  new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
                                  new SqlParameter("ImperialType", item.ImperialType),
                                  new SqlParameter("DistanceInKm", dis));
                            }


                            Int64 ExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ExcerciseSets ORDER BY ID DESC").FirstOrDefault();

                            if (item.ExerciseTypeId == 1)
                            {
                                if (item.weightList.Count > 0)
                                {
                                    foreach (var item1 in item.weightList)
                                    {                               
                                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@Generated_ExerciseSet_Id=@Generated_ExerciseSet_Id, @ImperialType=@ImperialType",
                                                             new SqlParameter("TotalWeight", item1.TotalWeight),
                                                             new SqlParameter("TotalRaps", item1.TotalRaps),
                                                             new SqlParameter("ImperialType", item1.ImperialType),
                                                             new SqlParameter("Generated_ExerciseSet_Id", ExerciseSetId));

                                    }
                                }
                            }
                            else if (item.ExerciseTypeId == 2)
                            {
                                if (item.levelList.Count > 0)
                                {
                                    foreach (var item2 in item.levelList)
                                    {
                                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddLevelExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@ExerciseSetId=@ExerciseSetId, @ImperialType=@ImperialType",
                                                                                                    new SqlParameter("TotalWeight", item2.TotalWeight),
                                                                                                    new SqlParameter("TotalRaps", item2.TotalRaps),
                                                                                                    new SqlParameter("ImperialType", item2.ImperialType),
                                                                                                    new SqlParameter("ExerciseSetId", ExerciseSetId));


                                    }
                                }
                            }
                            else if (item.ExerciseTypeId == 3)
                            {
                                if (item.timedList.Count > 0)
                                {
                                    foreach (var item3 in item.timedList)
                                    {
                                        if (item3.TimedSet!=null)
                                        {
                                            int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddTimedExercise @ExerciseSetId=@ExerciseSetId,@TimedSet=@TimedSet",
                                                                                                         new SqlParameter("ExerciseSetId", ExerciseSetId),
                                                                                                         new SqlParameter("TimedSet", item3.TimedSet));
                                        }
                                    }
                                }
                            }
                            else if (item.ExerciseTypeId == 4)
                            {
                                if (item.rapList.Count > 0)
                                {
                                    foreach (var item4 in item.rapList)
                                    {
                                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddRapsExercise @ExerciseSetId=@ExerciseSetId,@RepsSets=@RepsSets",
                                                                                                     new SqlParameter("ExerciseSetId", ExerciseSetId),
                                                                                                     new SqlParameter("RepsSets", item4.RepsSets));

                                    }
                                }
                            }

                            else if (item.ExerciseTypeId == 5)
                            {
                                if (item.distance != null)
                                {

                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddDistanceExercise @RepsSetsTime=@RepsSetsTime,@ExerciseSetId=@ExerciseSetId",
                                                                                                 new SqlParameter("RepsSetsTime", item.distance.RepsSetsTime),
                                                                                                 new SqlParameter("ExerciseSetId", ExerciseSetId));


                                }
                            }
                        }

                    }
                }




            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        public int EditScheduleExercise1(UpdateActualExerciseRequest objUpdateActualExerciseRequest)
        {
            try
            {
                foreach (var item in objUpdateActualExerciseRequest.UpdateList)
                {
                    if (item.ExerciseTypeId == 6)
                    {
                        if (item.Text!=null)
                        {
                            int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualFreeTextExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@Text=@Text",
                                                                                           new SqlParameter("UserId", objUpdateActualExerciseRequest.UserId),
                                                                                           new SqlParameter("WorkOutId", objUpdateActualExerciseRequest.WorkOutId),
                                                                                           new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                           new SqlParameter("ExerciseTypeId", (Object)item.ExerciseTypeId ?? DBNull.Value),
                                                                                           new SqlParameter("Text", item.Text));

                        }

                    }
                    else
                    {
                        int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm",
                                                                                      new SqlParameter("UserId", objUpdateActualExerciseRequest.UserId),
                                                                                      new SqlParameter("WorkOutId", objUpdateActualExerciseRequest.WorkOutId),
                                                                                      new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                      new SqlParameter("ExerciseTypeId", item.ExerciseTypeId),
                                                                                      new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
                                                                                      new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));

                        Int64 ActualExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ScheduleActualExcerciseSets ORDER BY ID DESC").FirstOrDefault();
                        if (item.ExerciseTypeId == 1)
                        {
                            if (item.weightList.Count > 0)
                            {
                                foreach (var item1 in item.weightList)
                                {
                                    int roweffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@Generated_ActualExerciseSet_Id=@Generated_ActualExerciseSet_Id",
                                                       new SqlParameter("TotalWeight", item1.TotalWeight),
                                                       new SqlParameter("TotalRaps", item1.TotalRaps),
                                                       new SqlParameter("Generated_ActualExerciseSet_Id", ActualExerciseSetId));

                                }
                            }
                        }
                        else if (item.ExerciseTypeId == 2)
                        {
                            if (item.levList.Count > 0)
                            {
                                foreach (var item2 in item.levList)
                                {
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualLevelExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@ActualExerciseSetId=@ActualExerciseSetId",
                                                                                                 new SqlParameter("TotalWeight", item2.TotalWeight),
                                                                                                 new SqlParameter("TotalRaps", item2.TotalRaps),
                                                                                                 new SqlParameter("ActualExerciseSetId", ActualExerciseSetId));

                                }
                            }
                        }
                        else if (item.ExerciseTypeId == 3)
                        {
                            if (item.timList.Count > 0)
                            {
                                foreach (var item3 in item.timList)
                                {
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddAScheduleActualTimedExercise @ActualExerciseSetId=@ActualExerciseSetId,@TimedSet=@TimedSet",
                                                                                                 new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
                                                                                                 new SqlParameter("TimedSet", item3.TimedSet));

                                }
                            }
                        }
                        else if (item.ExerciseTypeId == 4)
                        {
                            if (item.rapList.Count > 0)
                            {
                                foreach (var item4 in item.rapList)
                                {
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualRepsExercise @ActualExerciseSetId=@ActualExerciseSetId,@RepsSets=@RepsSets",
                                                                                                 new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
                                                                                                 new SqlParameter("RepsSets", item4.RepsSets));

                                }
                            }
                        }

                        //else if (item.ExerciseTypeId == 5)
                        //{
                        //    if (item.DistanceInKm != null)
                        //    {

                        //        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualDistanceExercise @RepsSetsTime=@RepsSetsTime,@ActualExerciseSetId=@ActualExerciseSetId, @DistanceInKM=@DistanceInKM",
                        //                                                                     new SqlParameter("RepsSetsTime", item),
                        //                                                                     new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
                        //                                                                     new SqlParameter("DistanceInKM", (Object)item.DistanceInKm ?? DBNull.Value));

                        //    }
                        //}

                        else if (item.ExerciseTypeId == 5)
                        {
                            if (item.disList != null)
                            {
                                foreach (var item10 in item.disList)
                                {
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualDistanceExercise @RepsSetsTime=@RepsSetsTime,@ActualExerciseSetId=@ActualExerciseSetId, @DistanceInKM=@DistanceInKM",
                                                                                             new SqlParameter("RepsSetsTime", item10.RepsSetsTime),
                                                                                             new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
                                                                                             new SqlParameter("DistanceInKM", (Object)item.DistanceInKm ?? DBNull.Value));



                                }
                            }
                        }

                        //else if (item.ExerciseTypeId == 6)
                        //{
                        //    var ActualExerciseId = _objFriendFitDBEntity.ScheduleExercises.Where(x => x.Id == objAddExerciseRequestModel.WorkOutId).FirstOrDefault().Id;

                        //    int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualFreeTextExercise1 @Text=@Text,@ExerciseId=@ExerciseId",
                        //                                                                       new SqlParameter("ExerciseId", ActualExerciseId),
                        //                                                                       new SqlParameter("Text", item.Text));
                        //}

                        else if (item.ExerciseTypeId == 6)
                        {
                            if (item.textListOfActual != null)
                            {
                                foreach (var item6 in item.textListOfActual)
                                {
                                    int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualFreeTextExercise1 @Text=@Text,@ExerciseId=@ExerciseId",
                                                                                               new SqlParameter("ExerciseId", item.ExcerciseId),
                                                                                               new SqlParameter("Text", item6.Text));
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        //public int EditScheduleExercise1(AddExerciseRequestModel objAddExerciseRequestModel)
        //{
        //    try
        //    {
        //        foreach (var item in objAddExerciseRequestModel.mainList)
        //        {
        //            if (item.ExerciseTypeId == 6)
        //            {
        //                int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualFreeTextExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@Text=@Text",
        //                                                                               new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
        //                                                                               new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
        //                                                                               new SqlParameter("ExerciseName", item.ExerciseName),
        //                                                                               new SqlParameter("ExerciseTypeId", (Object)item.ExerciseTypeId ?? DBNull.Value),
        //                                                                               new SqlParameter("Text", item.Text));
        //            }
        //            else
        //            {
        //                int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm",
        //                                                                              new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
        //                                                                              new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
        //                                                                              new SqlParameter("ExerciseName", item.ExerciseName),
        //                                                                              new SqlParameter("ExerciseTypeId", item.ExerciseTypeId),
        //                                                                              new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
        //                                                                              new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));

        //                Int64 ActualExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ScheduleActualExcerciseSets ORDER BY ID DESC").FirstOrDefault();
        //                if (item.ExerciseTypeId == 1)
        //                {
        //                    if (item.weightList.Count > 0)
        //                    {
        //                        foreach (var item1 in item.weightList)
        //                        {
        //                            int roweffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@Generated_ActualExerciseSet_Id=@Generated_ActualExerciseSet_Id",
        //                                               new SqlParameter("TotalWeight", item1.TotalWeight),
        //                                               new SqlParameter("TotalRaps", item1.TotalRaps),
        //                                               new SqlParameter("Generated_ActualExerciseSet_Id", ActualExerciseSetId));

        //                        }
        //                    }
        //                }
        //                else if (item.ExerciseTypeId == 2)
        //                {
        //                    if (item.levelList.Count > 0)
        //                    {
        //                        foreach (var item2 in item.levelList)
        //                        {
        //                            int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualLevelExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@ActualExerciseSetId=@ActualExerciseSetId",
        //                                                                                         new SqlParameter("TotalWeight", item2.TotalWeight),
        //                                                                                         new SqlParameter("TotalRaps", item2.TotalRaps),
        //                                                                                         new SqlParameter("ActualExerciseSetId", ActualExerciseSetId));

        //                        }
        //                    }
        //                }
        //                else if (item.ExerciseTypeId == 3)
        //                {
        //                    if (item.timedList.Count > 0)
        //                    {
        //                        foreach (var item3 in item.timedList)
        //                        {
        //                            int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddAScheduleActualTimedExercise @ActualExerciseSetId=@ActualExerciseSetId,@TimedSet=@TimedSet",
        //                                                                                         new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
        //                                                                                         new SqlParameter("TimedSet", item3.TimedSet));

        //                        }
        //                    }
        //                }
        //                else if (item.ExerciseTypeId == 4)
        //                {
        //                    if (item.rapList.Count > 0)
        //                    {
        //                        foreach (var item4 in item.rapList)
        //                        {
        //                            int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualRepsExercise @ActualExerciseSetId=@ActualExerciseSetId,@RepsSets=@RepsSets",
        //                                                                                         new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
        //                                                                                         new SqlParameter("RepsSets", item4.RepsSets));

        //                        }
        //                    }
        //                }

        //                else if (item.ExerciseTypeId == 5)
        //                {
        //                    if (item.distance != null)
        //                    {

        //                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualDistanceExercise @RepsSetsTime=@RepsSetsTime,@ActualExerciseSetId=@ActualExerciseSetId, @DistanceInKM=@DistanceInKM",
        //                                                                                     new SqlParameter("RepsSetsTime", item.distance.RepsSetsTime),
        //                                                                                     new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
        //                                                                                     new SqlParameter("DistanceInKM", (Object)item.DistanceInKm ?? DBNull.Value));

        //                    }
        //                }
        //                else if (item.ExerciseTypeId == 6)
        //                {
        //                    var ActualExerciseId = _objFriendFitDBEntity.ScheduleExercises.Where(x => x.Id == objAddExerciseRequestModel.WorkOutId).FirstOrDefault().Id;

        //                    int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddScheduleActualFreeTextExercise1 @Text=@Text,@ExerciseId=@ExerciseId",
        //                                                                                       new SqlParameter("ExerciseId", ActualExerciseId),
        //                                                                                       new SqlParameter("Text", item.Text));
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //    return 1;
        //}
    }
}
