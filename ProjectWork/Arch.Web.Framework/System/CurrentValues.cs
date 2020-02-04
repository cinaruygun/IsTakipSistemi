using System.Linq;
using Arch.Service.Interfaces;
using Arch.Service.Services;
using System.Collections.Generic;
using Arch.Dto.ListedDto;
using Arch.Dto.SingleDto;
using Arch.Core.Enums;

namespace System.Web.Mvc
{
    public static class CurrentValues
    {
        private static IUtilityService _utilityService
        {
            get
            {
                return DependencyResolver.Current.GetService<UtilityService>();
            }
        }
        public static int GetTaskCount(int? assigned)
        {
            return _utilityService.GetTaskCount(assigned);
        }
        public static string GetLookupName(short eventTypeId)
        {
            return Resources._lookupLists.Where(p => p.Id == eventTypeId).Select(p => p.Name).SingleOrDefault();
        }
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;
        public static string GetNameableDate(DateTime date)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "şimdi" : ts.Seconds + " saniye önce";

            if (delta < 2 * MINUTE)
                return "bir dakika önce";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " dakika önce";

            if (delta < 90 * MINUTE)
                return "bir saat önce";

            if (delta < 24 * HOUR)
                return ts.Hours + " saat önce";

            if (delta < 48 * HOUR)
                return "dün";

            if (delta < 30 * DAY)
                return ts.Days + " gün önce";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "bir ay önce" : months + " ay önce";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "bir yıl önce" : years + " yıl önce";
            }
        }
    }
}