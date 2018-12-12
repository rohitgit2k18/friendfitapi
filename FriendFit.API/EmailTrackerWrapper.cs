using FriendFit.Data;
using FriendFit.Data.ApiModel.APIRequestModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FriendFit.API
{
    public class EmailTrackerWrapper
    {
        private FriendFitDBContext _objFriendFitDBEntity = null;

        public EmailTrackerWrapper()
        {
            _objFriendFitDBEntity = new FriendFitDBContext();
        }

        public async Task<int> EmailTimeSave(EmailTimeSaveModel objreq)
        {
            int result = await Task<int>.Run(() =>
            {
                return _objFriendFitDBEntity.Database.ExecuteSqlCommandAsync("AddMailSentTime @UserId=@UserId,@ResetMail=@ResetMail,@VerifyMail=@VerifyMail,@MailSentTime=@MailSentTime",
                                                                   new SqlParameter("UserId", objreq.UserId),
                                                                    new SqlParameter("ResetMail", objreq.@ResetMail),
                                                                    new SqlParameter("VerifyMail", objreq.VerifyMail),
                                                                    new SqlParameter("MailSentTime", objreq.MailSentTime));
            });
            return result;
        }
    }
}