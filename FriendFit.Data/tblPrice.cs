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
    using System.Collections.Generic;
    
    public partial class tblPrice
    {
        public int Id { get; set; }
        public string Duration { get; set; }
        public Nullable<double> Recurring { get; set; }
        public Nullable<double> One_Off { get; set; }
        public string IsSMS { get; set; }
        public Nullable<System.DateTime> RowInsert { get; set; }
        public Nullable<System.DateTime> RowUpdate { get; set; }
    }
}