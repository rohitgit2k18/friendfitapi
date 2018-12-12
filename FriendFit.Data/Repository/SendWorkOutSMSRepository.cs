using FriendFit.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;

namespace FriendFit.Data.Repository
{
    public class SendWorkOutSMSRepository : ISendWorkOutSMSRepository
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        public List<ListOfInvitedUserSMS> SendEmailBeforeWorkoutToUser()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
           return ListofUserSMS.Response.UserInvitedList= _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                            new SqlParameter("Is", "SendEmailBeforeWorkoutToUser")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendSMSBeforeWorkoutToUser()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendSMSBeforeWorkoutToUser")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendNotificationBeforeWorkoutToUser()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendNotificationBeforeWorkoutToUser")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendEmailBeforeWorkoutToFriends()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendEmailBeforeWorkoutToFriends")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendSMSBeforeWorkoutToFriends()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendSMSBeforeWorkoutToFriends")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendNotificationBeforeWorkoutToFriends()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendNotificationBeforeWorkoutToFriends")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendEmailToUserWhenWorkoutMissed()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendEmailToUserWhenWorkoutMissed")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendSMSToUserWhenWorkoutMissed()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendSMSToUserWhenWorkoutMissed")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendNotificationToUserWhenWorkoutMissed()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendNotificationToUserWhenWorkoutMissed")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendEmailFriendsWhenWorkoutMissed()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendEmailFriendsWhenWorkoutMissed")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendSMSFriendsWhenWorkoutMissed()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendSMSFriendsWhenWorkoutMissed")).ToList();
        }

        public List<ListOfInvitedUserSMS> SendNotificationFriendsWhenWorkoutMissed()
        {
            ListOfInvitedUserSMSResponse ListofUserSMS = new ListOfInvitedUserSMSResponse();
            return ListofUserSMS.Response.UserInvitedList = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedUserSMS>("GetUserSendSMSList @Is=@Is",
                                                             new SqlParameter("Is", "SendNotificationFriendsWhenWorkoutMissed")).ToList();
        }


    }
}
