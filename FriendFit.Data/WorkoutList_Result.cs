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
    
    public partial class WorkoutList_Result
    {
        public long WorkoutId { get; set; }
        public Nullable<long> UserId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> WorkDate { get; set; }
        public string DateOfWorkout { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string WorkoutNotes { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Status { get; set; }
        public Nullable<bool> ScheduleWorkout { get; set; }
    }
}