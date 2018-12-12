using FriendFit.Data.ApiModel.APIRequestModel;
using System;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.UI;

namespace FriendFit.Data.IRepository
{
    public interface IinvoiceRepository
    {
        string DownloadApplicationPDF(EmailModelAttachment EMA);
        string SendEmailWithAttachment(EmailModelAttachment EMA);
        string DownloadApplicationPDF_Friend(EmailModelAttachment EMA);
    }
}
