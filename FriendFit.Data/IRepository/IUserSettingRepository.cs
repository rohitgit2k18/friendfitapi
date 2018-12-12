using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.IRepository
{
    public interface IUserSettingRepository
    {
        LoginResult Login(LoginModelRequest objLoginModelRequest);
        int UpdateToken(Int64 UserId);
        int ResetPassword(ResetPasswordRequest objResetPasswordRequest, Int64 UserId);
        int AddUser(SignUpModelRequset objSignUpModelRequset);
        UserDetailsModel EditUser(EditProfileRequestModel objEditProfileRequestModel);
        int UpdateUser(UserUpdateModelRequest objUserUpdateModelRequest, Int64 UserId);
        int UpdateUserimperial(ImperialUpdateModelRequest objImperialUpdateModelRequest, Int64 UserId);

        
        //CountryListModel CountryList();

        int LogoutUser(Int64 UserId);

        AppUserDetails DetailsOfUser(Int64 UserId);



    }
}
