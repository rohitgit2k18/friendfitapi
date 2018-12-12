using FriendFit.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;


namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/Email")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class _EmailController : ApiController
    {
        [HttpPost]
        [Route("SendEmail")]
        public async Task<IHttpActionResult> SendEmail(EmailModel model)
        {
            try
            {
                var SendingMessage = new MailMessage();
                SendingMessage.To.Add(new MailAddress(model.ToEmail));  // replace with valid value
                SendingMessage.From = new MailAddress("noreply@noti.fit");  // replace with valid value
                SendingMessage.Subject = model.Subject;
                SendingMessage.Body = model.messagebody;
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
                   var dd= smtp.SendMailAsync(SendingMessage);
                    await smtp.SendMailAsync(SendingMessage);
                }
            }
            catch (Exception ex)
            {
            }
            return Ok();
        }


        [HttpPost]
        [Route("SendEmailWithAttachment")]
        public async Task<IHttpActionResult> SendEmailWithAttachment(EmailModel model)
        {
            try
            {
                string PDFURL="dgdfggdf.pdf";       
                var SendingMessage = new MailMessage();
                SendingMessage.To.Add(new MailAddress(model.ToEmail));  // replace with valid value
                SendingMessage.From = new MailAddress("noreply@noti.fit");  // replace with valid value
                SendingMessage.Subject = model.Subject;
                SendingMessage.Body = model.messagebody;               
                Attachment at = new Attachment(System.Web.HttpContext.Current.Server.MapPath(PDFURL));
                SendingMessage.Attachments.Add(at);
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
            }
            catch (Exception ex)
            {
            }
            return Ok();
        }
    }
}
