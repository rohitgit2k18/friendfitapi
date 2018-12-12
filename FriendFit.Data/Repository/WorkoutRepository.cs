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
    public class WorkoutRepository: IWorkoutRepository
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();     

        public int AddWorkout(AddWorkoutModelRequest objAddWorkoutModelRequest,Int64 UserId)
        {
            WorkOut objWork = new WorkOut()
            {
                UserId = objAddWorkoutModelRequest.UserId,
                Description = objAddWorkoutModelRequest.Description,
                DateOfWorkout = objAddWorkoutModelRequest.DateOfWorkout,
                StartTime = objAddWorkoutModelRequest.StartTime,
                FinishTime = objAddWorkoutModelRequest.FinishTime,
                WorkoutNotes = objAddWorkoutModelRequest.WorkoutNotes,
                Createdate = System.DateTime.Now,
                CreatedBy= UserId,
                IsActive= true,
                RowStatus= true,
                StatusId=2,
                AutoFinishTime=objAddWorkoutModelRequest.AutoFinishTime,
                ScheduleWorkout=false
                              
            };
            _objFriendFitDBEntity.WorkOuts.Add(objWork);
            _objFriendFitDBEntity.SaveChanges();
            return 1;
        }
        public List<WorkoutList> WorkoutList(ListOfWorkoutRequestModel objListOfWorkoutRequestModel)
        {
            List<WorkoutList> model = new List<ApiModel.APIResponseModel.WorkoutList>();
            try
            {
                model = _objFriendFitDBEntity.Database.SqlQuery<WorkoutList>("WorkoutList @UserId=@UserId",
                                                            new SqlParameter("UserId", objListOfWorkoutRequestModel.UserId)).ToList();               
            }
            catch(Exception ex)
            {                               
            }
            return model;
        }

        public WorkoutDetails Workoutdetails(WorkoutDetailsRequestModel objWorkoutDetailsRequestModel)
        {
            try
            {
                WorkoutDetails model = _objFriendFitDBEntity.Database.SqlQuery<WorkoutDetails>("WorkoutDetailsById @WorkoutId=@WorkoutId",
                                                                                                 new SqlParameter("WorkoutId", objWorkoutDetailsRequestModel.WorkoutId)).FirstOrDefault();
                return model;
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

        public int UpdateWorkoutDetailsById(UpdateWorkoutRequestModel objUpdateWorkoutRequestModel,Int64 UserId)
        {
            try
            {
                int updateWorDetails = _objFriendFitDBEntity.Database.ExecuteSqlCommand("UpdateWorkoutDetails @Description=@Description,@DateOfWorkout=@DateOfWorkout,@StartTime=@StartTime,@FinishTime=@FinishTime,@WorkoutNotes=@WorkoutNotes,@UserId=@UserId,@WorkOutId=@WorkOutId",
                                                                                        new SqlParameter("Description", objUpdateWorkoutRequestModel.Description),
                                                                                        new SqlParameter("DateOfWorkout", objUpdateWorkoutRequestModel.DateOfWorkout),
                                                                                        new SqlParameter("StartTime", objUpdateWorkoutRequestModel.StartTime),
                                                                                        new SqlParameter("FinishTime", objUpdateWorkoutRequestModel.FinishTime),
                                                                                        new SqlParameter("WorkoutNotes", objUpdateWorkoutRequestModel.WorkoutNotes),
                                                                                        new SqlParameter("UserId", UserId),
                                                                                        new SqlParameter("WorkOutId", objUpdateWorkoutRequestModel.WorkOutId));
            }
            catch(Exception ex)
            {
                
            }
            return 1;
        }

        public PreviousExercise PreviousWorkOut(Int64 WorkoutId,Int64 UserId)
        {
            try
            {

                PreviousExerciseModelResponse response = new PreviousExerciseModelResponse();
                var mailList = _objFriendFitDBEntity.Database.SqlQuery<PreviousExercise>("").FirstOrDefault();
            }
            catch(Exception ex)
            {

            }
            return null;
        }

        public List<WorkoutList> WorkoutListForMobile(ListOfWorkoutRequestModel objListOfWorkoutRequestModel, string Search)
        {
            List<WorkoutList> model = new List<ApiModel.APIResponseModel.WorkoutList>();
            try
            {
                model = _objFriendFitDBEntity.Database.SqlQuery<WorkoutList>("WorkoutListForMobile @UserId=@UserId, @Search = @Search",
                                                           new SqlParameter("UserId", objListOfWorkoutRequestModel.UserId),
                                                           new SqlParameter("Search", Search)).ToList();               
            }
            catch (Exception ex)
            {
            }
            return model;
        }
    }
}
