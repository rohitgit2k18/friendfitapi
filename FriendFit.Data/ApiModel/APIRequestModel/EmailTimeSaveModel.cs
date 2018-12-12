using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
    public class EmailTimeSaveModel
    {
        public Int64 UserId { get; set; }
        public bool ResetMail { get; set; }
        public bool VerifyMail { get; set; }
        public TimeSpan MailSentTime { get; set; }
    }
}
