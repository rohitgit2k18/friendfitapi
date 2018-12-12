using FriendFit.API.Filters;
using FriendFit.Data;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.IRepository;
using FriendFit.Data.Repository;
using FriendFit.Entity.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/Workout")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WorkoutController : ApiController
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        private IWorkoutRepository _objIWorkoutRepository;
        private HttpResponseMessage _response;

        public WorkoutController()
        {
            _objIWorkoutRepository = new WorkoutRepository();
        }

        [HttpPost]
        [Route("AddWorkout")]
        [SecureResource]
        public HttpResponseMessage AddWorkout(AddWorkoutModelRequest objAddWorkoutModelRequest)
        {
            AddWorkoutResponseModel result = new AddWorkoutResponseModel();
            if (ModelState.IsValid)
            {
                try
                {
                    var headers = Request.Headers;
                    string token = headers.Authorization.Parameter.ToString();
                    Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();
                    
                  int value=  _objIWorkoutRepository.AddWorkout(objAddWorkoutModelRequest,UserId);
                  Int64 WorkoutId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("SELECT TOP 1 Id FROM WorkOut ORDER BY id DESC").FirstOrDefault();
                    if(value>0)
                    {
                        result.WorkoutId = WorkoutId;
                        result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Message = "Workout added successfully!";
                    }
                    else
                    {
                        result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                        result.Message = "Parameters are not correct";
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
                _response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                ModelState.AddModelError("", "One or more errors occurred.");
            }
            return _response;
        }


        [HttpPost]
        [Route("ListOfWorkouts")]
        [SecureResource]
        public HttpResponseMessage ListOfWorkout(ListOfWorkoutRequestModel objListOfWorkoutRequestModel)
        {
            WorkoutListModelResponse result = new WorkoutListModelResponse();
            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

              result.Response.workoutlist= _objIWorkoutRepository.WorkoutList(objListOfWorkoutRequestModel).ToList();
              if(result.Response.workoutlist.Count>0)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Success!!";
                }
               else
                {
                    result.Response.StatusCode= Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Response.Message = "No Records";
                }
            }
            catch(Exception ex)
            {
                res.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpPost]
        [Route("WorkoutDetails")]
        [SecureResource]
        public HttpResponseMessage WorkoutDetails(WorkoutDetailsRequestModel objWorkoutDetailsRequestModel)
        {
            WorkoutDetailsModelResponse result = new WorkoutDetailsModelResponse();
            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result.Response = _objIWorkoutRepository.Workoutdetails(objWorkoutDetailsRequestModel);
                if (result.Response!=null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Success!!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Response.Message = "No Records";
                }

            }
            catch (Exception ex)
            {
                res.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpPost]
        [Route("UpdateWorkoutByUserId")]
        [SecureResource]
        public HttpResponseMessage UpdateWorkoutByUserId(UpdateWorkoutRequestModel objUpdateWorkoutRequestModel)
        {
            
            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                int updateModel = _objIWorkoutRepository.UpdateWorkoutDetailsById(objUpdateWorkoutRequestModel, UserId);
                if(updateModel>0)
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Success!!";
                }
                else
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                    result.Message = "Parameters are not correct";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpPost]
        [Route("StatusUpdateCompleted")]
        [SecureResource]
        public HttpResponseMessage StatusUpdateCompleted(Int64 WorkoutId)
        {
            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                int StatusCompleted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("update Workout set StatusId=1 where Id={0} and UserId={1}", WorkoutId,UserId);
                int FinishTimeUpdate = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Update Workout set FinishTime=@FinishTime where Id=@Id and UserId=@UserId",
                                                                                          new SqlParameter("FinishTime", System.DateTime.Now),
                                                                                          new SqlParameter("Id", WorkoutId),
                                                                                          new SqlParameter("UserId", UserId));
                if(StatusCompleted>0)
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Completed";
                }
                else
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                    result.Message = "This workout id is not in database";
                }
            }
            catch(Exception ex)
            {

            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpPost]
        [Route("StatusInProgress")]
        [SecureResource]
        public HttpResponseMessage StatusInProgress(Int64 WorkoutId)
        {
            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                int StatusCompleted = _objFriendFitDBEntity.Database.ExecuteSqlCommand("update Workout set StatusId=3, Actual_StartTime={2} where Id={0} and UserId={1}", WorkoutId,UserId, DateTime.Now);
                if (StatusCompleted > 0)
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "In Progress";
                    result.WorkoutId = WorkoutId;
                }
                else
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                    result.Message = "This workout id is not in database";
                }
            }
            catch (Exception ex)
            {
               
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpPost]
        [Route("PreviousExercise")]
        [SecureResource]
        public HttpResponseMessage PreviousExercseByUserId(Int64 WorkOutId)
        {
            PreviousExerciseModelResponse result = new PreviousExerciseModelResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();
                result.Response = _objIWorkoutRepository.PreviousWorkOut(WorkOutId,UserId);
            }
            catch (Exception ex)
            {

            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpPost]
        [Route("ListOfWorkoutsForMobile")]
        [SecureResource]
        public HttpResponseMessage ListOfWorkoutsForMobile(ListOfWorkoutRequestModel objListOfWorkoutRequestModel, string Search)
        {
            WorkoutListModelResponse result = new WorkoutListModelResponse();
            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result.Response.workoutlist = _objIWorkoutRepository.WorkoutListForMobile(objListOfWorkoutRequestModel, Search).ToList();
                if (result.Response.workoutlist.Count > 0)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Success!!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Response.Message = "No Records";
                }
            }
            catch (Exception ex)
            {
                res.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

    }
}
