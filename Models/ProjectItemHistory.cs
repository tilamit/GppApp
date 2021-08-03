using GppApp.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GppApp.Models
{
    public class ProjectItemHistory : ProjectItems
    {
        public string PreItemDescription { get; set; }
        public string PreProjectNotes { get; set; }
        public string PreImage { get; set; }
        public Nullable<bool> PreChecked { get; set; }
        public Nullable<int> PreStatus { get; set; }
        public Nullable<System.DateTime> PreCreatedOn { get; set; }
        public Nullable<int> PreCreatedBy { get; set; }

        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string PersonCreated { get; set; }
    }
}