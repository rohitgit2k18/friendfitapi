using FriendFit.API.Filters;
using FriendFit.API.Models;
using FriendFit.Data;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.IRepository;
using FriendFit.Data.Repository;
using FriendFit.Entity.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/Friend")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FriendInvitationController : ApiController
    {
        private string _profileImagesPath = WebConfigurationManager.AppSettings["ProfileImages"];
        private string _ServerURL = WebConfigurationManager.AppSettings["FrendFitAPIServerURL"];
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        private IFriendInvitationRepository _objIFriendInvitationRepository;
        private HttpResponseMessage _response;
        private TwilioSMSDemoController SMSCont = new TwilioSMSDemoController();
        public FriendInvitationController()
        {
            _objIFriendInvitationRepository = new FriendInvitationRepository();
        }

        [HttpPost]
        [Route("AddFriendInvitation")]
        [SecureResource]
        public HttpResponseMessage AddFriendInvitation(AddFriendInvitationRequestModel objFriendInvitation)
        {
            AddFriendIFitResponse result = new AddFriendIFitResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                string checkuser = _objFriendFitDBEntity.Database.SqlQuery<string>("select Email from FriendsInvitation where Email={0}", objFriendInvitation.Email).FirstOrDefault();
                string UserName = _objFriendFitDBEntity.Database.SqlQuery<string>("select FirstName from UserProfile where Id={0}", UserId).FirstOrDefault();
                int value = _objIFriendInvitationRepository.AddFriendInvitation(objFriendInvitation);

                string RegistrationUrl = WebConfigurationManager.AppSettings["FrendFitSignUp"];
                #region send mail for notification
                if (objFriendInvitation.Email != null)
                {
                    if (objFriendInvitation.Email != "")
                    {
                        var SendingMessage = new MailMessage();
                        SendingMessage.To.Add(new MailAddress(objFriendInvitation.Email));  // replace with valid value
                        SendingMessage.From = new MailAddress("noreply@noti.fit");  // replace with valid value
                        SendingMessage.Subject = "Notification your email (noti.fit)";
                        //SendingMessage.Body = " <p><strong> Hi " + objFriendInvitation.Email + "</ strong >  ,<br/>Congratulations for on signing up to noti.fit so please click on,<br/><br/><strong>Please visit</strong> <a href='" + RegistrationUrl +"'>" + RegistrationUrl+ "</a>  <br/>  to fill the details and click on sign up button<br/><br/>Kind regards,<br/>The noti.fit team";
                        SendingMessage.Body = "<p>Hi " + objFriendInvitation.FriendsName + "</p><p>Your friend " + objFriendInvitation.Email + " has invited you to track their workouts on noti.fit!noti.fit is a workout tracker that sends you notifications when your friend misses a workout.</ p >< p > Please sign up at " + RegistrationUrl + " to get started!</ p >< p > Cheers<strong>,</ strong >< br /> The noti.fit tea</ p > ";

                        SendingMessage.IsBodyHtml = true;
                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = "testifiedemail@gmail.com",  // replace with valid value
                                Password = "testifiedpassword@hackfree"  // replace with valid value
                            };
                            smtp.Credentials = credential;
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            smtp.Send(SendingMessage);
                        }
                    }
                }
                #endregion

                #region notification via sms
                if (objFriendInvitation.MobileNumber != null)
                {
                    if (objFriendInvitation.MobileNumber != "")
                    {
                        twilioModel tm = new twilioModel();
                        tm.mobileNo = objFriendInvitation.MobileNumber;
                        tm.messagebody = "Hi " + UserName + ", " + objFriendInvitation.FriendsName + " has nominated you to receive their workout reminders. Please accept/ decline at " + RegistrationUrl + " or ignore this text for no further communication.";
                        var SMSStatus = SMSCont.SendSMS(tm);
                    }
                }
                #endregion
                if (value > 0)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Friend invitation Added successfully";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                    result.Response.Message = "Some parameters is incorrect";
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
        [Route("ListOfInvitedFriend")]
        [SecureResource]
        public HttpResponseMessage ListOfInvitedFriend()
        {
            ListOfInvitedFriendResponse result = new ListOfInvitedFriendResponse();

            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                //result.Response.listofFriends = _objIFriendInvitationRepository.ListOfInvitedFriend(UserId);
                List<ListOfInvitedFriends> obj = new List<ListOfInvitedFriends>();

                obj = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedFriends>("ListOfFriends @UserId=@UserId",
                    new SqlParameter("UserId", UserId)).ToList();

                var _ListOfInvitedFriends = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedFriends>("ListOfFriends @UserId=@UserId",
                     new SqlParameter("UserId", UserId)).Where(s => s.PaymentDone == 0).ToList();

                foreach (var item in _ListOfInvitedFriends)
                {
                    DateTime dt_CurrentDate = DateTime.Today.Date;
                    DateTime dt_PurchaseDate = Convert.ToDateTime(item.PurchaseDate).Date;
                    DateTime dt_ExpiryDate = Convert.ToDateTime(item.ExpiryDate).Date;
                    DateTime New_dt_PurchaseDate;
                    DateTime New_dt_ExpiryDate;

                    if (dt_PurchaseDate < dt_CurrentDate)
                    {
                        double TotalDayDiff = (dt_CurrentDate - dt_PurchaseDate).TotalDays;
                        New_dt_PurchaseDate = Convert.ToDateTime(item.PurchaseDate).Date.AddDays(TotalDayDiff);
                        New_dt_ExpiryDate = Convert.ToDateTime(item.ExpiryDate).Date.AddDays(TotalDayDiff);

                        var UpdateFriendsInvitations = _objFriendFitDBEntity.FriendsInvitations.Where(x => x.Id == item.FriendId).FirstOrDefault();
                        UpdateFriendsInvitations.PurchaseDate = New_dt_PurchaseDate;
                        UpdateFriendsInvitations.ExpiryDate = New_dt_ExpiryDate;
                        _objFriendFitDBEntity.Entry(UpdateFriendsInvitations).State = System.Data.Entity.EntityState.Modified;
                        _objFriendFitDBEntity.SaveChanges();
                    }
                }

                result.Response.listofFriends = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedFriends>("ListOfFriends @UserId=@UserId",
                    new SqlParameter("UserId", UserId)).ToList();

                if (result.Response.listofFriends.Count > 0)
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

        [HttpPost]
        [Route("SendRecurringNotification")]
        [SecureResource]
        public HttpResponseMessage SendRecurringNotification()
        {
            ListOfInvitedFriendResponse result = new ListOfInvitedFriendResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                //result.Response.listofFriends = _objIFriendInvitationRepository.ListOfInvitedFriend(UserId);
                List<ListOfInvitedFriends> obj = new List<ListOfInvitedFriends>();

                obj = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedFriends>("SendRecurringNotification @UserId=@UserId",
                    new SqlParameter("UserId", UserId)).ToList();


                result.Response.listofFriends = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedFriends>("SendRecurringNotification @UserId=@UserId",
                    new SqlParameter("UserId", UserId)).ToList();

                #region Send a mail Recuring
                foreach (var item in obj)
                {
                    var email = item.Email;
                    DateTime d1 = Convert.ToDateTime(item.ExpiryDate);
                    DateTime d2 = DateTime.Now;

                    TimeSpan t = d1 - d2;
                    double NrOfDays = t.TotalDays;
                    int ndays = Convert.ToInt32(NrOfDays);
                    if (ndays <= -6 || ndays <= -5 || ndays <= -4 || ndays <= -3 || ndays <= -2 || ndays <= -1)
                    {
                        if (item.Email != null)
                        {
                            // update recursive flag
                            int value = _objFriendFitDBEntity.Database.ExecuteSqlCommand("update FriendsInvitation set RecFlag='RF' where Email='" + item.Email + "' and FriendsName='" + item.FriendsName + "'");
                            //
                            string RegistrationUrl = WebConfigurationManager.AppSettings["FrendFitSignUp"];
                            var SendingMessage = new MailMessage();
                            SendingMessage.To.Add(new MailAddress(item.Email));
                            SendingMessage.From = new MailAddress("noreply@noti.fit");
                            SendingMessage.Subject = "Recuring Notification email (noti.fit)";
                            //SendingMessage.Body = "<p>Hi [FirstNameOfNewFriend]</p>< p >Your friend[FirstNameOfExistingUser] has invited you to track their workouts on noti.fit!noti.fit is a workout tracker that sends you notifications when your friend misses a workout.</ p >< p > Please sign up at[link] to get started!</ p >< p > Cheers<strong>,</ strong >< br /> The noti.fit tea</ p > ";
                            SendingMessage.Body = " <p><strong> Hi " + item.Email + "</ strong >  ,<br/>Recurring : Recurring payment to be due " + item.ExpiryDate + " of this date,<br/><br/><strong>Please visit</strong> <br/><br/>Kind regards,<br/>The noti.fit team";
                            SendingMessage.IsBodyHtml = true;

                            using (var smtp = new SmtpClient())
                            {
                                var credential = new NetworkCredential
                                {
                                    UserName = "testifiedemail@gmail.com",
                                    Password = "testifiedpassword@hackfree"
                                };
                                smtp.Credentials = credential;
                                smtp.Host = "smtp.gmail.com";
                                smtp.Port = 587;
                                smtp.EnableSsl = true;
                                smtp.Send(SendingMessage);
                            }
                        }
                    }
                }
                #endregion

                if (result.Response.listofFriends.Count > 0)
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


        [HttpPost]
        [SecureResource]
        [Route("UploadUserProfile")]
        public async Task<HttpResponseMessage> PostUserImage()
        {
            UploadUserPicRequestModel objUploadUserPicRequestModel = new UploadUserPicRequestModel();
            PicUploadResponse res = new PicUploadResponse();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 3; //Size = 3 MB 
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault().Substring(postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault().LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                            res.Response.Message = string.Format("Please Upload image of type .jpg,.gif,.png");
                            _response = Request.CreateResponse(HttpStatusCode.NotAcceptable, res);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                            res.Response.Message = string.Format("Please Upload a file upto 2 mb.");
                            _response = Request.CreateResponse(HttpStatusCode.NotAcceptable, res);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Content/ProfileImages/" + postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault());
                            objUploadUserPicRequestModel.ProfilePic = _profileImagesPath + postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                            postedFile.SaveAs(filePath);
                            int value = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Update UserProfile set ProfilePic=@ProfilePic where Id=@UserId",
                             new SqlParameter("ProfilePic", objUploadUserPicRequestModel.ProfilePic),
                             new SqlParameter("UserId", UserId)
                           );
                            if (value > 0)
                            {
                                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                res.Response.Message = "Profile Picture is updated successfully!!";
                                _response = Request.CreateResponse(HttpStatusCode.OK, res);
                            }
                            else
                            {
                                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                                res.Response.Message = "Picture which you are trying to update is incorrect Format.";
                                _response = Request.CreateResponse(HttpStatusCode.NotAcceptable, res);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                res.Response.Message = "Some error occured!!";
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, res);
            }
            return _response;
        }

        [HttpPost]
        [SecureResource]
        [Route("GetUserImage")]
        public HttpResponseMessage GetUserImage()
        {
            UpdateUserPicResponseModel result = new UpdateUserPicResponseModel();
            PicUploadResponse res = new PicUploadResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                if (UserId != 0)
                {
                    string value = _objFriendFitDBEntity.Database.SqlQuery<string>("select ProfilePic from UserProfile where Id=@UserId", new SqlParameter("UserId", UserId)).FirstOrDefault();

                    if (value != null)
                    {
                        result.Response.ProfilePic = _ServerURL + value;
                        result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Response.Message = "Success!!";

                    }
                    else
                    {
                        result.Response.ProfilePic = _ServerURL + "/Content/ProfileImages/default.png";
                        result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Response.Message = "Success!!";
                        //res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                        //res.Response.Message = "Image does not found!!";
                        //_response = Request.CreateResponse(HttpStatusCode.NotFound, res);
                    }
                }
                else
                {
                    res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    res.Response.Message = "User Id does not found!!";
                    _response = Request.CreateResponse(HttpStatusCode.NotFound, res);
                }
            }
            catch (Exception ex)
            {

                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                res.Response.Message = "Some error occured!!";
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, res);
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpPost]
        [SecureResource]
        [Route("DeleteFriend")]
        public HttpResponseMessage DeleteFriend(Int64 id)
        {
            PicUploadResponse res = new PicUploadResponse();
            try
            {
                int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Update FriendsInvitation set IsRowActive=0 where Id={0}", id);
                if (rowEffected > 0)
                {
                    res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    res.Response.Message = "Friend Notification is deleted successfully!!";
                }
                else
                {
                    res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    res.Response.Message = "Id of given user is not there!";
                }
            }
            catch (Exception ex)
            {
                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                res.Response.Message = "Some error occured!!";
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, res);
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, res);
            return _response;
        }

        [HttpGet]
        [Route("DeliveryMethodList")]
        public HttpResponseMessage DeliveryMethodList()
        {
            DeliveryMethodListResponse result = new DeliveryMethodListResponse();
            PicUploadResponse res = new PicUploadResponse();
            try
            {
                result.Response.ListOfDelivery = _objFriendFitDBEntity.Database.SqlQuery<DeliveryMethodList>("select Id,Name from DeliveryTypeMaster").ToList();
                if (result.Response.ListOfDelivery.Count > 0)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    res.Response.Message = "Success!!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    res.Response.Message = "Id is not Found";
                }

            }
            catch (Exception ex)
            {
                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                res.Response.Message = "Some error occured!!";
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, res);
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpGet]
        [Route("DurationList")]
        public HttpResponseMessage DurationList()
        {
            DurationListResponse result = new DurationListResponse();
            PicUploadResponse res = new PicUploadResponse();
            try
            {
                result.Response.listofDuration = _objFriendFitDBEntity.Database.SqlQuery<DurationList>("select Id,TotalMonths from DurationMaster").ToList();
                if (result.Response.listofDuration.Count > 0)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    res.Response.Message = "Success!!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    res.Response.Message = "Id is not Found";
                }

            }
            catch (Exception ex)
            {
                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                res.Response.Message = "Some error occured!!";
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, res);
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpGet]
        [Route("FrequencyList")]
        public HttpResponseMessage FrequencyList()
        {
            FrequencyListResponse result = new FrequencyListResponse();
            PicUploadResponse res = new PicUploadResponse();
            try
            {
                result.Response.listofFreq = _objFriendFitDBEntity.Database.SqlQuery<FrequencyList>("select Id,Frequency from FrequencyMaster").ToList();
                if (result.Response.listofFreq.Count > 0)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    res.Response.Message = "Success!!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    res.Response.Message = "Id is not Found";
                }

            }
            catch (Exception ex)
            {
                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                res.Response.Message = "Some error occured!!";
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, res);
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }


        [HttpGet]
        [Route("SubscriptionList")]
        public HttpResponseMessage SubscriptionList()
        {
            SubscriptionListResponse result = new SubscriptionListResponse();
            PicUploadResponse res = new PicUploadResponse();
            try
            {
                result.Response.listofSubscription = _objFriendFitDBEntity.Database.SqlQuery<SubscriptionList>("select Id,SubcriptionType from SubscriptionTypeMaster").ToList();
                if (result.Response.listofSubscription.Count > 0)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    res.Response.Message = "Success!!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    res.Response.Message = "Id is not Found";
                }

            }
            catch (Exception ex)
            {
                res.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                res.Response.Message = "Some error occured!!";
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, res);
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }
    }
}
