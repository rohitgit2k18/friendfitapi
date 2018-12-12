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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{

       [RoutePrefix("api/User")]
     [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserSettingsController : ApiController
    {
            private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();
        string ChangePasswordUrl = WebConfigurationManager.AppSettings["FrendFitLocal"];
        string RegistrationUrl = WebConfigurationManager.AppSettings["MailConfirmation"];

        const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$";
        private bool IsPasswordValid;
        private IUserSettingRepository _objIUserSettings;
            private HttpResponseMessage _response;

        private TwilioSMSDemoController SMSCont=new TwilioSMSDemoController();
        public UserSettingsController()
        {
            _objIUserSettings = new UserSettingsRepository();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login(LoginModelRequest objLoginModelRequest)
        {
            LoginModelResponse result = new LoginModelResponse(); 
            if(ModelState.IsValid)
            {
                try
                {

                    Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("Select Id from UserProfile where Email={0}", objLoginModelRequest.Email).FirstOrDefault();
                    bool mailVerified = _objFriendFitDBEntity.Database.SqlQuery<bool>("select EmailConfirmed from UserProfile where Id={0}", UserId).FirstOrDefault();
                    if (mailVerified == false)
                    {
                        result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                        result.Response.Message = "Please Verify Your Email Id that has been sent to your mail.";
                    }
                    else
                    {
                        result.Response = _objIUserSettings.Login(objLoginModelRequest);
                        //Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select Id from UserProfile where Email={0}", objLoginModelRequest.Email).FirstOrDefault();

                        if (result.Response != null)
                        {
                            string Token = _objFriendFitDBEntity.Database.SqlQuery<string>("select TokenCode from UserToken where UserId={0}", UserId).FirstOrDefault();

                            if (Token == null || Token == "0")
                            {
                                if(Token == null)
                                {
                                    UserToken objToken = new UserToken()
                                    {
                                        UserId = result.Response.Id,
                                        RoleId = result.Response.RoleId,
                                        CreatedOn = DateTime.Now,
                                        IsActive = true,
                                        ExpiryDate = DateTime.Now.AddDays(7),
                                        TokenCode = Guid.NewGuid().ToString() + result.Response.Id.ToString() + Guid.NewGuid().ToString()
                                    };
                                    _objFriendFitDBEntity.UserTokens.Add(objToken);
                                    _objFriendFitDBEntity.SaveChanges();
                                    result.Response.TokenCode = objToken.TokenCode;
                                }
                                else
                                {
                                   
                                    int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Update Token set TokenCode=@TokenCode,ExpiryDate=@ExpiryDate where UserId=@UserId",
                                                               new SqlParameter("TokenCode", Guid.NewGuid().ToString() + result.Response.Id.ToString() + Guid.NewGuid().ToString()),
                                                               new SqlParameter("ExpiryDate", DateTime.Now.AddDays(7)),
                                                               new SqlParameter("UserId", UserId));

                                }
                               
                            }
                            else
                            {
                                result.Response.TokenCode = Token;
                            }
                            result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                            result.Response.Message = "You are logged in successfully!";
                        }
                        else
                        {
                            var GetIsActive = _objFriendFitDBEntity.UserProfiles.Where(a => a.Password == objLoginModelRequest.Password && a.Email == objLoginModelRequest.Email).FirstOrDefault();
                            if (GetIsActive!=null)
                            {
                                var IsActive = _objFriendFitDBEntity.UserProfiles.Where(a => a.Password == objLoginModelRequest.Password && a.Email == objLoginModelRequest.Email && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                                if (IsActive!=null)
                                {
                                    FResponse res = new FResponse();
                                    res.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                                    res.Message = "Email or Password is Incorrect";
                                    _response = Request.CreateResponse(HttpStatusCode.Unauthorized, res);
                                }
                                else
                                {
                                    FResponse res = new FResponse();
                                    res.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                                    res.Message = "Your Account is currently disabled kindly contact Admin.";
                                    _response = Request.CreateResponse(HttpStatusCode.Unauthorized, res);

                                }
                            }
                            else
                            {
                                FResponse res = new FResponse();
                                res.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                                res.Message = "Email or Password is Incorrect";
                                _response = Request.CreateResponse(HttpStatusCode.Unauthorized, res);
                            }
                           
                            return _response;
                        }
                    }
                    
                  
                }
                catch (Exception ex)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
            }   
            else
            {
                result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Model is not valid");
            }
            //result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
            //result.Response.Message = "Success";
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }
        
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<HttpResponseMessage> UserForgetPassword(ForgetPasswordRequest reqForgetPasswordRequest)
        {
            FResponse result = new FResponse();
            try
            {
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select Id from UserProfile where Email={0}", reqForgetPasswordRequest.Email).FirstOrDefault();
              
                if (UserId != 0)
                {
                    _objIUserSettings.UpdateToken(UserId);
                    string token = _objFriendFitDBEntity.Database.SqlQuery<string>("select TokenCode from UserToken where UserId={0}",UserId).FirstOrDefault();

                    string Email = _objFriendFitDBEntity.Database.SqlQuery<string>("Select Email from UserProfile Where Id=" + UserId).FirstOrDefault();


                    var SendingMessage = new MailMessage();
                    SendingMessage.To.Add(new MailAddress(Email));  // replace with valid value
                    SendingMessage.From = new MailAddress("noreply@noti.fit");  // replace with valid value
                    SendingMessage.Subject = "Password Reset (noti.fit)";
                    SendingMessage.Body = "Hi,<br/>You've requested a reset of your noti.fit password. If you didn't make the request, please ignore this email and your password won't be reset.<br/><br/> You can reset your password by visiting <br/> <br/><a href='" + ChangePasswordUrl + token + "'>" + ChangePasswordUrl + token + "</a> <br/><br/>This email will be valid for the next 12 hours.,<br/><br/><br/><br/>Kind regards<br/>The noti.fit team";
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
                        await smtp.SendMailAsync(SendingMessage);  
                    }
                    //track sent Email time for expiry time :

                    EmailTimeSaveModel objreq = new EmailTimeSaveModel();
                    objreq.UserId = UserId;
                    objreq.ResetMail = true;
                    objreq.VerifyMail = false;
                    objreq.MailSentTime = DateTime.Now.TimeOfDay;
                    EmailTrackerWrapper wrapper = new EmailTrackerWrapper();
                    wrapper.EmailTimeSave(objreq);

                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                   result.Message= "Please check your Email for further instructions";
                }
                else
                {                   
                    FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/Content/ErrorLog.txt"), FileMode.Append, FileAccess.Write);
                    StreamWriter swr = new StreamWriter(fs);
                    swr.Write("Enter ur Exception Here");
                    swr.Close();
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                    result.Message = "This Mail Id is not registered";
                }
                var message = Request.CreateResponse(HttpStatusCode.Created, result);
                return message;
            }
            catch (Exception ex)
            {
                result.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            return _response;
        }

        [HttpPost]
        [Route("ResetPassword")]
        [SecureResource]
        public HttpResponseMessage UserResetPassword(ResetPasswordRequest objResetPasswordRequest)
        {

            FResponse result = new FResponse();
            if(ModelState.IsValid)
            {
                try
                {
                    IsPasswordValid = (Regex.IsMatch(objResetPasswordRequest.Password, passwordRegex));
                    //if (IsPasswordValid == true)
                    //{
                        var headers = Request.Headers;
                        string token = headers.Authorization.Parameter.ToString();
                        Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                        TimeSpan mailSentTime = _objFriendFitDBEntity.Database.SqlQuery<TimeSpan>("SELECT TOP 1 MailSentTime FROM MailTimeLogs where UserId=@UserId ORDER BY Id DESC",
                                                                                                    new SqlParameter("UserId", UserId)).FirstOrDefault();
                        TimeSpan Nowtime = DateTime.Now.TimeOfDay;
                        var TimeDiff = Nowtime - mailSentTime;
                        if (TimeDiff.Hours > 12)
                        {
                            result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                            result.Message = "Link is Expired";
                        }
                        else
                        {
                            //update password if link is not expired
                            var model = _objIUserSettings.ResetPassword(objResetPasswordRequest, UserId);

                            if (model > 0)
                            {
                                result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                result.Message = "Your password has been updated successfully.";
                            }
                            else
                            {
                                result.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                                result.Message = "Not Updated";
                            }
                        }
                    //}
                    //else
                    //{
                        
                    //    result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                    //    result.Message = "Password Must contain at least one number and one uppercase and lowercase letter,  and atleast one special character and  must be in between 6 to 20 characters";
                    //}                                      
                    
                    _response = Request.CreateResponse(HttpStatusCode.OK, result);
                    return _response;
                }
                catch (Exception ex)
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
            }
            else
            {
                result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                result.Message = "Request is not valid !!";
               
            }
            
            return _response;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<HttpResponseMessage> UserSignUp(SignUpModelRequset objSignUpModelRequset)
        {
            SignUpResponseModelResponse result = new SignUpResponseModelResponse();
            if(ModelState.IsValid)
            {
                try
                {
                    IsPasswordValid = (Regex.IsMatch(objSignUpModelRequset.Password, passwordRegex));
                    //if (IsPasswordValid == true)
                    //{
                        string IsMailIdExist = _objFriendFitDBEntity.Database.SqlQuery<string>("Select Email from UserProfile where Email={0}", objSignUpModelRequset.Email).FirstOrDefault();

                        if (IsMailIdExist == null)
                        {
                            var model = _objIUserSettings.AddUser(objSignUpModelRequset);
                            Random random = new Random();
                        Int64 otp = Convert.ToInt64(random.Next(1000, 9999)); /// to specify range for random number
                        Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("Select Id from UserProfile where Email={0}", objSignUpModelRequset.Email).FirstOrDefault();
                            int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("CreateNewToken @UserId=@UserId,@TokenCode=@TokenCode,@ExpiryDate=@ExpiryDate",
                                                                                                    new SqlParameter("UserId", UserId),
                                                                                                    new SqlParameter("TokenCode", Guid.NewGuid().ToString() + UserId.ToString() + Guid.NewGuid().ToString()),
                                                                                                    new SqlParameter("ExpiryDate", DateTime.Now.AddDays(7)));

                            string Token = _objFriendFitDBEntity.Database.SqlQuery<string>("Select TokenCode from UserToken where UserId={0}", UserId).FirstOrDefault();
                            //mail sending after registration
                            if (objSignUpModelRequset.Email != null)
                            {
                                var SendingMessage = new MailMessage();
                                SendingMessage.To.Add(new MailAddress(objSignUpModelRequset.Email));  // replace with valid value
                                SendingMessage.From = new MailAddress("noreply@noti.fit");  // replace with valid value
                                SendingMessage.Subject = "Verify your email (noti.fit)";
                                SendingMessage.Body = "Hi,<br/>Congratulations on signing up to noti.fit, the fitness tracker that keeps you honest!<br/><br/>Please visit <a href='" + RegistrationUrl + Token + "'>" + RegistrationUrl + Token + "</a> <br/>  to verify your email address and activate your account, or copy the link into a browser if you can't open it from your email address.<br/><br/>Kind regards,<br/>The noti.fit team";
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
                                    await smtp.SendMailAsync(SendingMessage);
                                }


                                //track sent Email time for expiry time :

                                EmailTimeSaveModel objreq = new EmailTimeSaveModel();
                                objreq.UserId = UserId;
                                objreq.ResetMail = false;
                                objreq.VerifyMail = true;
                                objreq.MailSentTime = DateTime.Now.TimeOfDay;
                                EmailTrackerWrapper wrapper = new EmailTrackerWrapper();
                                wrapper.EmailTimeSave(objreq);

                                result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                result.Response.Message = "Please check your Email for further instructions";
                            }
                            else
                            {
                                FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/Content/ErrorLog.txt"), FileMode.Append, FileAccess.Write);
                                StreamWriter swr = new StreamWriter(fs);
                                swr.Write("Enter ur Exception Here");
                                swr.Close();
                                result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                                result.Response.Message = "This Mail Id is not registered";
                            }


                        //mail SMS after registration
                        if (objSignUpModelRequset.MobileNumber != null)
                        {                          
                            twilioModel tm = new twilioModel();
                            tm.countryCode = objSignUpModelRequset.CountryId;
                            tm.mobileNo = objSignUpModelRequset.MobileNumber;
                            tm.messagebody = "Hi " +objSignUpModelRequset.FirstName+ ", Welcome to noti.fit. Please confirm your mobile at <a href='" + RegistrationUrl + Token + "'>" + RegistrationUrl + Token + "</a> - If this wasn't you, ignore this SMS or decline at the link";
                            var SMSStatus = SMSCont.SendSMS(tm);                                       

                            //track sent Email time for expiry time :
                            EmailTimeSaveModel objreq = new EmailTimeSaveModel();
                            objreq.UserId = UserId;
                            objreq.ResetMail = false;
                            objreq.VerifyMail = true;
                            objreq.MailSentTime = DateTime.Now.TimeOfDay;
                            EmailTrackerWrapper wrapper = new EmailTrackerWrapper();
                            wrapper.EmailTimeSave(objreq);

                            result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                            result.Response.Message = "Please check your Mobile for further instructions";
                        }
                        else
                        {
                            FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/Content/ErrorLog.txt"), FileMode.Append, FileAccess.Write);
                            StreamWriter swr = new StreamWriter(fs);
                            swr.Write("Enter ur Exception Here");
                            swr.Close();
                            result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                            result.Response.Message = "This Mobile No. is not registered";
                        }
                        //
                        if (model > 0)
                            {
                                result.Response.Token = Token;
                                result.Response.UserId = UserId;
                                result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                result.Response.Message = "Check your email and confirm your account, you must be confirmed " + " " + "before you can log in.";
                            }
                            else
                            {
                                result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                                result.Response.Message = "The Data which you are providing it is in the wrong format";
                            }
                        }
                        else
                        {
                            result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Ambiguous);
                            result.Response.Message = "MailID elready Exist";
                        }
                    //}
                    //else
                    //{

                    //    result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                    //    result.Message = "Password Must contain at least one number and one uppercase and lowercase letter,  and atleast one special character and  must be in between 6 to 20 characters";

                    //}

                }
                catch (Exception ex)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
        [Route("ResendMailForSignUp")]
        public async Task<HttpResponseMessage> ResendRegistrationMaiil(string Email)
        {
            FResponse result = new FResponse();
            try
            {
               
               
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("Select Id from UserProfile where Email={0}", Email).FirstOrDefault();
                string UserToken = _objFriendFitDBEntity.Database.SqlQuery<string>("select TokenCode from UserToken where UserId={0}", UserId).FirstOrDefault();
                if (Email != null)
                {
                    var SendingMessage = new MailMessage();
                    SendingMessage.To.Add(new MailAddress(Email));  // replace with valid value
                    SendingMessage.From = new MailAddress("noreply@noti.fit");  // replace with valid value
                    SendingMessage.Subject = "Verify your email (noti.fit)";
                    SendingMessage.Body = "Hi,<br/>Congratulations on signing up to noti.fit, the fitness tracker that keeps you honest!<br/><br/>Please visit <a href='" + RegistrationUrl + UserToken + "'>" + RegistrationUrl + UserToken + "</a> <br/>  to verify your email address and activate your account, or copy the link into a browser if you can't open it from your email address.<br/><br/>Kind regards,<br/>The noti.fit team";
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
                        await smtp.SendMailAsync(SendingMessage);
                    }
                    //track sent Email time for expiry time :

                    EmailTimeSaveModel objreq = new EmailTimeSaveModel();
                    objreq.UserId = UserId;
                    objreq.ResetMail = false;
                    objreq.VerifyMail = true;
                    objreq.MailSentTime = DateTime.Now.TimeOfDay;
                    EmailTrackerWrapper wrapper = new EmailTrackerWrapper();
                    wrapper.EmailTimeSave(objreq);

                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Please check your Email for further instructions";
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpPost]
        [Route("ResendMailForForget")]
        public async Task<HttpResponseMessage> ResendMailForForget(string Email)
        {
            FResponse result = new FResponse();
            try
            {              

                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select Id from UserProfile where Email={0}", Email).FirstOrDefault();
                string UserToken = _objFriendFitDBEntity.Database.SqlQuery<string>("select TokenCode from UserToken where UserId={0}", UserId).FirstOrDefault();
                if (Email != null)
                {
                    var SendingMessage = new MailMessage();
                    SendingMessage.To.Add(new MailAddress(Email));  // replace with valid value
                    SendingMessage.From = new MailAddress("noreply@noti.fit");  // replace with valid value
                    SendingMessage.Subject = "Verify your email (noti.fit)";
                    SendingMessage.Body = "Hi,<br/>You've requested a reset of your noti.fit password. If you didn't make the request, please ignore this email and your password won't be reset.<br/><br/> You can reset your password by visiting <br/> <br/><a href='" + ChangePasswordUrl + UserToken + "'>" + ChangePasswordUrl + UserToken + "</a> <br/><br/>This email will be valid for the next 12 hours.,<br/><br/><br/><br/>Kind regards<br/>The noti.fit team";
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
                        await smtp.SendMailAsync(SendingMessage);
                    }
                    //track sent Email time for expiry time :

                    EmailTimeSaveModel objreq = new EmailTimeSaveModel();
                    objreq.UserId = UserId;
                    objreq.ResetMail = true;
                    objreq.VerifyMail = false;
                    objreq.MailSentTime = DateTime.Now.TimeOfDay;
                    EmailTrackerWrapper wrapper = new EmailTrackerWrapper();
                    wrapper.EmailTimeSave(objreq);
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Please check your Email for further instructions";
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
           
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public HttpResponseMessage VerifyEmail(string Token)
        {
            FResponse result = new FResponse();
            try
            {
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("Select UserId from UserToken where TokenCode={0}",Token).FirstOrDefault();
                //check link is valid or not

                TimeSpan mailSentTime = _objFriendFitDBEntity.Database.SqlQuery<TimeSpan>("SELECT TOP 1 MailSentTime FROM MailTimeLogs where UserId=@UserId ORDER BY Id DESC",
                                                                                            new SqlParameter("UserId", UserId)).FirstOrDefault();
                TimeSpan Nowtime = DateTime.Now.TimeOfDay;
                var TimeDiff = Nowtime - mailSentTime;
                if (TimeDiff.Hours > 12)
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                    result.Message = "Link is Expired";
                }
                else
                {
                    bool confirmation = _objFriendFitDBEntity.Database.SqlQuery<bool>("select EmailConfirmed from UserProfile where Id={0}", UserId).FirstOrDefault();

                    if (confirmation == true)
                    {
                        result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Message = "Your email address was verified successfully.” Or “Your phone number was verified successfully.";
                    }
                    else
                    {
                        int mailConfirmation = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Update UserProfile set EmailConfirmed=1 where Id={0}", UserId);
                        if (mailConfirmation > 0)
                        {
                            result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                            result.Message = "Mail Id has been Verified!!";
                        }
                        else
                        {
                            result.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                            result.Message = "Something went wrong,Please Try again";
                        }
                    }

                }

            }
            catch(Exception ex)
            {
               
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }   

        [HttpPost]
        [Route("EditProfile")]
        [SecureResource]
        public HttpResponseMessage UserEditProfile(EditProfileRequestModel objEditProfileRequestModel)
        {
            FResponse res = new FResponse();
            UserDetailsModelResponse result = new UserDetailsModelResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 userId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                result.Response.details = _objIUserSettings.EditUser(objEditProfileRequestModel);
                if(result.Response.details != null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Success!!";
                }
                else
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Response.Message = "UserId is not valid!!";
                }
            }
            catch(Exception ex)
            {                
                  _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpPost]
        [Route("UpdateUser")]
        [SecureResource]
        public HttpResponseMessage UserUpdateProfile(UserUpdateModelRequest objUserUpdateModelRequest)
        {
           FResponse result = new FResponse();
             try
                {
                    var headers = Request.Headers;
                    string token = headers.Authorization.Parameter.ToString();
                    Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                    var model = _objIUserSettings.UpdateUser(objUserUpdateModelRequest,UserId);
                    if (model>0)
                    {
                        result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        result.Message = "Your profile updated successfully!!";
                    }
                    else
                    {
                        result.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                        result.Message = "Data is invalid!!";
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
        [Route("UserUpdateImperialMatrics")]
        [SecureResource]
        public HttpResponseMessage UserUpdateImperialMatrics(ImperialUpdateModelRequest objImperialUpdateModelRequest)
        {
            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                var model = _objIUserSettings.UpdateUserimperial(objImperialUpdateModelRequest, UserId);
                if (model > 0)
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Your profile updated successfully!!";
                }
                else
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Message = "Data is invalid!!";
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
        [Route("CountryList")]
        public HttpResponseMessage CountryList()
        {
           CountryListModelResponse result = new CountryListModelResponse();
            try
            {
                result.Response.CountryList = _objFriendFitDBEntity.Database.SqlQuery<CountryList>("GetCountryList").ToList();
                if(result.Response.CountryList != null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Country List";
                }
                else
                {
                    _response = Request.CreateResponse(HttpStatusCode.NotFound, "No data in Country List");
                }
            }
            catch(Exception ex)
            {
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [HttpGet]
        [Route("ImperialMetricsList")]
        public HttpResponseMessage ImperialMetricsList()
        {
            ImperialMetricsListModelResponse result = new ImperialMetricsListModelResponse();
            try
            {
                result.Response.ImperialMetricsList = _objFriendFitDBEntity.Database.SqlQuery<ImperialMetricsList>("GetImperialMetricsList").ToList();
                if (result.Response.ImperialMetricsList != null)
                {
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Imperial Metrics List";
                }
                else
                {
                    _response = Request.CreateResponse(HttpStatusCode.NotFound, "No data in Imperial Metrics List");
                }
            }
            catch (Exception ex)
            {
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

        [SecureResource]
        [HttpPost]
        [Route("Logout")]
        public HttpResponseMessage Logout()
        {
            FResponse result = new FResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select userId from Token where TokenCode={0}", token).FirstOrDefault();

                //result.Response = _objIUserSettings.LogoutUser();
                int LogoutUser = _objIUserSettings.LogoutUser(UserId);

                if (result != null)
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Message = "Logout successfully....";
                    _response = Request.CreateResponse(HttpStatusCode.OK, result);

                }
                else
                {
                    result.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    result.Message = "Some Error Occurred...";
                    _response = Request.CreateResponse(HttpStatusCode.OK, result);

                }
            }
            catch (Exception ex)
            {
                result.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                result.Message = ex.ToString();
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, result);
            }
            return _response;
        }


        [HttpGet]
        [Route("ImperialMetrics")]
        [SecureResource]
        public HttpResponseMessage ImperialMetrics()
        {
            ImperialMetricsModelResponse result = new ImperialMetricsModelResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();


                ImperialMetrics _obj_IM = _objFriendFitDBEntity.Database.SqlQuery<ImperialMetrics>("GetImperialMetrics @UserId=@UserId",
                                                                     new SqlParameter("UserId", UserId)).FirstOrDefault();

             
                if (_obj_IM != null)
                {
                    result.Response.ImperialMetrics = _obj_IM;
                    result.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    result.Response.Message = "Imperial Metrics";
                }
                else
                {
                    _response = Request.CreateResponse(HttpStatusCode.NotFound, "No data in Imperial Metrics.");
                }
            }
            catch (Exception ex)
            {
                _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
            }
            _response = Request.CreateResponse(HttpStatusCode.OK, result);
            return _response;
        }

    }
}
