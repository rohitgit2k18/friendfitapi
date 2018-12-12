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
    public class ExerciseRepository : IExerciseRepository
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        public int AddExercise(AddExerciseRequestModel objAddExerciseRequestModel)
        {
            try
            {
                var GetWorkOutCreatedDate = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == objAddExerciseRequestModel.WorkOutId).FirstOrDefault().DateOfWorkout.Value.Date;

                DateTime curentdate = DateTime.Now;
                string str_CurrentDate = curentdate.ToString("MM/dd/yyyy");
                string ActualDate = GetWorkOutCreatedDate.ToString("MM/dd/yyyy");

                foreach (var item in objAddExerciseRequestModel.mainList)
                {
                    if (str_CurrentDate == ActualDate)
                    {
                        if (item.ExerciseTypeId == 6)
                        {
                            int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualFreeTextExercise_FirstTime @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@Text=@Text",
                                                                                           new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                           new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
                                                                                           new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                           new SqlParameter("ExerciseTypeId", (Object)item.ExerciseTypeId ?? DBNull.Value),
                                                                                           new SqlParameter("Text", item.Text));
                        }
                        else
                        {
                            int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualExercise_FirstTime @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm, @ImperialType=@ImperialType",
                                                                                          new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                          new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
                                                                                          new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                          new SqlParameter("ExerciseTypeId", item.ExerciseTypeId),
                                                                                          new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
                                                                                          new SqlParameter("ImperialType", item.ImperialType),
                                                                                          new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));

                            Int64 ActualExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ActualExcerciseSets ORDER BY ID DESC").FirstOrDefault();
                            if (item.ExerciseTypeId == 1)
                            {
                                if (item.weightList.Count > 0)
                                {
                                    foreach (var item1 in item.weightList)
                                    {
                                        int roweffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@Generated_ActualExerciseSet_Id=@Generated_ActualExerciseSet_Id, @ImperialType=@ImperialType",
                                                           new SqlParameter("TotalWeight", item1.TotalWeight),
                                                           new SqlParameter("TotalRaps", item1.TotalRaps),
                                                           new SqlParameter("ImperialType", item1.ImperialType),
                                                           new SqlParameter("Generated_ActualExerciseSet_Id", ActualExerciseSetId));

                                    }
                                }
                            }
                            else if (item.ExerciseTypeId == 2)
                            {
                                if (item.levelList.Count > 0)
                                {
                                    foreach (var item2 in item.levelList)
                                    {
                                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualLevelExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@ActualExerciseSetId=@ActualExerciseSetId, @ImperialType=@ImperialType",
                                                                                                     new SqlParameter("TotalWeight", item2.TotalWeight),
                                                                                                     new SqlParameter("TotalRaps", item2.TotalRaps),
                                                                                                     new SqlParameter("ImperialType", item2.ImperialType),
                                                                                                     new SqlParameter("ActualExerciseSetId", ActualExerciseSetId));

                                    }
                                }
                            }
                            else if (item.ExerciseTypeId == 3)
                            {
                                if (item.timedList.Count > 0)
                                {
                                    foreach (var item3 in item.timedList)
                                    {
                                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualTimedExercise @ActualExerciseSetId=@ActualExerciseSetId,@TimedSet=@TimedSet",
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
                                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualRapsExercise @ActualExerciseSetId=@ActualExerciseSetId,@RepsSets=@RepsSets",
                                                                                                     new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
                                                                                                     new SqlParameter("RepsSets", item4.RepsSets));

                                    }
                                }
                            }

                            else if (item.ExerciseTypeId == 5)
                            {
                                if (item.distance != null)
                                {

                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualDistanceExercise @RepsSetsTime=@RepsSetsTime,@ActualExerciseSetId=@ActualExerciseSetId",
                                                                                                 new SqlParameter("RepsSetsTime", item.distance.RepsSetsTime),
                                                                                                 new SqlParameter("ActualExerciseSetId", ActualExerciseSetId));


                                }
                            }
                            else if (item.ExerciseTypeId == 6)
                            {
                                var ActualExerciseId = _objFriendFitDBEntity.Exercises.Where(x => x.WorkOutId == objAddExerciseRequestModel.WorkOutId).FirstOrDefault().Id;

                                int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualFreeTextExercise @Text=@Text,@ExerciseId=@ExerciseId",
                                                                                                   new SqlParameter("ExerciseId", ActualExerciseId),
                                                                                                   new SqlParameter("Text", item.Text));
                            }
                        }
                    }//----------------Main If Condition Close
                    else
                    {
                        if (item.ExerciseTypeId == 6)
                        {
                            int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddFreeTextExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@Text=@Text",
                                                                                           new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                           new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
                                                                                           new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                           new SqlParameter("ExerciseTypeId", (Object)item.ExerciseTypeId ?? DBNull.Value),
                                                                                           new SqlParameter("Text", item.Text));
                        }
                        else
                        {
                            int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm, @ImperialType=@ImperialType",
                                                                                          new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                          new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
                                                                                          new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                          new SqlParameter("ExerciseTypeId", item.ExerciseTypeId),
                                                                                          new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
                                                                                          new SqlParameter("ImperialType", item.ImperialType),
                                                                                          new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));

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
                                        int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddTimedExercise @ExerciseSetId=@ExerciseSetId,@TimedSet=@TimedSet",
                                                                                                     new SqlParameter("ExerciseSetId", ExerciseSetId),
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

        public int AddExerciseSchedule(AddExerciseRequestModel objAddExerciseRequestModel)
        {
            try
            {
                foreach (var item in objAddExerciseRequestModel.mainList)
                {
                    if (item.ExerciseTypeId == 6)
                    {
                        int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddFreeTextExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@Text=@Text",
                                                                                       new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                       new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
                                                                                       new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                       new SqlParameter("ExerciseTypeId", (Object)item.ExerciseTypeId ?? DBNull.Value),
                                                                                       new SqlParameter("Text", item.Text));
                    }
                    else
                    {
                        int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddExercise @UserId=@UserId,@WorkOutId=@WorkOutId,@ExerciseName=@ExerciseName,@ExerciseTypeId=@ExerciseTypeId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm",
                                                                                      new SqlParameter("UserId", objAddExerciseRequestModel.UserId),
                                                                                      new SqlParameter("WorkOutId", objAddExerciseRequestModel.WorkOutId),
                                                                                      new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                      new SqlParameter("ExerciseTypeId", item.ExerciseTypeId),
                                                                                      new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
                                                                                      new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));

                        Int64 ExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ExcerciseSets ORDER BY ID DESC").FirstOrDefault();

                        if (item.ExerciseTypeId == 1)
                        {
                            if (item.weightList.Count > 0)
                            {
                                foreach (var item1 in item.weightList)
                                {
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@Generated_ExerciseSet_Id=@Generated_ExerciseSet_Id",
                                                                                                 new SqlParameter("TotalWeight", item1.TotalWeight),
                                                                                                 new SqlParameter("TotalRaps", item1.TotalRaps),
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
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddLevelExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@ExerciseSetId=@ExerciseSetId",
                                                                                                 new SqlParameter("TotalWeight", item2.TotalWeight),
                                                                                                 new SqlParameter("TotalRaps", item2.TotalRaps),
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
                                    int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddTimedExercise @ExerciseSetId=@ExerciseSetId,@TimedSet=@TimedSet",
                                                                                                 new SqlParameter("ExerciseSetId", ExerciseSetId),
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
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        //this is for exercise details
        public EditExerciseResponseModel ExerciseDetailsByWorkOutId(Int64 WorkOutId, Int64 userId)
        {
            EditExerciseResponseModel objEditExerciseResponseModel = new EditExerciseResponseModel();
            //IEnumerable<ActualWeightExercise> ActualWeightExercisesCollection;
            //IEnumerable<ActualLevelExercise> ActualLevelExercisesCollection;
            //IEnumerable<ActualRepsExercise> ActualRepsExercisesCollection;
            //IEnumerable<ActualDistanceExercise> ActualDistanceExercisesCollection;
            //IEnumerable<ActualTimedExercise> ActualTimeExercisesCollection;
            //IEnumerable<ActualFreeTextExercise> ActualTextExercisesCollection;

            var ActualWeightExercisesCollection = _objFriendFitDBEntity.ActualWeightExercises.ToList();
            var ActualLevelExercisesCollection = _objFriendFitDBEntity.ActualLevelExercises.ToList();
            var ActualRepsExercisesCollection = _objFriendFitDBEntity.ActualRepsExercises.ToList();
            var ActualDistanceExercisesCollection = _objFriendFitDBEntity.ActualDistanceExercises.ToList();
            var ActualTimeExercisesCollection = _objFriendFitDBEntity.ActualTimedExercises.ToList();
            var ActualTextExercisesCollection = _objFriendFitDBEntity.ActualFreeTextExercises.ToList();
            try
            {
                var GetWorkOutCreatedDate = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == WorkOutId).FirstOrDefault().Createdate.Value.Date;

                if (GetWorkOutCreatedDate==DateTime.Now.Date)
                {
                    objEditExerciseResponseModel.IsActual = "True";
                }
                else
                {
                    objEditExerciseResponseModel.IsActual = "False";
                }


                objEditExerciseResponseModel.Response.editExercise = _objFriendFitDBEntity.Database.SqlQuery<EditExercise>("Editexercise @WorkOutId=@WorkOutId,@UserId=@UserId",
                    new SqlParameter("WorkOutId", WorkOutId),
                    new SqlParameter("UserId", userId)).ToList();


                foreach (var item in objEditExerciseResponseModel.Response.editExercise)
                {
                    if (item.ExerciseTypeId == 1)
                    {
                        List<WeightModelView> listOfWeight = _objFriendFitDBEntity.Database.SqlQuery<WeightModelView>("Select Id,TotalWeight,TotalRaps, ImperialType from WeightExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.weightList = listOfWeight;
                    }
                    else if (item.ExerciseTypeId == 2)
                    {
                        List<LevelModelView> listOfLevel = _objFriendFitDBEntity.Database.SqlQuery<LevelModelView>("Select Id,TotalWeight,TotalRaps, ImperialType from LevelExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.levelList = listOfLevel;
                    }
                    else if (item.ExerciseTypeId == 3)
                    {
                        List<TimedModelView> listOfTime = _objFriendFitDBEntity.Database.SqlQuery<TimedModelView>("select Id,TimedSet from TimedExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.timeList = listOfTime;
                    }
                    else if (item.ExerciseTypeId == 4)
                    {
                        List<RapsModelView> listOfRaps = _objFriendFitDBEntity.Database.SqlQuery<RapsModelView>("select Id,RepsSets from RepsExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.respList = listOfRaps;
                    }
                    else if (item.ExerciseTypeId == 5)
                    {
                        DistanceModelView exOfDistance = _objFriendFitDBEntity.Database.SqlQuery<DistanceModelView>("select Id,RepsSetsTime from DistanceExercise where ExerciseSetId={0}", item.ExerciseSetId).FirstOrDefault();
                        item.distance = exOfDistance;
                    }
                    else if (item.ExerciseTypeId == 6)
                    {
                        TextModelView exOfText = _objFriendFitDBEntity.Database.SqlQuery<TextModelView>("select Id,Text from TextExercise where ExerciseId={0}", item.ExerciseId).FirstOrDefault();
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
                    var Last_ActualWeightExercise_ExcerciseId = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == var_ActualWeightExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualWeightExercise_ExcerciseId != null)
                    {
                        ActualWeightExercisesCollection = _objFriendFitDBEntity.ActualWeightExercises.Where(x => x.ActualExerciseSetId == Last_ActualWeightExercise_ExcerciseId.Id).ToList();
                    }
                }

                if (var_ActualLevelExercise != null)
                {
                    var Last_ActualLevelExercise_ExcerciseId = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == var_ActualLevelExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualLevelExercise_ExcerciseId != null)
                    {
                        ActualLevelExercisesCollection = _objFriendFitDBEntity.ActualLevelExercises.Where(x => x.ActualExerciseSetId == Last_ActualLevelExercise_ExcerciseId.Id).ToList();
                    }
                }

                if (var_ActualRepsExercise != null)
                {
                    var Last_ActualRepsExercise_ExcerciseId = _objFriendFitDBEntity.ActualExcerciseSets.OrderByDescending(x => x.CreatedDate).Where(x => x.ExcerciseId == var_ActualRepsExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualRepsExercise_ExcerciseId != null)
                    {
                        ActualRepsExercisesCollection = _objFriendFitDBEntity.ActualRepsExercises.Where(x => x.ActualExerciseSetId == Last_ActualRepsExercise_ExcerciseId.Id).ToList();
                    }
                }

                if (var_ActualDistanceExercise != null)
                {
                    var Last_ActualDistanceExercise_ExcerciseId = _objFriendFitDBEntity.ActualExcerciseSets.OrderByDescending(x => x.CreatedDate).Where(x => x.ExcerciseId == var_ActualDistanceExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualDistanceExercise_ExcerciseId != null)
                    {
                        ActualDistanceExercisesCollection = _objFriendFitDBEntity.ActualDistanceExercises.Where(x => x.ActualExerciseSetId == Last_ActualDistanceExercise_ExcerciseId.Id).ToList();
                    }
                }

                if (var_ActualFreeTextExercise != null)
                {
                    var Last_ActualFreeTextExercise_ExcerciseId = _objFriendFitDBEntity.ActualExcerciseSets.OrderByDescending(x => x.CreatedBy).Where(x => x.ExcerciseId == var_ActualFreeTextExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualFreeTextExercise_ExcerciseId != null)
                    {
                        ActualTextExercisesCollection = _objFriendFitDBEntity.ActualFreeTextExercises.OrderByDescending(x => x.CreatedDate).Where(x => x.ExerciseId == Last_ActualFreeTextExercise_ExcerciseId.ExcerciseId).Take(1).ToList();
                    }
                }

                if (var_ActualTimedExercise != null)
                {
                    var Last_ActualTimedExercise_ExcerciseId = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == var_ActualTimedExercise.ExerciseId).FirstOrDefault();
                    if (Last_ActualTimedExercise_ExcerciseId != null)
                    {
                        ActualTimeExercisesCollection = _objFriendFitDBEntity.ActualTimedExercises.Where(x => x.ActualExerciseSetId == Last_ActualTimedExercise_ExcerciseId.Id).ToList();
                    }
                }

                foreach (var item in objEditExerciseResponseModel.Response.editExercise)
                {

                    if (item.ExerciseTypeId == 1)
                    {
                        var ActualExcerciseSetCollection = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (ActualExcerciseSetCollection != null)
                        {
                            //for actual Weight exercise data
                            //foreach (var item1 in ActualExcerciseSetCollection)
                            //{                 
                            item.SetsNumberActual = ActualExcerciseSetCollection.SetsNumber;
                            // var ActualWeightExercisesCollection = _objFriendFitDBEntity.ActualWeightExercises.ToList().Where(x => x.ActualExerciseSetId == item1.Id & x.Createdate.Value.Date == latestdate.Createdate).OrderByDescending(x => x.Createdate).ToList();
                            foreach (var item2 in ActualWeightExercisesCollection)
                            {
                                ActualWeightModelView model = new ActualWeightModelView();
                                model.TotalRaps = item2.TotalRaps;
                                model.ImperialType = item2.ImperialType;
                                model.TotalWeight = Convert.ToInt64(item2.TotalWeight);
                                model.Id = item2.Id;
                                item.actualWeightModel.Add(model);
                            }
                            //    }
                        }
                    }

                    if (item.ExerciseTypeId == 2)
                    {
                        var ActualExcerciseSetCollection1 = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        if (ActualExcerciseSetCollection1 != null)
                        {
                            //for actual Level exercise data
                            //      foreach (var item1 in ActualExcerciseSetCollection1)
                            //     {

                            item.SetsNumberActual = ActualExcerciseSetCollection1.SetsNumber;
                            //  var ActualLevelExercisesCollection = _objFriendFitDBEntity.ActualLevelExercises.ToList().Where(x => x.ActualExerciseSetId == item1.Id).ToList();
                            foreach (var item3 in ActualLevelExercisesCollection)
                            {
                                ActualLevelModel1 model1 = new ActualLevelModel1();
                                model1.TotalRaps = item3.TotalRaps;
                                model1.ImperialType = item3.ImperialType;
                                model1.TotalWeight = Convert.ToInt64(item3.TotalWeight);
                                item.actualLevelList.Add(model1);
                            }

                            //     }
                        }
                    }


                    if (item.ExerciseTypeId == 3)
                    {
                        var ActualExcerciseSetCollection2 = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        //for actual Time exercise data
                        //foreach (var item1 in ActualExcerciseSetCollection2)
                        //{
                        if (ActualExcerciseSetCollection2 != null)
                        {
                            item.SetsNumberActual = ActualExcerciseSetCollection2.SetsNumber;
                            // var ActualTimeExercisesCollection = _objFriendFitDBEntity.ActualTimedExercises.ToList().Where(x => x.ActualExerciseSetId == item1.Id).ToList();
                            foreach (var item3 in ActualTimeExercisesCollection)
                            {
                                ActualTimedModel1 model2 = new ActualTimedModel1();
                                model2.TimedSet = item3.TimedSet;
                                item.actualTimeList.Add(model2);
                            }
                        }
                        //   }
                    }

                    if (item.ExerciseTypeId == 4)
                    {
                        var ActualExcerciseSetCollection3 = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        //for actual Reps exercise data
                        //foreach (var item1 in ActualExcerciseSetCollection3)
                        //{
                        if (ActualExcerciseSetCollection3 != null)
                        {
                            item.SetsNumberActual = ActualExcerciseSetCollection3.SetsNumber;
                            //  var ActualRepsExercisesCollection = _objFriendFitDBEntity.ActualRepsExercises.ToList().Where(x => x.ActualExerciseSetId == item1.Id).ToList();
                            foreach (var item4 in ActualRepsExercisesCollection)
                            {
                                ActualRapsModel1 model3 = new ActualRapsModel1();
                                model3.Id = item4.Id;
                                model3.RepsSets = item4.RepsSets;
                                item.actualRapsList.Add(model3);
                            }
                        }
                        //}
                    }

                    if (item.ExerciseTypeId == 5)
                    {
                        var ActualExcerciseSetCollection4 = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        //for actual Distance exercise data
                        //foreach (var item1 in ActualExcerciseSetCollection4)
                        //{
                        if (ActualExcerciseSetCollection4 != null)
                        {
                            item.ActualDistanceInKm = ActualExcerciseSetCollection4.DistanceInKm;
                            item.SetsNumberActual = ActualExcerciseSetCollection4.SetsNumber;
                            item.ImperialType = ActualExcerciseSetCollection4.ImperialType;
                            //  var ActualDistanceExercisesCollection = _objFriendFitDBEntity.ActualDistanceExercises.ToList().Where(x => x.ActualExerciseSetId == item1.Id).ToList();
                            foreach (var item5 in ActualDistanceExercisesCollection)
                            {
                                ActualDistanceModel1 model4 = new ActualDistanceModel1();
                                // model4 = _objFriendFitDBEntity.Database.SqlQuery<ActualDistanceModel1>("select Id,RepsSetsTime from ActualDistanceExercise where ActualExerciseSetId={0}", item5.ActualExerciseSetId).FirstOrDefault();
                                model4.RepsSetsTime = item5.RepsSetsTime;
                                // item.ActualDistanceInKm = model4.RepsSetsTime;
                                model4.Id = item5.Id;
                                item.actualDistance.Add(model4);
                            }
                        }
                        //}
                    }


                    if (item.ExerciseTypeId == 6)
                    {
                        //var ActualExcerciseSetCollection5 = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        var ActualExcerciseSetCollection5 = _objFriendFitDBEntity.ActualFreeTextExercises.Where(x => x.ExerciseId == item.ExerciseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                        //for actual Text exercise data
                        //foreach (var item1 in ActualExcerciseSetCollection5)
                        //{

                        if (ActualExcerciseSetCollection5 != null)
                        {
                            //item.SetsNumberActual = ActualExcerciseSetCollection5.SetsNumber;
                            item.SetsNumberActual = 0;

                            // var ActualTextExercisesCollection = _objFriendFitDBEntity.ActualFreeTextExercises.ToList().Where(x => x.ExerciseId == item1.Id).ToList();

                            foreach (var item6 in ActualTextExercisesCollection)
                            {
                                ActualTextModel1 model5 = new ActualTextModel1();
                                // model5 = _objFriendFitDBEntity.Database.SqlQuery<ActualTextModel1>("select Id,Text from ActualFreeTextExercise where ExerciseId={0}", item.ExerciseId).FirstOrDefault();
                                model5.Id = item6.Id;
                                model5.Text = item6.Text;
                                item.actualText.Add(model5);
                                // item.actualText = model5.Text;                  
                            }
                        }
                        //  }
                    }
                }
                return objEditExerciseResponseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //for adding actual exercise

        //for adding actual exercise
        public int UpdateExercise(UpdateActualExerciseRequest objUpdateActualExerciseRequest)
        {
            try
            {


                foreach (var item in objUpdateActualExerciseRequest.UpdateList)
                {

                    int rowEffectedUpdate = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Update Exercise set ExerciseName=@ExerciseName where Id=@Id and UserId=@UserId",
                                                                                         new SqlParameter("ExerciseName", item.ExerciseName),
                                                                                         new SqlParameter("Id", item.ExcerciseId),
                                                                                         new SqlParameter("UserId", objUpdateActualExerciseRequest.UserId));

                    int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualExercise @ExcerciseId=@ExcerciseId,@SetsNumber=@SetsNumber,@DistanceInKm=@DistanceInKm, @ImperialType=@ImperialType",
                                                                                     new SqlParameter("ExcerciseId", item.ExcerciseId),
                                                                                     new SqlParameter("SetsNumber", (Object)item.SetsNumber ?? DBNull.Value),
                                                                                     new SqlParameter("ImperialType", item.ImperialType),
                                                                                     new SqlParameter("DistanceInKm", (Object)item.DistanceInKm ?? DBNull.Value));


                    //Int64 ExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ExcerciseSets ORDER BY ID DESC").FirstOrDefault();
                    Int64 ActualExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ActualExcerciseSets ORDER BY ID DESC").FirstOrDefault();
                    if (item.ExerciseTypeId == 1)
                    {
                        if (item.weightList.Count > 0)
                        {
                            foreach (var item1 in item.weightList)
                            {
                                int roweffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@ImperialType=@ImperialType, @Generated_ActualExerciseSet_Id=@Generated_ActualExerciseSet_Id",
                                                   new SqlParameter("TotalWeight", item1.TotalWeight),
                                                   new SqlParameter("TotalRaps", item1.TotalRaps),
                                                   new SqlParameter("ImperialType", item1.ImperialType),
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
                                int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualLevelExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@ActualExerciseSetId=@ActualExerciseSetId,@ImperialType=@ImperialType",
                                                                                             new SqlParameter("TotalWeight", item2.TotalWeight),
                                                                                             new SqlParameter("TotalRaps", item2.TotalRaps),
                                                                                              new SqlParameter("ImperialType", item2.ImperialType),
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
                                int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualTimedExercise @ActualExerciseSetId=@ActualExerciseSetId,@TimedSet=@TimedSet",
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
                                int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualRapsExercise @ActualExerciseSetId=@ActualExerciseSetId,@RepsSets=@RepsSets",
                                                                                             new SqlParameter("ActualExerciseSetId", ActualExerciseSetId),
                                                                                             new SqlParameter("RepsSets", item4.RepsSets));

                            }
                        }
                    }

                    else if (item.ExerciseTypeId == 5)
                    {
                        if (item.disList != null)
                        {
                            foreach (var item10 in item.disList)
                            {
                                int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualDistanceExercise @RepsSetsTime=@RepsSetsTime,@ActualExerciseSetId=@ActualExerciseSetId",
                                                                                         new SqlParameter("RepsSetsTime", item10.RepsSetsTime),
                                                                                         new SqlParameter("ActualExerciseSetId", ActualExerciseSetId));

                            }
                        }
                    }
                    else if (item.ExerciseTypeId == 6)
                    {
                        if (item.textListOfActual != null)
                        {
                            foreach (var item6 in item.textListOfActual)
                            {
                                int rowsInserted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualFreeTextExercise @Text=@Text,@ExerciseId=@ExerciseId",
                                                                                           new SqlParameter("ExerciseId", item.ExcerciseId),
                                                                                           new SqlParameter("Text", item6.Text));
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


        public int UpdateActualExercise(Int64 UserId, Int64 ExerciseId, UpdatingActExistingRequest objReq)
        {
            try
            {
                Int64 ActualExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("Select Id from ActualExcerciseSets where ExcerciseId={0}", ExerciseId).FirstOrDefault();
                List<UpdateActualWeightList> ActualWeightExerciseId = _objFriendFitDBEntity.Database.SqlQuery<UpdateActualWeightList>("Select Id from ActualWeightExercise where ActualExerciseSetId={0}", ActualExerciseSetId).ToList();

                //deleting Exercise in table Actual Exercise and Actual Weight
                foreach (var item in ActualWeightExerciseId)
                {
                    int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Delete from ActualWeightExercise where Id=@Id",
                                                                                       new SqlParameter("Id", item.Id));

                }

                int rowEffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Delete from ActualExcerciseSets where ExcerciseId={0}", ExerciseId);


                ////updating 3 table of Exercise
                int rowEffectedUpdate = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Update Exercise set ExerciseName=@ExerciseName where Id=@Id",
                                                                                          new SqlParameter("ExerciseName", objReq.ExerciseName),
                                                                                          new SqlParameter("Id", ExerciseId));

                int rowEffected2 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualExercise @ExcerciseId=@ExcerciseId,@SetsNumber=@SetsNumber,@UserId=@UserId",
                                                                                      new SqlParameter("ExcerciseId", ExerciseId),
                                                                                      new SqlParameter("SetsNumber", objReq.SetsNumber),
                                                                                      new SqlParameter("UserId", UserId));

                Int64 UpdatedActualExerciseSetId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM ActualExcerciseSets ORDER BY ID DESC").FirstOrDefault();

                foreach (var it in objReq.updateWeightList)
                {

                    int roweffected1 = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddActualWeightExercise @TotalWeight=@TotalWeight,@TotalRaps=@TotalRaps,@Generated_ActualExerciseSet_Id=@Generated_ActualExerciseSet_Id",
                                   new SqlParameter("TotalWeight", it.TotalWeight),
                                   new SqlParameter("TotalRaps", it.TotalRaps),
                                   new SqlParameter("Generated_ActualExerciseSet_Id", UpdatedActualExerciseSetId));

                }

            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }


        //this is for exercise details
        public all_EditExerciseResponseModel AllPreviouseExerciseDetailsByWorkOutId(Int64 WorkOutId, Int64 userId)
        {
            all_EditExerciseResponseModel objEditExerciseResponseModel = new all_EditExerciseResponseModel();
            try
            {
                var GetWorkOutCreatedDate = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == WorkOutId).FirstOrDefault().Createdate.Value.Date;
                objEditExerciseResponseModel.IsActual = Convert.ToString(_objFriendFitDBEntity.Exercises.Where(x => x.WorkOutId == WorkOutId).FirstOrDefault().IsActive);

                objEditExerciseResponseModel.Response.all_editExercise = _objFriendFitDBEntity.Database.SqlQuery<all_EditExercise>("Editexercise @WorkOutId=@WorkOutId,@UserId=@UserId",
                    new SqlParameter("WorkOutId", WorkOutId),
                    new SqlParameter("UserId", userId)).ToList();


                foreach (var item in objEditExerciseResponseModel.Response.all_editExercise)
                {
                    if (item.ExerciseTypeId == 1)
                    {
                        List<WeightModelView> listOfWeight = _objFriendFitDBEntity.Database.SqlQuery<WeightModelView>("Select Id,TotalWeight,TotalRaps from WeightExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.weightList = listOfWeight;
                    }
                    else if (item.ExerciseTypeId == 2)
                    {
                        List<LevelModelView> listOfLevel = _objFriendFitDBEntity.Database.SqlQuery<LevelModelView>("Select Id,TotalWeight,TotalRaps from LevelExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.levelList = listOfLevel;
                    }
                    else if (item.ExerciseTypeId == 3)
                    {
                        List<TimedModelView> listOfTime = _objFriendFitDBEntity.Database.SqlQuery<TimedModelView>("select Id,TimedSet from TimedExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.timeList = listOfTime;
                    }
                    else if (item.ExerciseTypeId == 4)
                    {
                        List<RapsModelView> listOfRaps = _objFriendFitDBEntity.Database.SqlQuery<RapsModelView>("select Id,RepsSets from RepsExercise where ExerciseSetId={0}", item.ExerciseSetId).ToList();
                        item.respList = listOfRaps;
                    }
                    else if (item.ExerciseTypeId == 5)
                    {
                        DistanceModelView exOfDistance = _objFriendFitDBEntity.Database.SqlQuery<DistanceModelView>("select Id,RepsSetsTime from DistanceExercise where ExerciseSetId={0}", item.ExerciseSetId).FirstOrDefault();
                        item.distance = exOfDistance;
                    }
                    else if (item.ExerciseTypeId == 6)
                    {
                        TextModelView exOfText = _objFriendFitDBEntity.Database.SqlQuery<TextModelView>("select Id,Text from TextExercise where ExerciseId={0}", item.ExerciseId).FirstOrDefault();
                        item.text = exOfText;
                    }
                }


                foreach (var item in objEditExerciseResponseModel.Response.all_editExercise)
                {
                    if (item.ExerciseTypeId == 1)
                    {
                        var AllDateOf_AcutalExerciseSets = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        foreach (var item_Date in AllDateOf_AcutalExerciseSets)
                        {
                            All_ActualDateExcerciseSet List_AllActualDate = new All_ActualDateExcerciseSet();
                            List_AllActualDate.ExcerciseDate = item_Date.CreatedDate;
                            List_AllActualDate.ExcerciseId = item_Date.Id;
                            List_AllActualDate.SetNum = item_Date.SetsNumber;
                            item._ActualDateExcerciseSet.Add(List_AllActualDate);
                        }

                        var ActualExcerciseSetCollection = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        foreach (var All_Reps_item in ActualExcerciseSetCollection)
                        {
                            var ActualExcerciseSetCollection3 = _objFriendFitDBEntity.ActualWeightExercises.Where(x => x.ActualExerciseSetId == All_Reps_item.Id).ToList();
                            //for actual Reps exercise data
                            foreach (var item1 in ActualExcerciseSetCollection3)
                            {
                                if (ActualExcerciseSetCollection3 != null)
                                {
                                    all_ActualWeightModelView model3 = new all_ActualWeightModelView();
                                    model3.TotalRaps = item1.TotalRaps;
                                    model3.TotalWeight = item1.TotalWeight;
                                    model3.Id = item1.Id;
                                    model3.CreatedDate = item1.Createdate;
                                    model3.SetsNumberActual = Convert.ToString(All_Reps_item.SetsNumber);
                                    model3.ActualExcerciseId = All_Reps_item.Id;
                                    item.actualWeightModel.Add(model3);
                                }
                            }
                        }                      
                    }

                    if (item.ExerciseTypeId == 2)
                    {                     
                        var AllDateOf_AcutalExerciseSets = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        foreach (var item_Date in AllDateOf_AcutalExerciseSets)
                        {
                            All_ActualDateExcerciseSet List_AllActualDate = new All_ActualDateExcerciseSet();
                            List_AllActualDate.ExcerciseDate = item_Date.CreatedDate;
                            List_AllActualDate.ExcerciseId = item_Date.Id;
                            List_AllActualDate.SetNum = item_Date.SetsNumber;
                            item._ActualDateExcerciseSet.Add(List_AllActualDate);
                        }

                        var ActualExcerciseSetCollection1 = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        foreach (var All_Reps_item in ActualExcerciseSetCollection1)
                        {
                            var ActualExcerciseSetCollection3 = _objFriendFitDBEntity.ActualLevelExercises.Where(x => x.ActualExerciseSetId == All_Reps_item.Id).ToList();
                            //for actual Reps exercise data
                            foreach (var item3 in ActualExcerciseSetCollection3)
                            {
                                if (ActualExcerciseSetCollection3 != null)
                                {
                                    all_ActualLevelModel1 model1 = new all_ActualLevelModel1();
                                    model1.TotalRaps = item3.TotalRaps;
                                    model1.TotalWeight = item3.TotalWeight;
                                    model1.CreatedDate = item3.Createdate;
                                    model1.SetsNumberActual = Convert.ToString(All_Reps_item.SetsNumber);
                                    model1.ActualExcerciseId = All_Reps_item.Id;
                                    item.actualLevelList.Add(model1);
                                }
                            }
                        }
                       
                    }
                    
                    if (item.ExerciseTypeId == 3)
                    {
                        var AllDateOf_AcutalExerciseSets = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        foreach (var item_Date in AllDateOf_AcutalExerciseSets)
                        {
                            All_ActualDateExcerciseSet List_AllActualDate = new All_ActualDateExcerciseSet();
                            List_AllActualDate.ExcerciseDate = item_Date.CreatedDate;
                            List_AllActualDate.ExcerciseId = item_Date.Id;
                            List_AllActualDate.SetNum = item_Date.SetsNumber;
                            item._ActualDateExcerciseSet.Add(List_AllActualDate);
                        }

                        var ActualExcerciseSetCollection2 = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();

                        foreach (var All_Reps_item in ActualExcerciseSetCollection2)
                        {
                            var ActualExcerciseSetCollection3 = _objFriendFitDBEntity.ActualTimedExercises.Where(x => x.ActualExerciseSetId == All_Reps_item.Id).ToList();
                            //for actual Reps exercise data
                            foreach (var item3 in ActualExcerciseSetCollection3)
                            {
                                if (ActualExcerciseSetCollection3 != null)
                                {                                   
                                    all_ActualTimedModel1 model2 = new all_ActualTimedModel1();
                                    model2.TimedSet = item3.TimedSet;
                                    model2.SetsNumberActual = Convert.ToString(All_Reps_item.SetsNumber);
                                    model2.CreatedDate = item3.Createdate;
                                    model2.Id = item3.Id;
                                    model2.ActualExcerciseId = All_Reps_item.Id;
                                    item.actualTimeList.Add(model2);

                                }
                            }
                        }

                    }                   

                    if (item.ExerciseTypeId == 4)
                    {
                        var AllDateOf_AcutalExerciseSets = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        foreach (var item_Date in AllDateOf_AcutalExerciseSets)
                        {
                            All_ActualDateExcerciseSet List_AllActualDate = new All_ActualDateExcerciseSet();
                            List_AllActualDate.ExcerciseDate = item_Date.CreatedDate;
                            List_AllActualDate.ExcerciseId = item_Date.Id;
                            List_AllActualDate.SetNum = item_Date.SetsNumber;
                            item._ActualDateExcerciseSet.Add(List_AllActualDate);
                        }

                        var Last_ActualRepsExercise_ExcerciseId = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        foreach (var All_Reps_item in Last_ActualRepsExercise_ExcerciseId)
                        {
                            var ActualExcerciseSetCollection3 = _objFriendFitDBEntity.ActualRepsExercises.Where(x => x.ActualExerciseSetId == All_Reps_item.Id).ToList();
                            //for actual Reps exercise data
                            foreach (var item1 in ActualExcerciseSetCollection3)
                            {
                                if (ActualExcerciseSetCollection3 != null)
                                {
                                    all_ActualRapsModel1 model3 = new all_ActualRapsModel1();
                                    model3.SetsNumberActual = Convert.ToString(All_Reps_item.SetsNumber);
                                    model3.Id = item1.Id;
                                    model3.RepsSets = item1.RepsSets;
                                    model3.CreatedDate = item1.Createdate;
                                    model3.ActualExcerciseId = All_Reps_item.Id;
                                    item.actualRapsList.Add(model3);                              
                                }
                            }
                        }
                    }

                    if (item.ExerciseTypeId == 5)
                    {
                        var AllDateOf_AcutalExerciseSets = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        foreach (var item_Date in AllDateOf_AcutalExerciseSets)
                        {
                            All_ActualDateExcerciseSet List_AllActualDate = new All_ActualDateExcerciseSet();
                            List_AllActualDate.ExcerciseDate = item_Date.CreatedDate;
                            List_AllActualDate.ExcerciseId = item_Date.Id;
                            List_AllActualDate.SetNum = item_Date.SetsNumber;
                            item._ActualDateExcerciseSet.Add(List_AllActualDate);
                        }

                        var ActualExcerciseSetCollection4 = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();

                        foreach (var All_Reps_item in ActualExcerciseSetCollection4)
                        {
                            var ActualExcerciseSetCollection3 = _objFriendFitDBEntity.ActualDistanceExercises.Where(x => x.ActualExerciseSetId == All_Reps_item.Id).ToList();

                            foreach (var item1 in ActualExcerciseSetCollection3)
                            {
                                if (ActualExcerciseSetCollection3 != null)
                                {
                                    all_ActualDistanceModel1 model4 = new all_ActualDistanceModel1();
                                    model4.Id = item1.Id;
                                    model4.RepsSetsTime = item1.RepsSetsTime;
                                    model4.Km = Convert.ToString(All_Reps_item.DistanceInKm);
                                    model4.CreatedDate = item1.Createdate;
                                    model4.ActualExcerciseId = All_Reps_item.Id;
                                    item.actualDistance.Add(model4);
                                }
                            }
                        }
                    }
                    
                    if (item.ExerciseTypeId == 6)
                    {
                        //var AllDateOf_AcutalExerciseSets = _objFriendFitDBEntity.ActualExcerciseSets.Where(x => x.ExcerciseId == item.ExerciseId).ToList();
                        var AllDateOf_AcutalExerciseSets = _objFriendFitDBEntity.ActualFreeTextExercises.ToList().Where(x => x.ExerciseId == item.ExerciseId).ToList();
                        foreach (var item_Date in AllDateOf_AcutalExerciseSets)
                        {
                            All_ActualDateExcerciseSet List_AllActualDate = new All_ActualDateExcerciseSet();
                            List_AllActualDate.ExcerciseDate = item_Date.CreatedDate;
                            List_AllActualDate.ExcerciseId = item_Date.Id;
                            //List_AllActualDate.SetNum = item_Date.SetsNumber;
                            item._ActualDateExcerciseSet.Add(List_AllActualDate);
                        }
                                               
                        var ActualTextExercisesCollection = _objFriendFitDBEntity.ActualFreeTextExercises.ToList().Where(x => x.ExerciseId == item.ExerciseId).ToList();

                            foreach (var item1 in ActualTextExercisesCollection)
                            {
                                if (ActualTextExercisesCollection != null)
                                {
                                    all_ActualTextModel1 model5 = new all_ActualTextModel1();
                                    model5.Id = item1.Id;
                                    model5.Text = item1.Text;
                                    model5.CreatedDate = item1.CreatedDate;
                                   model5.ActualExcerciseId = item1.Id;
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

    }
}
