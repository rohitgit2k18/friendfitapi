using FriendFit.API.Filters;
using FriendFit.Data;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/PushNotification")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PushNotificationController : ApiController
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        [Route("SendNotification_fcm")]
        [HttpPost]
        public NotificationResponse SendNotification_fcm(NotificationRequestModel ObjNotificationRequestModel)
        {
            NotificationResponse objNotificationResponse = new NotificationResponse();
            var serverKey = "AAAAvFQYgYA:APA91bGSoLVXzYCUNptgngQ-UctKIkgvOJ462Vvls7draV1P6OvQ1kHavjZZMAWhUYh_gXfDnH9duNT1QSwqH5KVy1PrL8rQN64xnByPZEonPbhW19pJRbDc-QrQB5wr_RWCjfVmNRFV";
            var senderId = "808864743808";
            string deviceId = ObjNotificationRequestModel.DeviceId;
            string DeviceType = ObjNotificationRequestModel.DeviceType;
            string MessageSend = ObjNotificationRequestModel.MessageBody; ;
            string Subject = ObjNotificationRequestModel.Subject;
            string sResponseFromServer = "";

            var objNotification = new
            {
                to = deviceId,
                priority = "high",
                content_available = true,
                data = new
                {
                    title = Subject,
                    body = MessageSend,
                    sound = "Enabled",
                    icon = "/firebase-logo.png",
                    status = true
                }
            };

            try
            {
                //for Ios
                Guid NotificationId = Guid.NewGuid();
                if (DeviceType.ToUpper() == "IOS")
                {
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";

                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(objNotification);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    sResponseFromServer = tReader.ReadToEnd();
                                    FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(sResponseFromServer);
                                    if (response.success == 1)
                                    {
                                        objNotificationResponse.Responses.Message = "succeeded";
                                        objNotificationResponse.Responses.StatusCode = 200;

                                    }
                                    else if (response.failure == 1)
                                    {
                                        objNotificationResponse.Responses.Message = "failed";
                                        objNotificationResponse.Responses.StatusCode = 400;
                                    }
                                }
                            }
                        }
                    }
                }
                //for Android
                else
                {
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";

                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(objNotification);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    sResponseFromServer = tReader.ReadToEnd();

                                    FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(sResponseFromServer);
                                    if (response.success == 1)
                                    {
                                        objNotificationResponse.Responses.Message = "succeeded";
                                        objNotificationResponse.Responses.StatusCode = 200;

                                    }
                                    else if (response.failure == 1)
                                    {
                                        objNotificationResponse.Responses.Message = "failed";
                                        objNotificationResponse.Responses.StatusCode = 400;
                                    }
                                }
                            }
                        }
                    }
                }
                // For ios
            }
            catch (Exception ex)
            {
                objNotificationResponse.Responses.Message = Convert.ToString(ex);
                objNotificationResponse.Responses.StatusCode = 501;
            }
            return objNotificationResponse;
        }

        [Route("SendNotification")]
        [HttpPost]       
        public string SendNotification(NotificationRequestModel ObjNotificationRequestModel)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("authorization", "Basic YWQzNzVhYzItNDM3YS00OTA3LTk0YmEtYjZjNGNhYTY0M2Rj");
            string MessageSend = ObjNotificationRequestModel.MessageBody;
            string NotificationTitle = ObjNotificationRequestModel.Subject;
            string deviceId = ObjNotificationRequestModel.DeviceId;
            string deviceType = ObjNotificationRequestModel.DeviceType;

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = "0c5b7cb6-a33d-453e-ada0-2c54d17bd7ba",
                contents = new { en = MessageSend },
                headings = new { en = NotificationTitle },
                include_player_ids = new string[] { deviceId }
            };

            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;
            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }
            System.Diagnostics.Debug.WriteLine(responseContent);
            return responseContent;
        }

        [Route("UpdatePlayerId")]
        [HttpPost]
        [SecureResource]
        public NotificationResponse UpdatePlayerId(string PlayerId)
        {
            NotificationResponse objUpdatePlayerId = new NotificationResponse();
            try
            {
                var headers = Request.Headers;
                string token = headers.Authorization.Parameter.ToString();
                Int64 UserId = _objFriendFitDBEntity.Database.SqlQuery<Int64>("select UserId from UserToken where TokenCode={0}", token).FirstOrDefault();

                var _UpdatePlayerId = _objFriendFitDBEntity.UserProfiles.Where(x => x.Id == UserId).FirstOrDefault();
                _UpdatePlayerId.DeviceId = PlayerId;
                _UpdatePlayerId.DeviceTypeId = 3;
                _objFriendFitDBEntity.Entry(_UpdatePlayerId).State = System.Data.Entity.EntityState.Modified;
                _objFriendFitDBEntity.SaveChanges();
                objUpdatePlayerId.Responses.Message = "Player Id is Updated Successfully.";
                objUpdatePlayerId.Responses.StatusCode = 200;
            }
            catch (Exception ex)
            {
                objUpdatePlayerId.Responses.Message = Convert.ToString(ex);
                objUpdatePlayerId.Responses.StatusCode = 501;
            }
            return objUpdatePlayerId;
        }
    }

    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FCMResult> results { get; set; }
    }
    public class FCMResult
    {
        public string message_id { get; set; }
    }
}



