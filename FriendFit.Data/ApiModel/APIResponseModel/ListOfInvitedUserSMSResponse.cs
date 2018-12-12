using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class ListOfInvitedUserSMSResponse
    {
        public ListOfInvitedUserSMSResponse()
        {
            Response = new ListOfInvitedUserSMSModel();
        }
        public ListOfInvitedUserSMSModel Response { get; set; }
    }

    public class ListOfInvitedUserSMS
    {
        public long WorkoutId { get; set; }
        public int StatusId { get; set; }
        public string FirstName { get; set; }
        public string MobileNumber { get; set; }
        public long CountryId { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }

        public DateTime? DateofWOrkout { get; set; }

        public string StartTime { get; set; }
        public string StartTime_Actual { get; set; }
        public string FinishTime { get; set; }

        public string SendTime_TextMe { get; set; }
        public string SendTime_TextFriend { get; set; }

        public string MissedTime_TextMe { get; set; }
        public string MissedTime_TextFriend { get; set; }
    }

    public class ListOfInvitedUserSMSModel
    {
        public ListOfInvitedUserSMSModel()
        {
            UserInvitedList = new List<APIResponseModel.ListOfInvitedUserSMS>();
        }
        public List<ListOfInvitedUserSMS> UserInvitedList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }


    }
}
