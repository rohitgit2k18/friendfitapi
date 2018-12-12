using FriendFit.API.Filters;
using FriendFit.Data;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.IRepository;
using FriendFit.Data.Repository;
using FriendFit.Entity.ApiModel.APIResponseModel;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/Exercise")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ExcerciseController : ApiController
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();
        private IExerciseRepository _objIExerciseRepository;
        public ExcerciseController()
        {
            _objIExerciseRepository = new ExerciseRepository();
        }
        private HttpResponseMessage _response;


        [HttpPost]
        [Route("AddExercise")]
        [SecureResource]
        public HttpResponseMessage AddExercise(AddExerciseRequestModel objAddExerciseRequestModel)
        {
            FResponse result = new FResponse();
              try
                {
                    var headers = Request.Headers;
                    string token = headers.Authorization.Parameter.ToString();
                    Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                    int value = _objIExerciseRepository.AddExercise(objAddExerciseRequestModel);
                    if (value > 0)
                    {
                    result.WorkoutId = objAddExerciseRequestModel.WorkOutId;
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Message = "Exercise added successfully!";
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
        [Route("AddExerciseSchedule")]
        [SecureResource]
        public HttpResponseMessage AddExerciseSchedule(AddExerciseRequestModel objAddExerciseRequestModel)
        {
            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                int value = _objIExerciseRepository.AddExerciseSchedule(objAddExerciseRequestModel);
                if (value > 0)
                {

                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Exercise added successfully!";
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

        [HttpGet]
        [Route("ExerciseDetailsByWorkOutId")]
        [SecureResource]
        public HttpResponseMessage ExerciseDetailsByWorkOutId(Int64 WorkOutId)
        {
            EditExerciseResponseModel result = new EditExerciseResponseModel();
            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result = _objIExerciseRepository.ExerciseDetailsByWorkOutId(WorkOutId,userId);

                //result.Response.editExercise.weightList = _objIExerciseRepository.WeightExerciseList(result.Response.editExercise.ExerciseSetId).ToList(); ;
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

        [HttpPost]
        [Route("ExerciseTypeList")]
        public HttpResponseMessage ExerciseTypeList()
        {
            ExerciseTypeListResponseModel result = new ExerciseTypeListResponseModel();
            try
            {
                result.Response.exerciseList = _objFriendFitDBEntity.Database.SqlQuery<ExerciseTypeList>("select Id, TypeName from ExerciseTypeMaster").ToList();
                if (result.Response.exerciseList != null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Exercise Type list";
                }
                else
                {
                    _response = Request.CreateResponse(HttpStatusCode.NotFound, "No data in Exercise Type List");
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
        [Route("AddActualExercise")]
        [SecureResource]
        public HttpResponseMessage AddActualExercise(UpdateActualExerciseRequest objUpdateActualExerciseRequest)
        {
            AddActualResponse result = new AddActualResponse();
            
                try
                {
                    var headers = Request.Headers;
                    string token = headers.Authorization.Parameter.ToString();
                    Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                    int value = _objIExerciseRepository.UpdateExercise(objUpdateActualExerciseRequest);
                    if (value > 0)
                    {
                        result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Response.Message = "Exercise Added successfully!";
                    }
                    else
                    {
                        result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                        result.Response.Message = "Parameters are not correct";
                    }
                }
                catch (Exception ex)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
                _response = Request.CreateResponse(HttpStatusCode.OK, result);
            
           
            return _response;
        }


        [HttpPost]
        [Route("UpdateActualExercise")]
        [SecureResource]
        public HttpResponseMessage UpdateActualExercise(Int64 UserId, Int64 ExerciseId, UpdatingActExistingRequest objReq)
        {
            FResponse result = new FResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    var headers = Request.Headers;
                    string token = headers.Authorization.Parameter.ToString();
                    Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();
                    int value = _objIExerciseRepository.UpdateActualExercise(UserId, ExerciseId, objReq);
                    if(value>0)
                    {
                        result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Message = "Actual Exercise Updated successfully!";
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
            }
            else
            {
                ModelState.AddModelError("", "One or more errors occurred.");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpGet]
        [Route("AllPreviouseExerciseDetailsByWorkOutId")]
        [SecureResource]
        public HttpResponseMessage AllPreviouseExerciseDetailsByWorkOutId(Int64 WorkOutId)
        {
            all_EditExerciseResponseModel result = new all_EditExerciseResponseModel();

            FResponse res = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                // result = _objIExerciseRepository.AllPreviouseExerciseDetailsByWorkOutId(WorkOutId, userId);


                result = _objIExerciseRepository.AllPreviouseExerciseDetailsByWorkOutId(WorkOutId, userId);


                //result.Response.editExercise.weightList = _objIExerciseRepository.WeightExerciseList(result.Response.editExercise.ExerciseSetId).ToList(); ;
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

    }
}
