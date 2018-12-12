using FriendFit.API.Controllers;
using Hangfire;
using Hangfire.Server;
using Owin;

namespace AppointmentReminders.Web
{
    public class Hangfire 
    {     

        public static void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("MyHangFire");
            app.UseHangfireDashboard("/jobs");
            app.UseHangfireServer();
        }

        public static void InitializeJobs()
        {
           string Minutes = Cron.Minutely();
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendSMSBeforeWorkoutToUser(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendSMSBeforeWorkoutToFriends(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendSMSToUserWhenWorkoutMissed(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendSMSFriendsWhenWorkoutMissed(Minutes), Minutes);

            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendEmailBeforeWorkoutToUser(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendEmailBeforeWorkoutToFriends(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendEmailToUserWhenWorkoutMissed(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendEmailFriendsWhenWorkoutMissed(Minutes), Minutes);

            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendNotificationBeforeWorkoutToUser(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendNotificationBeforeWorkoutToFriends(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendNotificationToUserWhenWorkoutMissed(Minutes), Minutes);
            RecurringJob.AddOrUpdate<SendWorkoutSMSController>(job => job.SendNotificationFriendsWhenWorkoutMissed(Minutes), Minutes);
        }
    }
}