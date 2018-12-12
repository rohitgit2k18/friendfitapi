using FriendFit.Data.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.IRepository
{
    public interface ISendWorkOutSMSRepository
    {
        List<ListOfInvitedUserSMS> SendNotificationBeforeWorkoutToUser();
        List<ListOfInvitedUserSMS> SendEmailBeforeWorkoutToUser();
        List<ListOfInvitedUserSMS> SendSMSBeforeWorkoutToUser();
        List<ListOfInvitedUserSMS> SendEmailBeforeWorkoutToFriends();
        List<ListOfInvitedUserSMS> SendSMSBeforeWorkoutToFriends();
        List<ListOfInvitedUserSMS> SendNotificationBeforeWorkoutToFriends();
        List<ListOfInvitedUserSMS> SendEmailToUserWhenWorkoutMissed();
        List<ListOfInvitedUserSMS> SendSMSToUserWhenWorkoutMissed();
        List<ListOfInvitedUserSMS> SendNotificationToUserWhenWorkoutMissed();
        List<ListOfInvitedUserSMS> SendEmailFriendsWhenWorkoutMissed();
        List<ListOfInvitedUserSMS> SendSMSFriendsWhenWorkoutMissed();
        List<ListOfInvitedUserSMS> SendNotificationFriendsWhenWorkoutMissed();
    }
}
