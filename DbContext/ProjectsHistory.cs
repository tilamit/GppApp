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
    
    public partial class ProjectsHistory
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string PreviousName { get; set; }
        public string PreviousRef { get; set; }
        public string PreProjectDetails { get; set; }
        public Nullable<int> PreCreatedBy { get; set; }
        public Nullable<System.DateTime> PreCreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
