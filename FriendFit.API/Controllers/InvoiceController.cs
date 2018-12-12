using FriendFit.Data;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.IRepository;
using FriendFit.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{

    [RoutePrefix("api/invoice")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InvoiceController : ApiController
    {       
        private IinvoiceRepository _objIinvoiceRepository;
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();
        public InvoiceController()
        {
            _objIinvoiceRepository = new invoiceRepository();
        }


        [HttpPost]
        [Route("TestInovice")]
        public string TestInovice(string Paymentid)
        {
            EmailModelAttachment tm = new EmailModelAttachment();
            var GetUserdetail = _objFriendFitDBEntity.UserProfiles.Where(x => x.Id == 44).FirstOrDefault();
            tm.ToEmail = "shrawan.choubey@mail.vinove.com";
            if (GetUserdetail.LastName != null)
            {
                tm.CustomerName = GetUserdetail.FirstName + ' ' + GetUserdetail.LastName;
            }
            else
            {
                tm.CustomerName = GetUserdetail.FirstName;
            }
            tm.PayPalPaymentId = Paymentid;
           // tm.messagebody = "Hi " + GetUserdetail.FirstName;
            tm.Subject = "Notifit Inovice";
            tm.FileURL = "~/Invoice/" + Paymentid + ".pdf";
            tm.messagebody = _objIinvoiceRepository.DownloadApplicationPDF(tm);         
            var SMSStatus = _objIinvoiceRepository.SendEmailWithAttachment(tm);
            return SMSStatus;
        }
     
    }
}
