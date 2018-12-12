using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/Price")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PriceController : ApiController
    {
        private FriendFit.Data.FriendFitDBContext _objFriendFitDBEntity = new FriendFit.Data.FriendFitDBContext();

        [HttpGet]
        [Route("GetPriceList")]
        public PriceListModelResponse GetPriceList()
        {
            PriceListModelResponse pmr = new PriceListModelResponse();
            List<PriceList> model = new List<PriceList>();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();
                var P_List = _objFriendFitDBEntity.tblPrices.ToList();

                foreach (var item in P_List)
                {
                    PriceList pl = new PriceList();
                    pl.Id = item.Id;
                    pl.IsSMS = item.IsSMS;
                    pl.One_Off = Convert.ToDecimal(item.One_Off);
                    pl.Recurring = Convert.ToDecimal(item.Recurring);
                    pl.RowInsertDate = item.RowInsert;
                    pl.RowInsertUpdate = item.RowUpdate;
                    pl.Duration = item.Duration;

                    model.Add(pl);
                }
                if (model.Count > 0)
                {
                    pmr.Response.PriceList = model;
                    pmr.Response.StatusCode= Convert.ToInt32(HttpStatusCode.OK);
                    pmr.Response.Message= "Goal Exercise added successfully!";
                }
                else
                {
                    pmr.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                    pmr.Response.Message = "Parameters are not correct";
                }
            }
            catch (Exception ex)
            {
                pmr.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                //pmr = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            //pmr = Request.CreateResponse(HttpStatusCode.OK, model);
            return pmr;
        }


        [HttpPost]
        [Route("GetPrice")]
        public PriceModelResponse GetPrice(GetPriceRequestModel _objGetPriceRequestModel)
        {
            PriceModelResponse pmr = new PriceModelResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();
                var Price = _objFriendFitDBEntity.GetPrice(_objGetPriceRequestModel.Duration, _objGetPriceRequestModel.Billing, _objGetPriceRequestModel.SendVia).Count();

                if (Price>0)
                {
                    var _Price = _objFriendFitDBEntity.GetPrice(_objGetPriceRequestModel.Duration, _objGetPriceRequestModel.Billing, _objGetPriceRequestModel.SendVia).FirstOrDefault();
                    pmr.Price= _Price;
                    pmr.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    pmr.Message = "successfully!";
                }
                else
                {
                    pmr.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    pmr.Message = "No Record Found !";
                }
            }
            catch (Exception ex)
            {
                pmr.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                pmr.Message = Convert.ToString(ex);
            }
            return pmr;
        }
    }
}
