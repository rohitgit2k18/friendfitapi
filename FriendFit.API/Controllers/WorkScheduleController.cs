using FriendFit.API.Filters;
using FriendFit.Data;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.IRepository;
using FriendFit.Data.Repository;
using FriendFit.Entity.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/Schedule")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WorkScheduleController : ApiController
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();
        private IScheduleRepository _objIScheduleRepository;
        private IWorkoutRepository _objIIWorkoutRepository;
        private HttpResponseMessage _response;

        public WorkScheduleController()
        {
            _objIScheduleRepository = new ScheduleRepository();
            _objIIWorkoutRepository = new WorkoutRepository();
        }

        [HttpPost]
        [Route("AddWorkoutSchedule")]
        [SecureResource]
        public HttpResponseMessage AddWorkoutSchedule(AddScheduleRequestModel objAddScheduleRequestModel)
        {
            FResponse result = new FResponse();
            if (ModelState.IsValid)
            {
                try
                {                   
                    var headers = Request.Headers;
                    string token = headers.Authorization.Parameter.ToString();
                    Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                    int value = _objIScheduleRepository.AddSchedule(objAddScheduleRequestModel,UserId);
                    if (value > 0)
                    {
                        result.WorkoutId = value;
                        result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Message = "Workout Schedule added successfully!";
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
        [Route("ListOfScheduleWorkout")]
        [SecureResource]
        public HttpResponseMessage ListOfSchedule(Int64 UserId)
        {
            ScheduleListResponseModel result = new ScheduleListResponseModel();
            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();
              

                result.Response.scheduleLists = _objIScheduleRepository.ScheduleList(UserId);
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

        [HttpPost]
        [Route("ScheduleWorkoutDetails")]
        [SecureResource]
        public HttpResponseMessage ScheduleWorkoutDetails(Int64 ScheduleId)
        {
            ScheduleDetailsResponseModel result = new ScheduleDetailsResponseModel();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result.Response.deatils = _objIScheduleRepository.ScheduleDetails(UserId,ScheduleId);
                if(result.Response.deatils!=null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Success!!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Response.Message = "Data is not found";
                }
            }
            catch(Exception ex)
            {
                result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpGet]
        [Route("RecurrenceTypeList")]
       // [SecureResource]
        public HttpResponseMessage RecurrenceTypeList()
        {
            RecurrenceListResponseModel result = new RecurrenceListResponseModel();
            try
            {

                result.Response.recurrenceList = _objFriendFitDBEntity.Database.SqlQuery<RecurrenceList>("Select Id,RecurrenceType from Recurrence").ToList();
                if (result.Response.recurrenceList != null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Recurrence List";
                }
                else
                {
                    _response = Request.CreateResponse(HttpStatusCode.NotFound, "No data in Recurrence List");
                }
            }
            catch (Exception ex)
            {
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpPost]
        [Route("UpdateScheduleWorkout")]
        [SecureResource]
        public HttpResponseMessage UpdateScheduleWorkout(UpdateScheduleWorkoutRequestModel objUpdateScheduleWorkoutRequestModel,Int64 ScheduleId)
        {

            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                int updateModel = _objIScheduleRepository.UpdateScheduleWorkout(objUpdateScheduleWorkoutRequestModel, ScheduleId, UserId);
                if (updateModel > 0)
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
        [Route("DeleteSchedule/{ScheduleId}")]
        [SecureResource]
        public HttpResponseMessage DeleteSchedule(int ScheduleId)
        {
            FResponse result = new FResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    var headers = Request.Headers;
                    string token = headers.Authorization.Parameter.ToString();
                    Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                    int value = _objIScheduleRepository.DeleteSchedule(ScheduleId, UserId);
                    if (value > 0)
                    {
                        result.WorkoutId = value;
                        result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Message = "Workout Schedule Deleted successfully!";
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
    }
}
