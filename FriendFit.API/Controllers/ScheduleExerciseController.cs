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
    [RoutePrefix("api/ExerciseSchedule")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ScheduleExerciseController : ApiController
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();
        private IScheduleExerciseRepository _objIScheduleExerciseRepository;
        public ScheduleExerciseController()
        {
            _objIScheduleExerciseRepository = new ScheduleExerciseRepository();
        }
        private HttpResponseMessage _response;


        [HttpPost]
        [Route("AddScheduleExercise")]
        [SecureResource]
        public HttpResponseMessage AddScheduleExercise(AddScheduleExerciseRequestModel objAddScheduleExerciseRequestModel)
        {
            FResponse result = new FResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    var headers = Request.Headers;
                    string token = headers.Authorization.Parameter.ToString();
                    Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();
                    int Value = _objIScheduleExerciseRepository.AddScheduleExercise(objAddScheduleExerciseRequestModel);
                    if (Value > 0)
                    {

                        result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Message = "Goal Exercise added successfully!";
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
                
        [HttpGet]
        [Route("EditScheduleExercise")]
        [SecureResource]
        public HttpResponseMessage EditScheduleExercise(Int64 ScheduleWorkOutId)
        {
            EditScheduleExerciseResponseModel result = new EditScheduleExerciseResponseModel();
            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                //result = _objIExerciseRepository.ExerciseDetailsByWorkOutId(WorkOutId, userId);

                ////result.Response.editExercise.weightList = _objIExerciseRepository.WeightExerciseList(result.Response.editExercise.ExerciseSetId).ToList(); ;
                //if (result.Response != null)
                //{
                //    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                //    result.Response.Message = "Success!!";
                //}
                //else
                //{
                //    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                //    result.Response.Message = "No Records";
                //}
            }
            catch (Exception ex)
            {
                res.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpGet]
        [Route("testss")]
        public TimeSpan testss()
        {
            string ats = "18:40:00.0000000";
            DateTime ts = Convert.ToDateTime(ats).ToUniversalTime();
            TimeSpan gettime = ts.TimeOfDay;
            DateTime sd = DateTime.UtcNow;
            return gettime;
        }

        [HttpPost]
        [Route("ListOfSchedule")]
        [SecureResource]
        public HttpResponseMessage ListOfSchedule(ListOfWorkoutRequestModel objListOfWorkoutRequestModel, string Search)
        {
            ScheduleListResponseModel result = new ScheduleListResponseModel();
            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result.Response.scheduleLists = _objIScheduleExerciseRepository.ScheduleList(objListOfWorkoutRequestModel, Search).ToList();
                if (result.Response.scheduleLists.Count > 0)
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


        [HttpGet]
        [Route("ScheduleDetailsById")]
        [SecureResource]    
        public HttpResponseMessage ScheduleDetailsById(Int64 ScheduleId)
        {
            EditExerciseResponseModel result = new EditExerciseResponseModel();
            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result = _objIScheduleExerciseRepository.ScheduleDetailsById(ScheduleId, userId);
                if (result.Response != null)
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
        //public HttpResponseMessage ScheduleDetailsById(Int64 ScheduleId)
        //{
        //    EditExerciseResponseModel result = new EditExerciseResponseModel();
        //    FResponse res = new FResponse();
        //    try
        //    {
        //        var headers = Request.Headers;
        //        string token = headers.Authorization.Parameter.ToString();
        //        Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

        //        //result = _objIScheduleExerciseRepository.ScheduleDetailsById(ScheduleId, userId);
        //        if (result.Response != null)
        //        {
        //            result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            result.Response.Message = "Success!!";
        //        }
        //        else
        //        {
        //            result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
        //            result.Response.Message = "No Records";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
        //    }
        //    _response = Request.CreateResponse(HttpStatusCode.OK, result);
        //    return _response;
        //}

        [HttpPost]
        [Route("AddScheduleExercise1")]
        [SecureResource]
        public HttpResponseMessage AddScheduleExercise1(AddExerciseRequestModel objAddExerciseRequestModel)
        {
            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                int value = _objIScheduleExerciseRepository.AddScheduleExercise1(objAddExerciseRequestModel);
                if (value > 0)
                {
                    result.WorkoutId = objAddExerciseRequestModel.WorkOutId;
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Goal Schedule Exercise added successfully!";
                }
                else
                {
                    result.WorkoutId = objAddExerciseRequestModel.WorkOutId;
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
        [Route("EditScheduleExercise1")]
        [SecureResource]
        public HttpResponseMessage EditScheduleExercise1(UpdateActualExerciseRequest objUpdateActualExerciseRequest)
        {
            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                int value = _objIScheduleExerciseRepository.EditScheduleExercise1(objUpdateActualExerciseRequest);

                if (value > 0)
                {
                    result.WorkoutId = objUpdateActualExerciseRequest.WorkOutId;
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Schedule Updated successfully!";
                }
                else
                {
                    result.WorkoutId = objUpdateActualExerciseRequest.WorkOutId;
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

    }
}
