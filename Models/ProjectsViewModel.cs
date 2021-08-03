using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GppApp.Models
{
    public class ProjectsViewModel
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string ItemDescription { get; set; }
        public string ProjectNotes { get; set; }
        public string Image { get; set; }
        public Nullable<bool> Checked { get; set; }
        public Nullable<int> Status { get; set; }
        public DateTime? ProCreatedOn { get; set; }
        public string CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    }
}