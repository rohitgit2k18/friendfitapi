using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.Repository
{
    public class FriendInvitationRepository : IFriendInvitationRepository
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();
        public int AddFriendInvitation(AddFriendInvitationRequestModel objFriendInvitation)
        {
            try
            {
                int GetMonths=0;
                string Months = objFriendInvitation.DurationId.ToString();
                if (Months == "1 month")
                {
                    GetMonths = 1;
                }
                else
                {
                    Months = objFriendInvitation.DurationId.ToString();
                    Months = Months.Replace(" months", "");
                    GetMonths = Convert.ToInt32(Months);
                }
                int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddFriendInvitation @UserId=@UserId,@DeliveryTypeId=@DeliveryTypeId,@FriendsName=@FriendsName,@DurationId=@DurationId,@PurchaseDate=@PurchaseDate,@ExpiryDate=@ExpiryDate,@SubscriptionTypeId=@SubscriptionTypeId,@Cost=@Cost,@Email=@Email,@MobileNumber=@MobileNumber,@CountryId=@CountryId",
                                                                                    new SqlParameter("UserId", objFriendInvitation.UserId),
                                                                                    new SqlParameter("DeliveryTypeId", objFriendInvitation.DeliveryTypeId),
                                                                                    new SqlParameter("FriendsName", objFriendInvitation.FriendsName),
                                                                                    new SqlParameter("DurationId", GetMonths),
                                                                                    new SqlParameter("PurchaseDate", objFriendInvitation.PurchaseDate),
                                                                                    new SqlParameter("ExpiryDate", objFriendInvitation.ExpiryDate),
                                                                                    new SqlParameter("SubscriptionTypeId", objFriendInvitation.SubscriptionTypeId),
                                                                                    new SqlParameter("Cost", objFriendInvitation.Cost),
                                                                                    new SqlParameter("Email", (Object)objFriendInvitation.Email ?? DBNull.Value),
                                                                                    new SqlParameter("MobileNumber", (Object)objFriendInvitation.MobileNumber ?? DBNull.Value),
                                                                                    new SqlParameter("CountryId", (Object)objFriendInvitation.CountryId ?? DBNull.Value));

            }
            catch (Exception ex)
            {

            }
            return 1;
        }

        public List<ListOfInvitedFriends> ListOfInvitedFriend(Int64 UserId)
        {
            try
            {
                List<ListOfInvitedFriends> list = _objFriendFitDBEntity.Database.SqlQuery<ListOfInvitedFriends>("ListOfFriends @UserId=@UserId",
                                                                                            new SqlParameter("UserId", UserId)).ToList();

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
