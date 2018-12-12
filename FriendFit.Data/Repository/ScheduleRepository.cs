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
    public class ScheduleRepository : IScheduleRepository
    {
        private FriendFitDBContext _objFriendFitDBEntity = new FriendFitDBContext();

        public int AddSchedule(AddScheduleRequestModel objAddScheduleRequestModel, Int64 UserId)
        {
            //TimeZoneInfo GetUTCTime = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Mean Time");
            //DateTime UTCTime = DateTime.UtcNow;
            //DateTime localDateTime = DateTime.Now.ToLocalTime();
            //TimeSpan dd = (UTCTime - localDateTime);

            //TimeZoneInfo timeZoneInfo_india = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime India = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo_india);
            //DateTime localDateTime = DateTime.Now.ToLocalTime();
            //TimeSpan dd = (India - localDateTime);
            TimeSpan newtime;
            TimeSpan span3;

            //var GetCountryId = _objFriendFitDBEntity.UserProfiles.Where(s => s.Id == UserId).FirstOrDefault();
            var GetCountryId = _objFriendFitDBEntity.UserProfiles.Where(s => s.Id == UserId).FirstOrDefault();
            if (GetCountryId.CountryId == 91)
            {
                span3 = TimeSpan.FromMinutes(0);
                newtime = objAddScheduleRequestModel.ScheduleTime;
            }
            else
            {
                span3 = TimeSpan.FromMinutes(270);
                newtime = objAddScheduleRequestModel.ScheduleTime.Subtract(span3);
                if (newtime.TotalDays >= 1)
                {
                    span3 = TimeSpan.FromDays(1);
                    TimeSpan span5 = TimeSpan.FromMinutes(270);
                    TimeSpan newtime1 = objAddScheduleRequestModel.ScheduleTime.Subtract(span3);
                    newtime = newtime1.Subtract(span5);
                }
            }

            //var GetCountryId = _objFriendFitDBEntity.UserProfiles.Where(s => s.Id == UserId).FirstOrDefault();

            //switch (GetCountryId.CountryId)
            //{
            //    case 1: //canada
            //        span3 = TimeSpan.FromMinutes(300);
            //        break;
            //    case 2:
            //        span3 = TimeSpan.FromMinutes(-330);
            //        break;
            //    case 44: //UK
            //        span3 = TimeSpan.FromMinutes(0);
            //        break;
            //    case 61: //australia
            //        span3 = TimeSpan.FromMinutes(-600);
            //        break;
            //    case 91: //india
            //        span3 = TimeSpan.FromMinutes(-330);
            //        break;
            //    case 973: //BAHRAIN 
            //        span3 = TimeSpan.FromMinutes(-180);
            //        break;
            //    default:
            //        span3 = TimeSpan.FromMinutes(-330);
            //        break;
            //}            
            //TimeSpan TImeSp = objAddScheduleRequestModel.ScheduleTime;
            //TimeSpan Start_UTCTime = TImeSp.Add(span3);

            //DateTime time = DateTime.Today.Add(newtime);
            //string displayTime = time.ToString("hh:mm tt");
            //TimeSpan GetScdeduleTime = Convert.ToDateTime(displayTime).TimeOfDay;

            WorkOutSchedule objWorkOutSchedule = new WorkOutSchedule()
            {
                Monday = objAddScheduleRequestModel.Monday,
                Tuesday = objAddScheduleRequestModel.Tuesday,
                Wednesday = objAddScheduleRequestModel.Wednesday,
                Thursday = objAddScheduleRequestModel.Thursday,
                Friday = objAddScheduleRequestModel.Friday,
                Saturday = objAddScheduleRequestModel.Saturday,
                Sunday = objAddScheduleRequestModel.Sunday,
                RecurrenceId = objAddScheduleRequestModel.RecurrenceId,
                ScheduleTime = objAddScheduleRequestModel.ScheduleTime,
                ScheduleTime_UTC = newtime,
                TextMeTime = objAddScheduleRequestModel.TextMeTime,
                TextFriendTime = objAddScheduleRequestModel.TextFriendTime,
                StartDate = objAddScheduleRequestModel.StartDate,
                EndDate = objAddScheduleRequestModel.EndDate,
                CreatedDate = DateTime.Now,
                CreatedBy = UserId,
                IsActive = true,
                RowStatus = false,
                UserId = UserId,
                WorkoutId = 0,
                WorkoutText = objAddScheduleRequestModel.WorkoutText
            };
            _objFriendFitDBEntity.WorkOutSchedules.Add(objWorkOutSchedule);
            _objFriendFitDBEntity.SaveChanges();


            Int64 workoutId = _objFriendFitDBEntity.WorkOutSchedules.Where(x => x.WorkoutText == objWorkOutSchedule.WorkoutText).FirstOrDefault().Id;
            var BulkInsert = _objFriendFitDBEntity.InsertBulkworkout(Convert.ToInt32(workoutId));
            return Convert.ToInt32(workoutId);
        }

        public List<ScheduleList> ScheduleList(Int64 UserId)
        {
            List<ScheduleList> obj = new List<ApiModel.APIResponseModel.ScheduleList>();
            try
            {

                obj = _objFriendFitDBEntity.Database.SqlQuery<ScheduleList>("ListOfWorkSchedule @UserId=@UserId",
                                                                new SqlParameter("UserId", UserId)).ToList();

                //var itemToRemove = obj.Single(r => r.Monday == false);
                //obj.Remove(itemToRemove);


            }
            catch (Exception ex)
            {
                return null;
            }
            return obj;
        }

        public ScheduleDetails ScheduleDetails(Int64 UserId, Int64 ScheduleId)
        {
            try
            {
                var model = _objFriendFitDBEntity.Database.SqlQuery<ScheduleDetails>("ScheduleDetailsByUserId @UserId=@UserId,@ScheduleId=@ScheduleId",
                                                                            new SqlParameter("UserId", UserId),
                                                                            new SqlParameter("ScheduleId", ScheduleId)).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                return null;
                //return ex.Message();
            }
        }

        public int UpdateScheduleWorkout(UpdateScheduleWorkoutRequestModel objUpdateScheduleWorkoutRequestModel, Int64 ScheduleId, Int64 UserId)
        {
            try
            {
                int rowEffected = _objFriendFitDBEntity.Database.ExecuteSqlCommand("UpdateScheduleWorkout @ScheduleId=@ScheduleId,@UserId=@UserId,@Monday=@Monday,@Tuesday=@Tuesday,@Wednesday=@Wednesday,@Thursday = @Thursday,@Friday = @Friday,@Saturday = @Saturday,@Sunday = @Sunday,@RecurrenceId = @RecurrenceId,@ScheduleTime = @ScheduleTime,@TextMeTime = @TextMeTime,@TextFriendTime = @TextFriendTime,@StartDate = @StartDate,@EndDate = @EndDate",
                                                                                    new SqlParameter("ScheduleId", ScheduleId),
                                                                                    new SqlParameter("UserId", UserId),
                                                                                    new SqlParameter("Monday", objUpdateScheduleWorkoutRequestModel.Monday),
                                                                                    new SqlParameter("Tuesday", objUpdateScheduleWorkoutRequestModel.Tuesday),
                                                                                    new SqlParameter("Wednesday", objUpdateScheduleWorkoutRequestModel.Wednesday),
                                                                                    new SqlParameter("Thursday", objUpdateScheduleWorkoutRequestModel.Thursday),
                                                                                    new SqlParameter("Friday", objUpdateScheduleWorkoutRequestModel.Friday),
                                                                                    new SqlParameter("Saturday", objUpdateScheduleWorkoutRequestModel.Saturday),
                                                                                    new SqlParameter("Sunday", objUpdateScheduleWorkoutRequestModel.Sunday),
                                                                                    new SqlParameter("RecurrenceId", objUpdateScheduleWorkoutRequestModel.RecurrenceId),
                                                                                    new SqlParameter("ScheduleTime", objUpdateScheduleWorkoutRequestModel.ScheduleTime),
                                                                                    new SqlParameter("TextMeTime", objUpdateScheduleWorkoutRequestModel.TextMeTime),
                                                                                    new SqlParameter("TextFriendTime", objUpdateScheduleWorkoutRequestModel.TextFriendTime),
                                                                                    new SqlParameter("StartDate", objUpdateScheduleWorkoutRequestModel.StartDate),
                                                                                    new SqlParameter("EndDate", objUpdateScheduleWorkoutRequestModel.EndDate));
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        public int DeleteSchedule(long ScheduleId, long UserId)
        {
            int Response;
            try
            {
                var UpWorkOut = _objFriendFitDBEntity.WorkOuts.Where(x => x.UserId == UserId && x.ScheduleId == ScheduleId).ToList();
                foreach (var item in UpWorkOut)
                {
                    var _UpWorkOut = _objFriendFitDBEntity.WorkOuts.Where(x => x.Id == item.Id).FirstOrDefault();
                    _UpWorkOut.IsActive = false;
                    _objFriendFitDBEntity.Entry(_UpWorkOut).State = System.Data.Entity.EntityState.Modified;
                    _objFriendFitDBEntity.SaveChanges();
                }
                var UpWorkOutSchedule = _objFriendFitDBEntity.WorkOutSchedules.Where(x => x.UserId == UserId && x.Id == ScheduleId).FirstOrDefault();
                UpWorkOutSchedule.IsActive = false;
                _objFriendFitDBEntity.Entry(UpWorkOutSchedule).State = System.Data.Entity.EntityState.Modified;
                _objFriendFitDBEntity.SaveChanges();
                Response = 1;
            }
            catch (Exception ex)
            {
                //Response = Convert.ToString(ex);
                Response = 0;
            }
            return Response;
        }
    }
}
