//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GppApp.DbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemsConfirmed
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public Nullable<int> ProjectItemId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
        public string Details { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> SystemDate { get; set; }
    }
}
