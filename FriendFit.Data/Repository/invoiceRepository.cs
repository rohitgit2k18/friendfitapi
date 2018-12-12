using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.IRepository;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using NReco.PdfGenerator;
using PageSize = iTextSharp.text.PageSize;

namespace FriendFit.Data.Repository
{
    public class invoiceRepository : IinvoiceRepository
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        public string DownloadApplicationPDF(EmailModelAttachment EMA)
        {
            try
            {
                if (System.IO.File.Exists(EMA.FileURL))
                {
                    System.IO.File.Delete(EMA.FileURL);
                }
                string strInvoiceComReceipt = string.Empty;
                System.IO.StreamReader strReader;
                strReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"/Invoice.html"));
                strInvoiceComReceipt = strReader.ReadToEnd();
                strReader.Close();
                strReader.Dispose();
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("TodayDate", DateTime.Now.ToString("MM/dd/yyyy"));
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("CustomerName", EMA.CustomerName);
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("IsSMS", EMA.IsSMS);
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("Isrecurringmonthly", EMA.Isrecurringmonthly);
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("ProductAmount", EMA.ProductAmount);
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("includingGST", EMA.includingGST);
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("TotalAmount", EMA.TotalAmount);
                string htmlContent1 = strInvoiceComReceipt;

                string path = System.Web.HttpContext.Current.Server.MapPath(@"/Invoice/" + EMA.PayPalPaymentId + ".pdf");

                byte[] pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(htmlContent1);
                System.IO.File.WriteAllBytes(path, pdfBytes);
                return "File Saved in Inovice Folder";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string DownloadApplicationPDF_Friend(EmailModelAttachment EMA)
        {
            try
            {
                if (System.IO.File.Exists(EMA.FileURL))
                {
                    System.IO.File.Delete(EMA.FileURL);
                }
                string strInvoiceComReceipt = string.Empty;
                System.IO.StreamReader strReader;
                strReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"/FriendsInvoice.html"));
                strInvoiceComReceipt = strReader.ReadToEnd();
                strReader.Close();
                strReader.Dispose();
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("TodayDate", DateTime.Now.ToString("MM/dd/yyyy"));
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("CustomerName", EMA.CustomerName);
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("FriendNotification", EMA.FriendsHTML);
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("includingGST", EMA.includingGST);
                strInvoiceComReceipt = strInvoiceComReceipt.Replace("TotalAmount", EMA.TotalAmount);

                string htmlContent1 = strInvoiceComReceipt;
                string path = System.Web.HttpContext.Current.Server.MapPath(@"/FriendInvoice/" + EMA.PayPalPaymentId + ".pdf");

                byte[] pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(htmlContent1);
                System.IO.File.WriteAllBytes(path, pdfBytes);
                return "File Saved in Friend Inovice Folder";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SendEmailWithAttachment(EmailModelAttachment EMA)
        {
            string Result = "";
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("noreply@noti.fit");
                mail.To.Add(EMA.ToEmail);
                mail.Subject = EMA.Subject;
                mail.Body = "Hi "+ EMA.CustomerName + ", Please find attached the Invoice.";
                Attachment at = new Attachment(HttpContext.Current.Server.MapPath(EMA.FileURL));
                mail.Attachments.Add(at);
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("testifiedemail@gmail.com", "testifiedpassword@hackfree");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                Result = "Ok";
            }
            catch (Exception ex)
            {
                Result = Convert.ToString(ex);
            }
            return Result;
        }

    }
}
