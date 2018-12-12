using FriendFit.API.Filters;
using FriendFit.Data;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.IRepository;
using FriendFit.Data.Repository;
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
    [RoutePrefix("api/Dashborad")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DashboardController : ApiController
    {
        
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        private IDashboardRepository _objIDashboardRepository;
        private HttpResponseMessage _response;
        public DashboardController()
        {
            _objIDashboardRepository = new DashboardRepository();
        }

        [HttpGet]
        [Route("CompletedWorkout")]
        [SecureResource]
        public HttpResponseMessage CompletedWorkout()
        {
            CompletedWorkoutResponse result = new CompletedWorkoutResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result.Response.compWorkout = _objFriendFitDBEntity.Database.SqlQuery<CompletWorkout>("GetCompletedWorkout @UserId=@UserId",
                    new SqlParameter("UserId",UserId)).FirstOrDefault();
                if(result.Response.compWorkout!=null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Success!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Response.Message = "No Records";
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
        [Route("MissedWorkout")]
        [SecureResource]
        public HttpResponseMessage MissedWorkout()
        {
            MissedWorkoutResponse result = new MissedWorkoutResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result.Response.missedWorkout = _objFriendFitDBEntity.Database.SqlQuery<MissedWorkout>("GetMissedWorkout @UserId=@UserId",
                    new SqlParameter("UserId", UserId)).FirstOrDefault();
                if (result.Response.missedWorkout != null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Success!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Response.Message = "No Records";
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



        [HttpGet]
        [Route("DashboardWorkoutData")]
        [SecureResource]
        public HttpResponseMessage DashboardWorkoutData()
        {
            DashboardWorkoutDataResponse result = new DashboardWorkoutDataResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result.Response.compWorkout = _objFriendFitDBEntity.Database.SqlQuery<DashboardWorkout>("GetCompletedWorkout @UserId=@UserId",
                    new SqlParameter("UserId", UserId)).FirstOrDefault();
                if (result.Response.compWorkout != null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Success!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Response.Message = "No Records";
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


        [HttpGet]
        [Route("Logo")]
        public string Logo()
        {
            string ImagePath = "";
            try
            {
                ImagePath = "~/images/"+_objFriendFitDBEntity.Logoes.Where(x => x.Status == true).OrderByDescending(x => x.Createdate).FirstOrDefault().ImagePath;
            }
            catch (Exception ex)
            {
                ImagePath = Convert.ToString(ex);
            }
            return ImagePath;
        }
    }
}
 