using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.ApiModel.APIResponseModel;
using FriendFit.Data.Filters;
using FriendFit.Data.IRepository;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace FriendFit.Data.Repository
{
    public class UserSettingsRepository : IUserSettingRepository
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        public LoginResult Login(LoginModelRequest objLoginModelRequest)
        {
            try
            {
                objLoginModelRequest.Password = CryptorEngine.Encrypt(objLoginModelRequest.Password, true);
                 
                LoginResult objUserProfile = _objFriendFitDBEntity.Database.SqlQuery<LoginResult>("LoginCustomer @Email=@Email,@Password=@Password",
                                      new SqlParameter("Email", objLoginModelRequest.Email),
                                      new SqlParameter("Password", objLoginModelRequest.Password)).FirstOrDefault();

                return objUserProfile;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public int UpdateToken(Int64 UserId)
        {
            string TokenCode = Guid.NewGuid().ToString() + UserId.ToString() + Guid.NewGuid().ToString();

            int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("UpdateToken @UserId=@UserId,@TokenCode=@TokenCode,@CreatedOn=@CreatedOn,@ExpiryDate=@ExpiryDate",
                                                                                new SqlParameter("UserId", UserId),
                                                                                new SqlParameter("TokenCode", TokenCode),
                                                                                new SqlParameter("CreatedOn", DateTime.Now),
                                                                                new SqlParameter("ExpiryDate", DateTime.Now.AddDays(7)));
            return rowEffected;
        }

        public int ResetPassword(ResetPasswordRequest objResetPasswordRequest,Int64 UserId)
        {
            try
            {
                objResetPasswordRequest.Password = CryptorEngine.Encrypt(objResetPasswordRequest.Password, true);
                int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Update UserProfile set Password=@Password where Id=@Id",
                                                                                   new SqlParameter("Password", objResetPasswordRequest.Password),
                                                                                   new SqlParameter("Id", UserId));

                return rowEffected;
            }
            catch(Exception ex)
            {
                return 0;
            }
            
        }

        public int AddUser(SignUpModelRequset objSignUpModelRequset)
        {
            objSignUpModelRequset.Password = CryptorEngine.Encrypt(objSignUpModelRequset.Password, true);
            int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("AddUsers @FirstName=@FirstName,@LastName=@LastName,@Email=@Email,@Password=@Password,@MobileNumber=@MobileNumber,@CountryId=@CountryId",                
                                                                                new SqlParameter("FirstName", objSignUpModelRequset.FirstName),
                                                                                new SqlParameter("LastName", objSignUpModelRequset.LastName),
                                                                                new SqlParameter("Email", objSignUpModelRequset.Email),
                                                                                new SqlParameter("Password", objSignUpModelRequset.Password),
                                                                                new SqlParameter("MobileNumber", objSignUpModelRequset.MobileNumber),
                                                                                new SqlParameter("CountryId", objSignUpModelRequset.CountryId));

            return rowEffected;
        }

        public UserDetailsModel EditUser(EditProfileRequestModel objEditProfileRequestModel)
        {

            UserDetailsModel UserDetails = _objFriendFitDBEntity.Database.SqlQuery<UserDetailsModel>("UserEditProfile @UserId=@UserId",
                                                                                 new SqlParameter("UserId", objEditProfileRequestModel.UserId)).FirstOrDefault();
            UserDetails.Password = CryptorEngine.Decrypt(UserDetails.Password, true);
            return UserDetails;
        }

        public int UpdateUser(UserUpdateModelRequest objUserUpdateModelRequest,Int64 UserId)
        {
            objUserUpdateModelRequest.Password = CryptorEngine.Encrypt(objUserUpdateModelRequest.Password, true);

            int rowEffected;
            if (objUserUpdateModelRequest.AutoSMSSignUp == null)
            {
                rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("UpdateUser @UserId=@UserId,@FirstName=@FirstName,@LastName=@LastName,@Email=@Email,@Password=@Password,@MobileNumber=@MobileNumber,@CountryId=@CountryId,@AutoSMSSignUp=@AutoSMSSignUp,@FullWorkoutStatus=@FullWorkoutStatus,@WorkoutStatus=@WorkoutStatus",
                                                                                                    new SqlParameter("UserId", UserId),
                                                                                                    new SqlParameter("FirstName", objUserUpdateModelRequest.FirstName),
                                                                                                    new SqlParameter("LastName", objUserUpdateModelRequest.LastName),
                                                                                                    new SqlParameter("Email", objUserUpdateModelRequest.Email),
                                                                                                    new SqlParameter("Password", (Object)objUserUpdateModelRequest.Password ?? DBNull.Value),
                                                                                                    new SqlParameter("MobileNumber", objUserUpdateModelRequest.MobileNumber),
                                                                                                    new SqlParameter("CountryId", objUserUpdateModelRequest.CountryId),
                                                                                                    new SqlParameter("AutoSMSSignUp", false),
                                                                                                    new SqlParameter("FullWorkoutStatus", objUserUpdateModelRequest.FullWorkoutStatus),
                                                                                                    new SqlParameter("WorkoutStatus", objUserUpdateModelRequest.WorkoutStatus));
            }
            else
            {
                rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("UpdateUser @UserId=@UserId,@FirstName=@FirstName,@LastName=@LastName,@Email=@Email,@Password=@Password,@MobileNumber=@MobileNumber,@CountryId=@CountryId,@AutoSMSSignUp=@AutoSMSSignUp,@FullWorkoutStatus=@FullWorkoutStatus,@WorkoutStatus=@WorkoutStatus",
                                                                                    new SqlParameter("UserId", UserId),
                                                                                    new SqlParameter("FirstName", objUserUpdateModelRequest.FirstName),
                                                                                    new SqlParameter("LastName", objUserUpdateModelRequest.LastName),
                                                                                    new SqlParameter("Email", objUserUpdateModelRequest.Email),
                                                                                    new SqlParameter("Password", (Object)objUserUpdateModelRequest.Password ?? DBNull.Value),
                                                                                    new SqlParameter("MobileNumber", objUserUpdateModelRequest.MobileNumber),
                                                                                    new SqlParameter("CountryId", objUserUpdateModelRequest.CountryId),
                                                                                    new SqlParameter("AutoSMSSignUp", objUserUpdateModelRequest.AutoSMSSignUp),
                                                                                    new SqlParameter("FullWorkoutStatus", objUserUpdateModelRequest.FullWorkoutStatus),
                                                                                    new SqlParameter("WorkoutStatus", objUserUpdateModelRequest.WorkoutStatus));
            }
            return rowEffected;
        }


        public int UpdateUserimperial(ImperialUpdateModelRequest objImperialUpdateModelRequest, Int64 UserId)
        {
            int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("UpdateUserImperial @UserId=@UserId,@DistenceImperial=@DistenceImperial,@WeightImperial=@WeightImperial",
                                                                                new SqlParameter("UserId", UserId),
                                                                                new SqlParameter("DistenceImperial", objImperialUpdateModelRequest.DistenceImperial),                                                                    
                                                                                new SqlParameter("WeightImperial", objImperialUpdateModelRequest.WeightImperial));
            return rowEffected;
        }
        //public CountryListModel CountryList()
        //{
        //    CountryListModel coun = new CountryListModel();

        //   coun.countryList = _objFriendFitDBEntity.Database.SqlQuery<CountryListModel>("GetCountryList").ToList();
        //    return coun;
        //}\\\



        public int LogoutUser(Int64 UserId)
        {
            try
            {
                int value = _objFriendFitDBEntity.Database.ExecuteSqlCommand("Logout @UserId=@UserId",
                            new SqlParameter("UserId", UserId));
                return value;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public AppUserDetails DetailsOfUser(Int64 UserId)
        {
            try
            {

            }
            catch(Exception ex)
            {
                return null;
            }
            return null;
        }

    }
}
