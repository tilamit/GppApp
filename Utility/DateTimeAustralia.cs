using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GppApp.Utility
{
    public static class DateTimeAustralia
    {
        public static DateTime GetDateTime()
        {
            //Australia Time
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo ausZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, ausZone);

            return localDateTime;
        }
    }
}