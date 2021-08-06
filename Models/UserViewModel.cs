using GppApp.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GppApp.Models
{
    public class UserViewModel : UserDetails
    {
        public string UserTypes { get; set; }
        public string UserGender { get; set; }
    }
}