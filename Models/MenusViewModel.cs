using GppApp.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRevision.Models
{
    public class MenusViewModel : Menus
    {
        public string AssignedMenu { get; set; }
        public Nullable<int> UserType { get; set; }
    }
}