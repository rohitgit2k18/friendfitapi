//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FriendFit.Data
{
    using System;
    
    public partial class ListOfWorkSchedule_Result
    {
        public long ScheduleId { get; set; }
        public Nullable<long> UserId { get; set; }
        public Nullable<int> RecurrenceId { get; set; }
        public string ScheduleTime { get; set; }
        public Nullable<System.TimeSpan> TextMeTime { get; set; }
        public Nullable<System.TimeSpan> TextFriendTime { get; set; }
        public string Schedule { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> WorkoutId { get; set; }
        public string Description { get; set; }
        public string WorkoutStartTime { get; set; }
        public string WorkoutFinishTime { get; set; }
        public Nullable<bool> AutoFinishTime { get; set; }
        public Nullable<bool> ScheduleOrNot { get; set; }
    }
}