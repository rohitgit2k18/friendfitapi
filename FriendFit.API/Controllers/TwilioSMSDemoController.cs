using FriendFit.API.Models;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/twilio")]
    public class TwilioSMSDemoController : ApiController
    {
        [HttpPost]
        [Route("SendSMS")]
        public async Task<IHttpActionResult> SendSMS(twilioModel model)
        {
            var tosms = "";
            if (model.countryCode!=null)
            {
                tosms = "+" + model.countryCode + model.mobileNo;
            }
            else
            {
                tosms = "+" + model.mobileNo;
            }
            try
            {
                // Find your Account Sid and Token at twilio.com/console
                var accountSid = "AC59287b3995152a1dc20fa6a9238fbe6b";
                var authToken = "34a067180051d73fb55ae8dbbea53441";
                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: model.messagebody,
                    from: new PhoneNumber("+17014014499"),
                    to: new PhoneNumber(tosms)
                );
                Console.Write(message.Sid);
                //var json = new JavaScriptSerializer().Serialize(message);
                //return message.Sid;
            }
            catch (Exception ex)
            {
            }
            return Ok();
        }   
    }
}