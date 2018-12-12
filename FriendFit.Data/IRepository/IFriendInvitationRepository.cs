using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.IRepository
{
   public interface IFriendInvitationRepository
    {
        int AddFriendInvitation(AddFriendInvitationRequestModel objFriendInvitation);
        List<ListOfInvitedFriends> ListOfInvitedFriend(Int64 UserId);
    }
}
