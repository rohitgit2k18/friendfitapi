using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/Notification")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationController : ApiController
    {
        // GET: Notification
        public IHttpActionResult Index()
        {

            return Ok();
        }

        [Route("Index")]
        [HttpPost]
        public IHttpActionResult Index(string Message)
        {
            string Messa = Notification(Message);
            return Ok();
        }

       
        public string Notification(string Message)
        {
            string response = "";
            try
            {
                string serverKey = "AAAACOf0tAc:APA91bFnpOrR4frj0mFe2yJVk8E_G4KC-5dWByZ_np6oIPNpRv0UJUCDT1ZmqBTj2s9dfN6op8WrlEcNHryro3uooNblWFklpWE1Q5U6DsxTgftArmry_TtXD0nPa9zZadwaLQPWaHRr";
                string senderId = "32931823423234";
                string deviceId = "/topics/all";
                string value = Message;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                string dd = System.DateTime.Now.ToString();
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = "Greetings",
                        title = "Augsburg",
                        sound = "Enabled"
                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        //using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        //{
                        //    using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        //    {
                        //        String sResponseFromServer = tReader.ReadToEnd();
                        //        response = sResponseFromServer;
                        //    }

                        //}
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())                      
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            response = tReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
            return response;
        }
    }
}
