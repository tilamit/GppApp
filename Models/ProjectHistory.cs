using GppApp.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRevision.Models
{
    public class ProjectHistory : Projects
    {
        public string PreProjectName { get; set; }
        public string PreProjectDetails { get; set; }
        public Nullable<int> PreCreatedBy { get; set; }
        public string PersonCreated { get; set; }
        public Nullable<System.DateTime> PreCreatedOn { get; set; }
        public string PreProjectRef { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}