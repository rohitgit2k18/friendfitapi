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
    
    public partial class ScheduleDetailsByUserId_Result
    {
        public long ScheduleId { get; set; }
        public Nullable<bool> Monday { get; set; }
        public Nullable<bool> Tuesday { get; set; }
        public Nullable<bool> Wednesday { get; set; }
        public Nullable<bool> Thursday { get; set; }
        public Nullable<bool> Friday { get; set; }
        public Nullable<bool> Saturday { get; set; }
        public Nullable<bool> Sunday { get; set; }
        public Nullable<int> RecurrenceId { get; set; }
        public string ScheduleTime { get; set; }
        public Nullable<System.TimeSpan> TextMeTime { get; set; }
        public Nullable<System.TimeSpan> TextFriendTime { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> WorkoutId { get; set; }
        public string Description { get; set; }
    }
}
