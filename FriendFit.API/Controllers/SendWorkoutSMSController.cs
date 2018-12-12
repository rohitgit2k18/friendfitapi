using FriendFit.API.Models;
using FriendFit.Data;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.IRepository;
using FriendFit.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/SendWorkoutSMS")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SendWorkoutSMSController : ApiController
    {
        private HttpResponseMessage _response;
        private ISendWorkOutSMSRepository _objISendWorkOutSMSRepository;
        private TwilioSMSDemoController TwilioSMS = new TwilioSMSDemoController();
        private PushNotificationController SendPushNotification = new PushNotificationController();
        private _EmailController SendEmail = new _EmailController();
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();
        string WorkoutListURL = System.Configuration.ConfigurationManager.AppSettings["FriendFitWorkoutListPage"];

        public SendWorkoutSMSController()
        {
            _objISendWorkOutSMSRepository = new SendWorkOutSMSRepository();
        }

        //Before Workout To User
        [HttpPost]
        [Route("SendEmailBeforeWorkoutToUser")]
        public HttpResponseMessage SendEmailBeforeWorkoutToUser(string Cron_Minutely)
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            DateTime d = DateTime.Now;
            string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm
            if (ModelState.IsValid)
            {
                try
                {
                    ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendEmailBeforeWorkoutToUser();
                    foreach (var item in ListofUserSMS.Response.UserInvitedList)
                    {
                        UserDetailsModel GetUserSMSPlan = _objFriendFitDBEntity.Database.SqlQuery<UserDetailsModel>("UserEditProfile @UserId=@UserId",
                                                                     new SqlParameter("UserId", item.UserId)).FirstOrDefault();
                        if (GetUserSMSPlan.ExpiryDate_App!=null)
                        {
                            DateTime TodayDate = DateTime.Now;
                            string Str_TodayDay = String.Format("{0:dd/MM/yyyy}", TodayDate);
                            DateTime dt_CurrentDate = DateTime.ParseExact(Str_TodayDay, "dd/MM/yyyy", null);
                            DateTime dt_ExpiryDate = DateTime.ParseExact(GetUserSMSPlan.ExpiryDate_App, "dd/MM/yyyy", null);

                            if (dt_CurrentDate < dt_ExpiryDate)
                            {
                                if (CurrentTime == item.SendTime_TextMe)
                                {
                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
                                    IsSMSSend.SendEmailToUser = 1;
                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
                                    _objFriendFitDBEntity.SaveChanges();

                                    Models.EmailModel tm = new Models.EmailModel();
                                    tm.ToEmail = item.Email;
                                    tm.Subject = "Start Your Workout";
                                    tm.messagebody = "Hi " + item.FirstName + ", time to work out " + item.Description + " at " + item.StartTime_Actual + " .";
                                    var SMSStatus = SendEmail.SendEmail(tm);
                                }
                                ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                ListofUserSMS.Response.Message = "Email Sended to User successfully!";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
                //  _response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                ModelState.AddModelError("", "One or more errors occurred.");
            }
            return _response;
        }

        [HttpPost]
        [Route("SendSMSBeforeWorkoutToUser")]
        public HttpResponseMessage SendSMSBeforeWorkoutToUser(string Cron_Minutely)
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            DateTime d = DateTime.Now;
            string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm

            if (ModelState.IsValid)
            {
                try
                {
                    ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendSMSBeforeWorkoutToUser();
                    foreach (var item in ListofUserSMS.Response.UserInvitedList)
                    {
                        UserDetailsModel GetUserSMSPlan = _objFriendFitDBEntity.Database.SqlQuery<UserDetailsModel>("UserEditProfile @UserId=@UserId",
                                                                      new SqlParameter("UserId", item.UserId)).FirstOrDefault();

                        if (GetUserSMSPlan.ExpiryDate_SMS!=null)
                        {
                            DateTime TodayDate = DateTime.Now;
                            string Str_TodayDay = String.Format("{0:dd/MM/yyyy}", TodayDate);
                            DateTime dt_CurrentDate = DateTime.ParseExact(Str_TodayDay, "dd/MM/yyyy", null);
                            DateTime dt_ExpiryDate = DateTime.ParseExact(GetUserSMSPlan.ExpiryDate_SMS, "dd/MM/yyyy", null);

                            if (dt_CurrentDate < dt_ExpiryDate)
                            {
                                if (CurrentTime == item.SendTime_TextMe)
                                {
                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
                                    IsSMSSend.SendSMSToUser = 1;
                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
                                    _objFriendFitDBEntity.SaveChanges();

                                    twilioModel tm = new twilioModel();
                                    tm.mobileNo = item.MobileNumber;
                                    tm.countryCode = Convert.ToInt64(item.CountryId);
                                    tm.messagebody = "Hi " + item.FirstName + ", time to work out " + item.Description + " at " + item.StartTime_Actual + " .";
                                    var SMSStatus = TwilioSMS.SendSMS(tm);
                                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                    ListofUserSMS.Response.Message = "SMS Sended to User successfully!";
                                }                              
                            }
                        }                     
                    }                   
                }
                catch (Exception ex)
                {
                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
                //   _response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                ModelState.AddModelError("", "One or more errors occurred.");
            }
            return _response;
        }

        [HttpPost]
        [Route("SendNotificationBeforeWorkoutToUser")]
        public HttpResponseMessage SendNotificationBeforeWorkoutToUser(string Cron_Minutely)
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            DateTime d = DateTime.Now;
            //string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture); // this show  11:12 Pm
            string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm

            if (ModelState.IsValid)
            {
                try
                {
                    ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendNotificationBeforeWorkoutToUser();
                    foreach (var item in ListofUserSMS.Response.UserInvitedList)
                    {
                        UserDetailsModel GetUserSMSPlan = _objFriendFitDBEntity.Database.SqlQuery<UserDetailsModel>("UserEditProfile @UserId=@UserId",
                                               new SqlParameter("UserId", item.UserId)).FirstOrDefault();

                        if (GetUserSMSPlan.ExpiryDate_App!=null)
                        {
                            DateTime TodayDate = DateTime.Now;
                            string Str_TodayDay = String.Format("{0:dd/MM/yyyy}", TodayDate);
                            DateTime dt_CurrentDate = DateTime.ParseExact(Str_TodayDay, "dd/MM/yyyy", null);
                            DateTime dt_ExpiryDate = DateTime.ParseExact(GetUserSMSPlan.ExpiryDate_App, "dd/MM/yyyy", null);

                            if (dt_CurrentDate < dt_ExpiryDate)
                            {
                                if (CurrentTime == item.SendTime_TextMe)
                                {
                                    var GetuserDetails = _objFriendFitDBEntity.UserProfiles.Where(x => x.Id == item.UserId).FirstOrDefault();
                                    string DeviceType = "";
                                    if (GetuserDetails.DeviceTypeId != null)
                                    {
                                        var _DeviceType = _objFriendFitDBEntity.DeviceMasters.Where(x => x.Id == GetuserDetails.DeviceTypeId).FirstOrDefault();
                                        DeviceType = _DeviceType.DeviceName;
                                        NotificationRequestModel ObjNotificationRequest = new NotificationRequestModel();
                                        ObjNotificationRequest.DeviceId = GetuserDetails.DeviceId;
                                        ObjNotificationRequest.DeviceType = DeviceType;
                                        ObjNotificationRequest.Subject = "Start Your Workout";
                                        ObjNotificationRequest.MessageBody = "Hi " + item.FirstName + ", time to work out " + item.Description + " at " + item.StartTime_Actual + " .";

                                        var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
                                        IsSMSSend.SendNotificationToUser = 1;
                                        _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
                                        _objFriendFitDBEntity.SaveChanges();
                                        var NotificationStatus = SendPushNotification.SendNotification(ObjNotificationRequest);
                                        ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                        ListofUserSMS.Response.Message = "Notification Sended to User successfully!";
                                    }
                                }
                            }
                        }                       
                    }                   
                }
                catch (Exception ex)
                {
                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
                //   _response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                ModelState.AddModelError("", "One or more errors occurred.");
            }
            return _response;
        }

        //=================================================================
        //Before Workout To Friends
        //[HttpPost]
        //[Route("SendEmailBeforeWorkoutToFriends")]
        //public HttpResponseMessage SendEmailBeforeWorkoutToFriends(string Cron_Minutely)
        //{
        //    ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
        //    DateTime d = DateTime.Now;
        //    string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendEmailBeforeWorkoutToFriends();
        //            foreach (var item in ListofUserSMS.Response.UserInvitedList)
        //            {
        //                var GetFriendsSMSPlan = _objFriendFitDBEntity.FriendsInvitations.Where(a => a.DeliveryTypeId == 1 && a.UserId == item.UserId && a.PaymentDone == 1
        //                && a.IsActive == true && a.IsRowActive == true).OrderByDescending(x => x.Id).ToList();

        //                if (GetFriendsSMSPlan.Count > 0)
        //                {
        //                    foreach (var item_GetFriendsSMSPlan in GetFriendsSMSPlan)
        //                    {
        //                        DateTime dt_CurrentDate = DateTime.Today.Date;
        //                        DateTime dt_ExpiryDate = item_GetFriendsSMSPlan.ExpiryDate.Value;

        //                        if (dt_CurrentDate < dt_ExpiryDate)
        //                        {
        //                            if (CurrentTime == item.SendTime_TextFriend)
        //                            {
        //                                var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
        //                                IsSMSSend.SendEmailToFriends = 1;
        //                                _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
        //                                _objFriendFitDBEntity.SaveChanges();

        //                                Models.EmailModel tm = new Models.EmailModel();
        //                                tm.ToEmail = item_GetFriendsSMSPlan.Email;
        //                                tm.Subject = "View Your Friend Workout";
        //                                tm.messagebody = "Hi " + item_GetFriendsSMSPlan.FriendsName + ", Your Friend " + item.FirstName + " time to work out " + item.Description + " at " + item.StartTime_Actual + "- go to <a href=" + WorkoutListURL + ">" + WorkoutListURL + "</a> to view your friend status.";
        //                                var SMSStatus = SendEmail.SendEmail(tm);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            ListofUserSMS.Response.Message = "Email Sended to User successfully!";
        //        }
        //        catch (Exception ex)
        //        {
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
        //        }
        //        //_response = Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "One or more errors occurred.");
        //    }
        //    return _response;
        //}


        //[HttpPost]
        //[Route("SendSMSBeforeWorkoutToFriends")]
        //public HttpResponseMessage SendSMSBeforeWorkoutToFriends(string Cron_Minutely)
        //{
        //    ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
        //    DateTime d = DateTime.Now;
        //    string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture); // this show  11:12 Pm

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendSMSBeforeWorkoutToFriends();
        //            foreach (var item in ListofUserSMS.Response.UserInvitedList)
        //            {
        //                //if (item.StatusId == 2)
        //                //{
        //                var GetFriendsSMSPlan = _objFriendFitDBEntity.FriendsInvitations.Where(a => a.DeliveryTypeId == 2 && a.UserId == item.UserId && a.PaymentDone == 1
        //               && a.IsActive == true && a.IsRowActive == true).OrderByDescending(x => x.Id).ToList();

        //                if (GetFriendsSMSPlan.Count > 0)
        //                {
        //                    foreach (var item_GetFriendsSMSPlan in GetFriendsSMSPlan)
        //                    {
        //                        DateTime dt_CurrentDate = DateTime.Today.Date;
        //                        DateTime dt_ExpiryDate = item_GetFriendsSMSPlan.ExpiryDate.Value;

        //                        if (dt_CurrentDate < dt_ExpiryDate)
        //                        {
        //                            if (CurrentTime == item.SendTime_TextMe)
        //                            {
        //                                var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
        //                                IsSMSSend.SendSMSToFriends = 1;
        //                                _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
        //                                _objFriendFitDBEntity.SaveChanges();

        //                                twilioModel tm = new twilioModel();
        //                                tm.mobileNo = item_GetFriendsSMSPlan.MobileNumber;
        //                                tm.countryCode = Convert.ToInt64(item.CountryId);
        //                                tm.messagebody = "Hi" + item_GetFriendsSMSPlan.FriendsName + ", Your Friend " + item.FirstName + " time to work out" + item.Description + " at " + item.StartTime_Actual + "- go to" + WorkoutListURL + "to view your friend status.";
        //                                var SMSStatus = TwilioSMS.SendSMS(tm);
        //                            }
        //                        }
        //                    }
        //                    //}
        //                }
        //            }
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            ListofUserSMS.Response.Message = "SMS Sended to User successfully!";
        //        }
        //        catch (Exception ex)
        //        {
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
        //        }
        //        //    _response = Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "One or more errors occurred.");
        //    }
        //    return _response;
        //}


        //[HttpPost]
        //[Route("SendNotificationBeforeWorkoutToFriends")]
        //public HttpResponseMessage SendNotificationBeforeWorkoutToFriends(string Cron_Minutely)
        //{
        //    ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
        //    DateTime d = DateTime.Now;
        //    string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm


        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendNotificationBeforeWorkoutToFriends();
        //            foreach (var item in ListofUserSMS.Response.UserInvitedList)
        //            {
        //                //if (item.StatusId == 2)
        //                //{
        //                    var GetFriendsSMSPlan = _objFriendFitDBEntity.FriendsInvitations.Where(a => a.DeliveryTypeId == 1 && a.UserId == item.UserId && a.PaymentDone == 1
        //                && a.IsActive == true && a.IsRowActive == true).OrderByDescending(x => x.Id).ToList();

        //                    if (GetFriendsSMSPlan.Count > 0)
        //                    {
        //                        foreach (var item_GetFriendsSMSPlan in GetFriendsSMSPlan)
        //                        {
        //                            DateTime dt_CurrentDate = DateTime.Today.Date;
        //                            DateTime dt_ExpiryDate = item_GetFriendsSMSPlan.ExpiryDate.Value;

        //                            if (dt_CurrentDate < dt_ExpiryDate)
        //                            {
        //                                if (CurrentTime == item.SendTime_TextMe)
        //                                {
        //                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
        //                                    IsSMSSend.SendNotificationToFriends = 1;
        //                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
        //                                    _objFriendFitDBEntity.SaveChanges();

        //                                    var GetuserDetails = _objFriendFitDBEntity.UserProfiles.Where(x => x.Id == item.UserId).FirstOrDefault();
        //                                    string DeviceType = "";
        //                                    if (GetuserDetails.DeviceTypeId != null)
        //                                    {
        //                                        var _DeviceType = _objFriendFitDBEntity.DeviceMasters.Where(x => x.Id == GetuserDetails.DeviceTypeId).FirstOrDefault();
        //                                        DeviceType = _DeviceType.DeviceName;
        //                                        NotificationRequestModel ObjNotificationRequest = new NotificationRequestModel();
        //                                        ObjNotificationRequest.DeviceId = GetuserDetails.DeviceId;
        //                                        ObjNotificationRequest.DeviceType = DeviceType;
        //                                        ObjNotificationRequest.Subject = "View Your Friend Workout";
        //                                        ObjNotificationRequest.MessageBody = "Hi " + item_GetFriendsSMSPlan.FriendsName + ", Your Friend " + item.FirstName + " time to work out" + item.Description + " at " + item.StartTime_Actual + "- go to" + WorkoutListURL + "to view your friend status.";
        //                                        var NotificationStatus = SendPushNotification.SendNotification(ObjNotificationRequest);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //               // }
        //            }
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            ListofUserSMS.Response.Message = "Notification Sended to User successfully!";
        //        }
        //        catch (Exception ex)
        //        {
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
        //        }
        //        //  _response = Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "One or more errors occurred.");
        //    }
        //    return _response;
        //}


        //=================================================================
        //To User When Workout Missed
        [HttpPost]
        [Route("SendEmailToUserWhenWorkoutMissed")]
        public HttpResponseMessage SendEmailToUserWhenWorkoutMissed(string Cron_Minutely)
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            DateTime d = DateTime.Now;
            string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm

            if (ModelState.IsValid)
            {
                try
                {
                    ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendEmailToUserWhenWorkoutMissed();
                    foreach (var item in ListofUserSMS.Response.UserInvitedList)
                    {
                        UserDetailsModel GetUserSMSPlan = _objFriendFitDBEntity.Database.SqlQuery<UserDetailsModel>("UserEditProfile @UserId=@UserId",
                                                                    new SqlParameter("UserId", item.UserId)).FirstOrDefault();

                        if (GetUserSMSPlan.ExpiryDate_App!=null)
                        {
                            DateTime TodayDate = DateTime.Now;
                            string Str_TodayDay = String.Format("{0:dd/MM/yyyy}", TodayDate);
                            DateTime dt_CurrentDate = DateTime.ParseExact(Str_TodayDay, "dd/MM/yyyy", null);
                            DateTime dt_ExpiryDate = DateTime.ParseExact(GetUserSMSPlan.ExpiryDate_App, "dd/MM/yyyy", null);

                            if (dt_CurrentDate < dt_ExpiryDate)
                            {
                                if (CurrentTime == item.MissedTime_TextMe)
                                {
                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
                                    IsSMSSend.EmailToUserWhenMissed = 1;
                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
                                    _objFriendFitDBEntity.SaveChanges();
                                    Models.EmailModel tm = new Models.EmailModel();
                                    tm.ToEmail = item.Email;
                                    tm.Subject = "Workout Missed";
                                    tm.messagebody = "Hi " + item.FirstName + ", your work out missed " + item.Description + " at " + item.StartTime_Actual + " .";
                                    var SMSStatus = SendEmail.SendEmail(tm);
                                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                    ListofUserSMS.Response.Message = "Email Sended to User successfully!";
                                }
                            }
                        }                                             
                    }
                }
                catch (Exception ex)
                {
                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
                //    _response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                ModelState.AddModelError("", "One or more errors occurred.");
            }
            return _response;
        }

        [HttpPost]
        [Route("SendSMSToUserWhenWorkoutMissed")]
        public HttpResponseMessage SendSMSToUserWhenWorkoutMissed(string Cron_Minutely)
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            DateTime d = DateTime.Now;
            string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm

            if (ModelState.IsValid)
            {
                try
                {
                    ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendSMSToUserWhenWorkoutMissed();
                    foreach (var item in ListofUserSMS.Response.UserInvitedList)
                    {
                        UserDetailsModel GetUserSMSPlan = _objFriendFitDBEntity.Database.SqlQuery<UserDetailsModel>("UserEditProfile @UserId=@UserId",
                                                                    new SqlParameter("UserId", item.UserId)).FirstOrDefault();

                        if (GetUserSMSPlan.ExpiryDate_SMS!=null)
                        {
                            DateTime TodayDate = DateTime.Now;
                            string Str_TodayDay = String.Format("{0:dd/MM/yyyy}", TodayDate);
                            DateTime dt_CurrentDate = DateTime.ParseExact(Str_TodayDay, "dd/MM/yyyy", null);
                            DateTime dt_ExpiryDate = DateTime.ParseExact(GetUserSMSPlan.ExpiryDate_SMS, "dd/MM/yyyy", null);

                            if (dt_CurrentDate < dt_ExpiryDate)
                            {
                                if (CurrentTime == item.MissedTime_TextMe)
                                {
                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
                                    IsSMSSend.SMSToUserWhenMissed = 1;
                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
                                    _objFriendFitDBEntity.SaveChanges();

                                    twilioModel tm = new twilioModel();
                                    tm.mobileNo = item.MobileNumber;
                                    tm.countryCode = Convert.ToInt64(item.CountryId);
                                    tm.messagebody = "Hi " + item.FirstName + ", your work out missed " + item.Description + " at " + item.StartTime_Actual +" .";
                                     //   "- go to" + WorkoutListURL + "to view their status.";
                                    var SMSStatus = TwilioSMS.SendSMS(tm);
                                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                    ListofUserSMS.Response.Message = "SMS Sended to User successfully!";
                                }
                            }                           
                        }                       
                    }
                }
                catch (Exception ex)
                {
                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
                //   _response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                ModelState.AddModelError("", "One or more errors occurred.");
            }
            return _response;
        }

        [HttpPost]
        [Route("SendNotificationToUserWhenWorkoutMissed")]
        public HttpResponseMessage SendNotificationToUserWhenWorkoutMissed(string Cron_Minutely)
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            DateTime d = DateTime.Now;
            string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm

            if (ModelState.IsValid)
            {
                try
                {
                    ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendNotificationToUserWhenWorkoutMissed();
                    foreach (var item in ListofUserSMS.Response.UserInvitedList)
                    {
                        UserDetailsModel GetUserSMSPlan = _objFriendFitDBEntity.Database.SqlQuery<UserDetailsModel>("UserEditProfile @UserId=@UserId",
                                                    new SqlParameter("UserId", item.UserId)).FirstOrDefault();

                        if (GetUserSMSPlan.ExpiryDate_App!=null)
                        {
                            DateTime TodayDate = DateTime.Now;
                            string Str_TodayDay = String.Format("{0:dd/MM/yyyy}", TodayDate);
                            DateTime dt_CurrentDate = DateTime.ParseExact(Str_TodayDay, "dd/MM/yyyy", null);
                            DateTime dt_ExpiryDate = DateTime.ParseExact(GetUserSMSPlan.ExpiryDate_App, "dd/MM/yyyy", null);

                            if (dt_CurrentDate < dt_ExpiryDate)
                            {
                                if (CurrentTime == item.MissedTime_TextMe)
                                {
                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
                                    IsSMSSend.NotificationToUserWhenMissed = 1;
                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
                                    _objFriendFitDBEntity.SaveChanges();

                                    var GetuserDetails = _objFriendFitDBEntity.UserProfiles.Where(x => x.Id == item.UserId).FirstOrDefault();
                                    string DeviceType = "";
                                    if (GetuserDetails.DeviceTypeId != null)
                                    {
                                        var _DeviceType = _objFriendFitDBEntity.DeviceMasters.Where(x => x.Id == GetuserDetails.DeviceTypeId).FirstOrDefault();
                                        DeviceType = _DeviceType.DeviceName;
                                        NotificationRequestModel ObjNotificationRequest = new NotificationRequestModel();
                                        ObjNotificationRequest.DeviceId = GetuserDetails.DeviceId;
                                        ObjNotificationRequest.DeviceType = DeviceType;
                                        ObjNotificationRequest.Subject = "Workout Missed";
                                        ObjNotificationRequest.MessageBody = "Hi " + item.FirstName + ", your work out missed " + item.Description + " at " + item.StartTime_Actual + " .";
                                        var NotificationStatus = SendPushNotification.SendNotification(ObjNotificationRequest);
                                        ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                        ListofUserSMS.Response.Message = "Notification Sended to User successfully!";
                                    }
                                }
                            }                            
                        }                    
                    }
                }
                catch (Exception ex)
                {
                    ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
                }
                //   _response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                ModelState.AddModelError("", "One or more errors occurred.");
            }
            return _response;
        }


        //=================================================================
        //To Friends When Workout Missed
        //[HttpPost]
        //[Route("SendEmailFriendsWhenWorkoutMissed")]
        //public HttpResponseMessage SendEmailFriendsWhenWorkoutMissed(string Cron_Minutely)
        //{
        //    ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
        //    DateTime d = DateTime.Now;
        //    string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendEmailFriendsWhenWorkoutMissed();
        //            foreach (var item in ListofUserSMS.Response.UserInvitedList)
        //            {
        //                if (item.StatusId == 2)
        //                {
        //                    var GetFriendsSMSPlan = _objFriendFitDBEntity.FriendsInvitations.Where(a => a.DeliveryTypeId == 1 && a.UserId == item.UserId
        //                    && a.PaymentDone == 1 && a.IsActive == true && a.IsRowActive == true).OrderByDescending(x => x.Id).ToList();

        //                    if (GetFriendsSMSPlan.Count > 0)
        //                    {
        //                        foreach (var item_GetFriendsSMSPlan in GetFriendsSMSPlan)
        //                        {
        //                            DateTime dt_CurrentDate = DateTime.Today.Date;
        //                            DateTime dt_ExpiryDate = item_GetFriendsSMSPlan.ExpiryDate.Value;

        //                            if (dt_CurrentDate < dt_ExpiryDate)
        //                            {
        //                                if (CurrentTime == item.MissedTime_TextFriend)
        //                                {
        //                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
        //                                    IsSMSSend.SendSMSToFriends = 1;
        //                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
        //                                    _objFriendFitDBEntity.SaveChanges();

        //                                    Models.EmailModel tm = new Models.EmailModel();
        //                                    tm.ToEmail = item.Email;
        //                                    tm.Subject = "Workout Missed by your Friend";
        //                                    tm.messagebody = "Hi " + item_GetFriendsSMSPlan.FriendsName + ", Your Friend " + item.FirstName + " missed his " + item.Description + " workout at" + item.StartTime_Actual + " - go to" + WorkoutListURL + "to view their status.";
        //                                    var SMSStatus = SendEmail.SendEmail(tm);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //                ListofUserSMS.Response.Message = "Email Sended to User successfully!";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
        //        }
        //        //   _response = Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "One or more errors occurred.");
        //    }
        //    return _response;
        //}

        //[HttpPost]
        //[Route("SendSMSFriendsWhenWorkoutMissed")]
        //public HttpResponseMessage SendSMSFriendsWhenWorkoutMissed(string Cron_Minutely)
        //{
        //    ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
        //    DateTime d = DateTime.Now;
        //    string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm


        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendSMSFriendsWhenWorkoutMissed();
        //            foreach (var item in ListofUserSMS.Response.UserInvitedList)
        //            {
        //                if (item.StatusId == 2)
        //                {
        //                    var GetFriendsSMSPlan = _objFriendFitDBEntity.FriendsInvitations.Where(a => a.DeliveryTypeId == 2 && a.UserId == item.UserId
        //                && a.PaymentDone == 1 && a.IsActive == true && a.IsRowActive == true).OrderByDescending(x => x.Id).ToList();

        //                    if (GetFriendsSMSPlan.Count > 0)
        //                    {
        //                        foreach (var item_GetFriendsSMSPlan in GetFriendsSMSPlan)
        //                        {
        //                            DateTime dt_CurrentDate = DateTime.Today.Date;
        //                            DateTime dt_ExpiryDate = item_GetFriendsSMSPlan.ExpiryDate.Value;

        //                            if (dt_CurrentDate < dt_ExpiryDate)
        //                            {
        //                                if (CurrentTime == item.MissedTime_TextFriend)
        //                                {
        //                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
        //                                    IsSMSSend.SendSMSToFriends = 1;
        //                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
        //                                    _objFriendFitDBEntity.SaveChanges();

        //                                    twilioModel tm = new twilioModel();
        //                                    tm.mobileNo = item.MobileNumber;
        //                                    tm.countryCode = Convert.ToInt64(item.CountryId);
        //                                    tm.messagebody = "Hi " + item_GetFriendsSMSPlan.FriendsName + ", Your Friend " + item.FirstName + " missed his " + item.Description + " workout at" + item.StartTime_Actual + " - go to" + WorkoutListURL + "to view their status.";
        //                                    var SMSStatus = TwilioSMS.SendSMS(tm);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //                ListofUserSMS.Response.Message = "SMS Sended to User successfully!";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
        //        }
        //        //  _response = Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "One or more errors occurred.");
        //    }
        //    return _response;
        //}

        //[HttpPost]
        //[Route("SendNotificationFriendsWhenWorkoutMissed")]
        //public HttpResponseMessage SendNotificationFriendsWhenWorkoutMissed(string Cron_Minutely)
        //{
        //    ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
        //    DateTime d = DateTime.Now;
        //    string CurrentTime = d.ToString("hh:mmtt", CultureInfo.InvariantCulture).Replace("01:", "1:").Replace("02:", "2:").Replace("03:", "3:").Replace("04:", "4:").Replace("05:", "5:").Replace("06:", "6:").Replace("07:", "7:").Replace("08:", "8:").Replace("09:", "9:"); // this show  11:12 Pm


        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ListofUserSMS.Response.UserInvitedList = _objISendWorkOutSMSRepository.SendNotificationFriendsWhenWorkoutMissed();
        //            foreach (var item in ListofUserSMS.Response.UserInvitedList)
        //            {
        //                if (item.StatusId == 2)
        //                {
        //                    var GetFriendsSMSPlan = _objFriendFitDBEntity.FriendsInvitations.Where(a => a.DeliveryTypeId == 1 && a.UserId == item.UserId
        //                    && a.PaymentDone == 1 && a.IsActive == true && a.IsRowActive == true).OrderByDescending(x => x.Id).ToList();

        //                    if (GetFriendsSMSPlan.Count > 0)
        //                    {
        //                        foreach (var item_GetFriendsSMSPlan in GetFriendsSMSPlan)
        //                        {
        //                            DateTime dt_CurrentDate = DateTime.Today.Date;
        //                            DateTime dt_ExpiryDate = item_GetFriendsSMSPlan.ExpiryDate.Value;

        //                            if (dt_CurrentDate < dt_ExpiryDate)
        //                            {
        //                                if (CurrentTime == item.MissedTime_TextFriend)
        //                                {
        //                                    var IsSMSSend = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.WorkoutId).FirstOrDefault();
        //                                    IsSMSSend.SendSMSToFriends = 1;
        //                                    _objFriendFitDBEntity.Entry(IsSMSSend).State = System.Data.Entity.EntityState.Modified;
        //                                    _objFriendFitDBEntity.SaveChanges();

        //                                    var GetuserDetails = _objFriendFitDBEntity.UserProfiles.Where(x => x.Id == item.UserId).FirstOrDefault();
        //                                    string DeviceType = "";
        //                                    if (GetuserDetails.DeviceTypeId != null)
        //                                    {
        //                                        var _DeviceType = _objFriendFitDBEntity.DeviceMasters.Where(x => x.Id == GetuserDetails.DeviceTypeId).FirstOrDefault();
        //                                        DeviceType = _DeviceType.DeviceName;
        //                                        NotificationRequestModel ObjNotificationRequest = new NotificationRequestModel();
        //                                        ObjNotificationRequest.DeviceId = GetuserDetails.DeviceId;
        //                                        ObjNotificationRequest.DeviceType = DeviceType;
        //                                        ObjNotificationRequest.Subject = "Workout Missed by your Friend";
        //                                        ObjNotificationRequest.MessageBody = "Hi " + item_GetFriendsSMSPlan.FriendsName + ", Your Friend " + item.FirstName + " missed his " + item.Description + " workout at" + item.StartTime_Actual + " - go to" + WorkoutListURL + "to view their status.";
        //                                        var NotificationStatus = SendPushNotification.SendNotification(ObjNotificationRequest);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //                        ListofUserSMS.Response.Message = "Notification Sended to User successfully!";
        //                    }
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            ListofUserSMS.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            _response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occurred");
        //        }
        //        //   _response = Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "One or more errors occurred.");
        //    }
        //    return _response;
        //}
    }
}


