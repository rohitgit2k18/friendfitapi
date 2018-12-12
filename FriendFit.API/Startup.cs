using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FriendFit.API.Controllers;
using FriendFit.API.Models;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FriendFit.API.Startup))]

namespace FriendFit.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //string constr = ConfigurationManager.ConnectionStrings["FriendFitDBContext"].ConnectionString;
            //GlobalConfiguration.Configuration.UseSqlServerStorage("MyHangFire");
            //app.UseHangfireDashboard();
            //app.UseHangfireServer();
            //ConfigureAuth(app);
            ConfigureAuth(app);
            string constr = ConfigurationManager.ConnectionStrings["FriendFitDBContext"].ConnectionString;
            GlobalConfiguration.Configuration.UseSqlServerStorage("MyHangFire");
            app.UseHangfireDashboard("/jobs");

            string Minutes = Cron.Minutely();
            SendWorkoutSMSController job = new SendWorkoutSMSController();
            RecurringJob.AddOrUpdate(() => job.SendSMSBeforeWorkoutToUser(Minutes), Minutes);
            //RecurringJob.AddOrUpdate(() => job.SendSMSBeforeWorkoutToFriends(Minutes), Minutes);
            RecurringJob.AddOrUpdate(() => job.SendSMSToUserWhenWorkoutMissed(Minutes), Minutes);
            //RecurringJob.AddOrUpdate(() => job.SendSMSFriendsWhenWorkoutMissed(Minutes), Minutes);

            RecurringJob.AddOrUpdate(() => job.SendEmailBeforeWorkoutToUser(Minutes), Minutes);
            //RecurringJob.AddOrUpdate(() => job.SendEmailBeforeWorkoutToFriends(Minutes), Minutes);
            RecurringJob.AddOrUpdate(() => job.SendEmailToUserWhenWorkoutMissed(Minutes), Minutes);
            //RecurringJob.AddOrUpdate(() => job.SendEmailFriendsWhenWorkoutMissed(Minutes), Minutes);

            RecurringJob.AddOrUpdate(() => job.SendNotificationBeforeWorkoutToUser(Minutes), Minutes);
            //RecurringJob.AddOrUpdate(() => job.SendNotificationBeforeWorkoutToFriends(Minutes), Minutes);
            RecurringJob.AddOrUpdate(() => job.SendNotificationToUserWhenWorkoutMissed(Minutes), Minutes);
            //RecurringJob.AddOrUpdate(() => job.SendNotificationFriendsWhenWorkoutMissed(Minutes), Minutes);
            app.UseHangfireServer();
        }
    }
}
