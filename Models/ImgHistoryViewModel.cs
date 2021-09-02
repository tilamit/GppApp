using GppApp.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GppApp.Models
{
    public class ImgHistoryViewModel : ItemImages
    {
        public string UserName { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}